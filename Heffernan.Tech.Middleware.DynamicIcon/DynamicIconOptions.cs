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
    public class DynamicIconOptions
    {
        private String route = DynamicIconMiddleware.DEFAULT_ROUTE;
        private Int32 minimumSize = DynamicIconMiddleware.MINIMUM_SIZE;
        private Int32 maximumSize = DynamicIconMiddleware.MAXIMUM_SIZE;
        private IconFormat imageFormat = DynamicIconMiddleware.DEFAULT_FORMAT;

        public Int32 DefaultSize { get; set; } = DynamicIconMiddleware.DEFAULT_SIZE;
        public String FontName { get; set; } = DynamicIconMiddleware.DEFAULT_FONT;
        public String DefaultBackground { get; set; } = DynamicIconMiddleware.DEFAULT_BACKGROUND;
        public String DefaultForeground { get; set; } = DynamicIconMiddleware.DEFAULT_FOREGROUND;
        public TimeSpan? CacheFor { get; set; } = new TimeSpan(DynamicIconMiddleware.DEFAULT_CACHE);

        public String Route
        {
            get => route;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentOutOfRangeException(nameof(value));

                route = value;
            }
        }

        public Int32 MinimumSize
        {
            get => minimumSize;
            set
            {
                if (value < DynamicIconMiddleware.MINIMUM_SIZE)
                    throw new ArgumentOutOfRangeException(nameof(value));

                minimumSize = value;
            }
        }

        public Int32 MaximumSize
        {
            get => maximumSize;
            set
            {
                if (value > DynamicIconMiddleware.MAXIMUM_SIZE)
                    throw new ArgumentOutOfRangeException(nameof(value));

                maximumSize = value;
            }
        }

        public IconFormat Format
        {
            get => imageFormat;
            set
            {
                if (Enum.IsDefined(typeof(IconFormat), value) == false)
                    throw new ArgumentOutOfRangeException(nameof(value));

                imageFormat = value;
            }
        }
    }
}