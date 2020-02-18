﻿/*
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

using Microsoft.AspNetCore.Builder;
using System;

namespace Heffernan.Tech.Middleware.DynamicIcon
{
    public static class Extensions
    {
        public static IApplicationBuilder UseDynamicIcon(this IApplicationBuilder app, Func<DynamicIconOptionsBuilder, DynamicIconOptionsBuilder> buildOptions)
        {
            DynamicIconOptionsBuilder builder = new DynamicIconOptionsBuilder();
            DynamicIconOptions options = buildOptions(builder).Build();

            return app.UseMiddleware<DynamicIconMiddleware>(options);
        }

        public static IApplicationBuilder UseDynamicIcon(this IApplicationBuilder app, DynamicIconOptions options)
        {
            return app.UseMiddleware<DynamicIconMiddleware>(options);
        }
    }
}
