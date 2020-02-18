using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Heffernan.Tech.Middleware.DynamicIcon.Test
{
    [TestClass]
    public class DynamicIconOptionsTest
    {
        [TestMethod]
        public void DefaultOptions()
        {
            DynamicIconOptions options = new DynamicIconOptions();

            Assert.AreEqual(DynamicIconMiddleware.DEFAULT_BACKGROUND, options.DefaultBackground);
            Assert.AreEqual(DynamicIconMiddleware.DEFAULT_FOREGROUND, options.DefaultForeground);
            Assert.AreEqual(DynamicIconMiddleware.DEFAULT_FORMAT, options.Format);
            Assert.AreEqual(DynamicIconMiddleware.DEFAULT_FONT, options.FontName);
            Assert.AreEqual(DynamicIconMiddleware.MINIMUM_SIZE, options.MinimumSize);
            Assert.AreEqual(DynamicIconMiddleware.DEFAULT_SIZE, options.DefaultSize);
            Assert.AreEqual(DynamicIconMiddleware.MAXIMUM_SIZE, options.MaximumSize);
            Assert.AreEqual(DynamicIconMiddleware.DEFAULT_ROUTE, options.Route);
        }

        [TestMethod]
        public void ValidOptions()
        {
            DynamicIconOptions options = new DynamicIconOptions()
            {
                Route = "/route",
                MinimumSize = 128,
                DefaultSize = 256,
                MaximumSize = 512,
                DefaultBackground = "C0FFEE",
                DefaultForeground = "DEADB0",
                Format = IconFormat.Ico,
                FontName = "Arial"
            };

            Assert.AreEqual("C0FFEE", options.DefaultBackground);
            Assert.AreEqual("DEADB0", options.DefaultForeground);
            Assert.AreEqual(IconFormat.Ico, options.Format);
            Assert.AreEqual("Arial", options.FontName);
            Assert.AreEqual(128, options.MinimumSize);
            Assert.AreEqual(256, options.DefaultSize);
            Assert.AreEqual(512, options.MaximumSize);
            Assert.AreEqual("/route", options.Route);
        }

        [TestMethod]
        public void InvalidFormat()
        {
            DynamicIconOptions options = new DynamicIconOptions();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                options.Format = 0;
            });
        }


        [TestMethod]
        public void InvalidMinimumSize()
        {
            DynamicIconOptions options = new DynamicIconOptions();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                options.MinimumSize = DynamicIconMiddleware.MINIMUM_SIZE - 1;
            });
        }

        [TestMethod]
        public void InvalidMaximumSize()
        {
            DynamicIconOptions options = new DynamicIconOptions();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                options.MaximumSize = DynamicIconMiddleware.MAXIMUM_SIZE + 1;
            });
        }

        [TestMethod]
        public void InvalidRoute()
        {
            DynamicIconOptions options = new DynamicIconOptions();

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                options.Route = null;
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                options.Route = String.Empty;
            });
        }

        [TestMethod]
        public void InvalidFontName()
        {
            DynamicIconOptions options = new DynamicIconOptions();

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                options.FontName = null;
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                options.FontName = String.Empty;
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                options.FontName = "InvalidFontName";
            });
        }
    }
}
