using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Heffernan.Tech.Middleware.DynamicIcon.Test
{
    [TestClass]
    public class DynamicIconOptionsBuilderTest
    {
        [TestMethod]
        public void SuccessfulBuild()
        {
            try
            {
                DynamicIconOptionsBuilder builder = new DynamicIconOptionsBuilder();
                DynamicIconOptions options = builder.UseDefaultBackground("C0FFEE")
                                                    .UseDefaultForeground("DEADB0")
                                                    .UseDefaultFormat(IconFormat.Ico)
                                                    .UseFontName("Arial")
                                                    .UseMinimumSize(128)
                                                    .UseDefaultSize(256)
                                                    .UseMaximumSize(512)
                                                    .UseRoute("/route")
                                                    .Build();

                Assert.AreEqual("C0FFEE", options.DefaultBackground);
                Assert.AreEqual("DEADB0", options.DefaultForeground);
                Assert.AreEqual(IconFormat.Ico, options.Format);
                Assert.AreEqual("Arial", options.FontName);
                Assert.AreEqual(128, options.MinimumSize);
                Assert.AreEqual(256, options.DefaultSize);
                Assert.AreEqual(512, options.MaximumSize);
                Assert.AreEqual("/route", options.Route);
            }
            catch
            {
                throw;
            }
        }

        [TestMethod]
        public void InvalidBackground()
        {
            DynamicIconOptionsBuilder builder = new DynamicIconOptionsBuilder();

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                builder.UseDefaultBackground(null);
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                builder.UseDefaultBackground(String.Empty);
            });
        }

        [TestMethod]
        public void InvalidForeground()
        {
            DynamicIconOptionsBuilder builder = new DynamicIconOptionsBuilder();

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                builder.UseDefaultForeground(null);
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                builder.UseDefaultForeground(String.Empty);
            });
        }

        [TestMethod]
        public void InvalidFormat()
        {
            DynamicIconOptionsBuilder builder = new DynamicIconOptionsBuilder();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                builder.UseDefaultFormat(0);
            });
        }

        [TestMethod]
        public void InvalidFont()
        {
            DynamicIconOptionsBuilder builder = new DynamicIconOptionsBuilder();

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                builder.UseFontName(null);
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                builder.UseFontName(String.Empty);
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                builder.UseFontName("InvalidFontName");
            });
        }

        [TestMethod]
        public void InvalidMinimumSize()
        {
            DynamicIconOptionsBuilder builder = new DynamicIconOptionsBuilder();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                builder.UseMinimumSize(DynamicIconMiddleware.MINIMUM_SIZE - 1);
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                builder.UseMinimumSize(DynamicIconMiddleware.MAXIMUM_SIZE + 1);
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                builder.UseMinimumSize(512)
                       .UseMaximumSize(128)
                       .UseDefaultSize(256)
                       .Build();
            });
        }

        [TestMethod]
        public void InvalidMaximumSize()
        {
            DynamicIconOptionsBuilder builder = new DynamicIconOptionsBuilder();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                builder.UseMaximumSize(DynamicIconMiddleware.MINIMUM_SIZE - 1);
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                builder.UseMaximumSize(DynamicIconMiddleware.MAXIMUM_SIZE + 1);
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                builder.UseMinimumSize(512)
                       .UseMaximumSize(128)
                       .UseDefaultSize(256)
                       .Build();
            });
        }

        [TestMethod]
        public void InvalidDefaultSize()
        {
            DynamicIconOptionsBuilder builder = new DynamicIconOptionsBuilder();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                builder.UseDefaultSize(DynamicIconMiddleware.MINIMUM_SIZE - 1);
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                builder.UseDefaultSize(DynamicIconMiddleware.MAXIMUM_SIZE + 1);
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                builder.UseMinimumSize(128)
                       .UseMaximumSize(512)
                       .UseDefaultSize(64)
                       .Build();
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                builder.UseMinimumSize(128)
                       .UseMaximumSize(512)
                       .UseDefaultSize(1024)
                       .Build();
            });
        }

        [TestMethod]
        public void InvalidRoute()
        {
            DynamicIconOptionsBuilder builder = new DynamicIconOptionsBuilder();

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                builder.UseRoute(null);
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                builder.UseRoute(String.Empty);
            });
        }
    }
}
