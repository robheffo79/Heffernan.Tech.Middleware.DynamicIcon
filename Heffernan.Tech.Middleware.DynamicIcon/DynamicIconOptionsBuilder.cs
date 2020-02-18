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

using System;

namespace Heffernan.Tech.Middleware.DynamicIcon
{
    public class DynamicIconOptionsBuilder
    {
        private String useRoute;
        private String defaultBackground;
        private String defaultForeground;
        private String fontName;
        private Int32? minimumSize;
        private Int32? maximumSize;
        private Int32? defaultSize;
        private IconFormat? iconFormat;
        private TimeSpan? cacheFor;

        public DynamicIconOptionsBuilder UseDefaultForeground(String foreground)
        {
            if (foreground == null)
                throw new ArgumentNullException(nameof(foreground));

            if (String.IsNullOrWhiteSpace(foreground))
                throw new ArgumentOutOfRangeException(nameof(foreground));

            defaultForeground = foreground;
            return this;
        }

        public DynamicIconOptionsBuilder UseDefaultBackground(String background)
        {
            if (background == null)
                throw new ArgumentNullException(nameof(background));

            if (String.IsNullOrWhiteSpace(background))
                throw new ArgumentOutOfRangeException(nameof(background));

            defaultBackground = background;
            return this;
        }

        public DynamicIconOptionsBuilder UseFontName(String font)
        {
            if (font == null)
                throw new ArgumentNullException(nameof(font));

            if (String.IsNullOrWhiteSpace(font))
                throw new ArgumentOutOfRangeException(nameof(font));

            fontName = font;
            return this;
        }

        public DynamicIconOptionsBuilder UseRoute(String route)
        {
            if (route == null)
                throw new ArgumentNullException(nameof(route));

            if (String.IsNullOrWhiteSpace(route))
                throw new ArgumentOutOfRangeException(nameof(route));

            useRoute = route;

            return this;
        }

        public DynamicIconOptionsBuilder UseMinimumSize(Int32 size)
        {
            if (size < DynamicIconMiddleware.MINIMUM_SIZE || size > DynamicIconMiddleware.MAXIMUM_SIZE)
                throw new ArgumentOutOfRangeException(nameof(size));

            minimumSize = size;

            return this;
        }

        public DynamicIconOptionsBuilder UseMaximumSize(Int32 size)
        {
            if (size < DynamicIconMiddleware.MINIMUM_SIZE || size > DynamicIconMiddleware.MAXIMUM_SIZE)
                throw new ArgumentOutOfRangeException(nameof(size));

            maximumSize = size;

            return this;
        }

        public DynamicIconOptionsBuilder UseDefaultFormat(IconFormat format)
        {
            if (Enum.IsDefined(typeof(IconFormat), format) == false)
                throw new ArgumentOutOfRangeException(nameof(format));

            iconFormat = format;

            return this;
        }

        public DynamicIconOptionsBuilder UseCacheTime(TimeSpan? cacheTime)
        {
            if (cacheTime != null && cacheTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(cacheTime));

            cacheFor = cacheTime;
            return this;
        }

        public DynamicIconOptionsBuilder UseDefaultSize(Int32 size)
        {
            if (size < DynamicIconMiddleware.MINIMUM_SIZE || size > DynamicIconMiddleware.MAXIMUM_SIZE)
                throw new ArgumentOutOfRangeException(nameof(size));

            defaultSize = size;
            return this;
        }

        public DynamicIconOptions Build()
        {
            DynamicIconOptions options = new DynamicIconOptions()
            {
                Route = useRoute ?? DynamicIconMiddleware.DEFAULT_ROUTE,
                MaximumSize = maximumSize ?? DynamicIconMiddleware.MAXIMUM_SIZE,
                MinimumSize = minimumSize ?? DynamicIconMiddleware.MINIMUM_SIZE,
                Format = iconFormat ?? DynamicIconMiddleware.DEFAULT_FORMAT,
                DefaultBackground = defaultBackground ?? DynamicIconMiddleware.DEFAULT_BACKGROUND,
                DefaultForeground = defaultForeground ?? DynamicIconMiddleware.DEFAULT_FOREGROUND,
                DefaultSize = defaultSize ?? DynamicIconMiddleware.DEFAULT_SIZE,
                FontName = fontName ?? DynamicIconMiddleware.DEFAULT_FONT,
                CacheFor = cacheFor ?? new TimeSpan(DynamicIconMiddleware.DEFAULT_CACHE)
            };

            if (options.MinimumSize > options.MinimumSize)
                throw new InvalidOperationException("Minimum size is greater than Maximum size.");

            if (options.DefaultSize < options.MinimumSize)
                throw new InvalidOperationException("Default size is less than Minimum size.");

            if (options.DefaultSize > options.MaximumSize)
                throw new InvalidOperationException("Default size is greater than Maximum size.");

            return options;
        }
    }
}
