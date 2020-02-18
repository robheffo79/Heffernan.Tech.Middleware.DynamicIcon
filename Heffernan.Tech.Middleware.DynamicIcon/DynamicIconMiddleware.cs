/*
MIT License

Copyright (c) 2020 Robert Heffernan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using Heffernan.Tech.Middleware.DynamicIcon.Renderer;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace Heffernan.Tech.Middleware.DynamicIcon
{
    public class DynamicIconMiddleware
    {
        public const String DEFAULT_FONT = "Arial";
        public const String DEFAULT_ROUTE = "/images/dynamicicon";
        public const String DEFAULT_BACKGROUND = "#00137F";
        public const String DEFAULT_FOREGROUND = "#FFFFFF";
        public const Int32 MINIMUM_SIZE = 32;
        public const Int32 MAXIMUM_SIZE = 4096;
        public const Int32 DEFAULT_SIZE = 64;
        public const IconFormat DEFAULT_FORMAT = IconFormat.Png;
        public const Int64 DEFAULT_CACHE = 31556926000000000;

        private readonly RequestDelegate _next;
        private readonly DynamicIconOptions _options;

        public DynamicIconMiddleware(RequestDelegate next, DynamicIconOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Request.Path == _options.Route)
            {
                if (context.Request.Method == "GET" || context.Request.Method == "HEAD")
                {
                    await BuildLetterImage(context);
                    return;
                }

                context.Response.StatusCode = 405;
                await context.Response.WriteAsync("Method Not Allowed");
            }

            await _next(context);
        }

        private async Task BuildLetterImage(HttpContext context)
        {
            IIconRenderer renderer = GetRenderer(_options.Format);
            renderer.Text = GetText(context);

            Byte[] bytes = await renderer.Render();

            SetHeaders(context, bytes.Length);

            await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }

        private IIconRenderer GetRenderer(IconFormat format)
        {
            IIconRenderer renderer = null;

            switch(format)
            {
                case IconFormat.Gif:
                case IconFormat.Ico:
                case IconFormat.Png:
                    renderer = new RasterIconRenderer();
                    break;

                case IconFormat.Svg:
                    renderer = new VectorIconRenderer();
                    break;
            }

            renderer.Size = _options.DefaultSize;
            renderer.FontName = _options.FontName;
            renderer.Background = _options.DefaultBackground;
            renderer.Foreground = _options.DefaultForeground;
            renderer.ImageFormat = GetFormat(_options.Format);

            return renderer;
        }

        private ImageFormat GetFormat(IconFormat format)
        {
            switch (format)
            {
                case IconFormat.Gif:
                    return ImageFormat.Gif;

                case IconFormat.Ico:
                    return ImageFormat.Icon;

                case IconFormat.Png:
                    return ImageFormat.Png;
            }

            return null;
        }

        private void SetHeaders(HttpContext context, Int32 length)
        {
            switch (_options.Format)
            {
                case IconFormat.Gif:
                    context.Response.ContentType = "image/gif";
                    break;

                case IconFormat.Ico:
                    context.Response.ContentType = "image/vnd.microsoft.icon";
                    break;

                case IconFormat.Png:
                    context.Response.ContentType = "image/png";
                    break;
            }

            context.Response.ContentLength = length;

            if(_options.CacheFor.HasValue && _options.CacheFor > TimeSpan.Zero)
            {
                context.Response.Headers["cache-control"] = $"max-age={(Int64)_options.CacheFor.Value.TotalSeconds}";
                context.Response.Headers["expires"] = DateTime.Now.Add(_options.CacheFor.Value).ToUniversalTime().ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'");
            }
        }

        private String GetText(HttpContext context)
        {
            if (context.Request.Query.ContainsKey("t"))
            {
                return context.Request.Query["t"].First();
            }

            return null;
        }
    }

    public enum IconFormat
    {
        Ico = 1,
        Png = 2,
        Gif = 3,
        Svg = 4
    }
}
