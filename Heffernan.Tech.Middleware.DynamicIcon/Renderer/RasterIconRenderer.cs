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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Heffernan.Tech.Middleware.DynamicIcon.Renderer
{
    internal partial class RasterIconRenderer : IIconRenderer
    {
        public Int32 Size { get; set; }
        public IconFormat IconFormat { get; set; }
        public String FontName { get; set; }
        public String Background { get; set; }
        public String Foreground { get; set; }
        public String Text { get; set; }

        public Task<Byte[]> Render()
        {
            return Task.Run(async () =>
            {
                using (Bitmap image = new Bitmap(Size, Size))
                {
                    using (Graphics gfx = Graphics.FromImage(image))
                    {
                        gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        gfx.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                        await DrawBackground(gfx);
                        DrawText(gfx);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            image.Save(ms, GetFormat(IconFormat));
                            return ms.ToArray();
                        }
                    }
                }
            });
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

        private void DrawText(Graphics gfx)
        {
            if (String.IsNullOrWhiteSpace(Text) == false)
            {
                FontFamily font = new FontFamily(FontName);
                GraphicsPath path = new GraphicsPath();
                Brush brush = GetColorBrush(Foreground);
                StringFormat format = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                
                try
                {
                    path.AddString(Text, font, (Int32)FontStyle.Regular, Size / 2, new Rectangle(0, 0, Size, Size), format);
                    gfx.FillPath(brush, path);
                }
                finally
                {
                    format.Dispose();
                    brush.Dispose();
                    path.Dispose();
                    font.Dispose();
                }
            }
        }

        private async Task DrawBackground(Graphics gfx)
        {
            using (Brush brush = await GetBrush(Background))
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(1, 1, Size - 3, Size - 3);
                    gfx.FillPath(brush, path);
                }
            }
        }

        private async Task<Brush> GetBrush(String background)
        {
            Brush brush = GetColorBrush(background);
            if (brush != null)
                return brush;

            brush = await GetImageBrush(background);
            if (brush != null)
                return brush;

            throw new InvalidOperationException($"{nameof(background)} is not a valid background");
        }

        private Task<Brush> GetImageBrush(String background)
        {
            throw new NotImplementedException();
        }

        private Brush GetColorBrush(String background)
        {
            try
            {
                Color color = ColorTranslator.FromHtml(background);
                return new SolidBrush(color);
            }
            catch (Exception)
            {
            }

            return null;
        }
    }
}
