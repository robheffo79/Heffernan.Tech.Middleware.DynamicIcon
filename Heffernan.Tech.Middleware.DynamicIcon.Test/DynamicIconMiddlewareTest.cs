using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Heffernan.Tech.Middleware.DynamicIcon.Test
{
    [TestClass]
    public class DynamicIconMiddlewareTest
    {
        [TestMethod]
        public async Task DefaultOptions()
        {
            DynamicIconOptions options = new DynamicIconOptions();
            options.DefaultSize = 128;
            options.FontName = "Ubuntu";

            DynamicIconMiddleware middleware = new DynamicIconMiddleware(next: (innerHttpContext) =>
            {
                throw new Exception("Test");
            }, options);

            HttpContext context = CreateContext("https://localhost/images/dynamicicon?t=DI");
            await middleware.InvokeAsync(context);

            Assert.AreEqual(200, context.Response.StatusCode);
            Assert.AreEqual("image/png", context.Response.ContentType);
            Assert.IsTrue(IsValidImage(context.Response.Body, options.DefaultSize));

            context.Response.Body.Dispose();
        }

        [TestMethod]
        public async Task DifferentRoute()
        {
            DynamicIconOptions options = new DynamicIconOptions();
            DynamicIconMiddleware middleware = new DynamicIconMiddleware(next: (innerHttpContext) =>
            {
                innerHttpContext.Response.StatusCode = 204;
                return Task.CompletedTask;
            }, options);

            HttpContext context = CreateContext("https://localhost/alternate/route");
            await middleware.InvokeAsync(context);

            Assert.AreEqual(204, context.Response.StatusCode);

            context.Response.Body.Dispose();
        }

        private Boolean IsValidImage(Stream body, Int32 expectedSize)
        {
            body.Seek(0, SeekOrigin.Begin);

            using (Image image = Bitmap.FromStream(body))
            {
                return image.Width == expectedSize && image.Height == expectedSize;
            }
        }

        private HttpContext CreateContext(String requestUri, String method = "GET", IEnumerable<KeyValuePair<String, String>> headers = null)
        {
            return CreateContext(new Uri(requestUri), method, headers);
        }

        private HttpContext CreateContext(Uri requestUri, String method = "GET", IEnumerable<KeyValuePair<String, String>> headers = null)
        {
            HttpContext context = new DefaultHttpContext();

            if(headers != null && headers.Any())
            {
                foreach(var headerGroup in headers.GroupBy(h => h.Key))
                {
                    context.Request.Headers.Add(headerGroup.Key, new StringValues(headerGroup.Select(h => h.Value).ToArray()));
                }
            }

            context.Request.Method = method;
            context.Request.Scheme = requestUri.Scheme;
            context.Request.Host = new HostString(requestUri.Host);
            context.Request.Path = new PathString(requestUri.AbsolutePath);
            context.Request.QueryString = new QueryString(requestUri.Query);

            context.Response.Body = new MemoryStream();

            return context;
        }
    }
}
