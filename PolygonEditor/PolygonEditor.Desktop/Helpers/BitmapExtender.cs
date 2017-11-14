using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using PolygonEditor.Desktop.Models;

namespace PolygonEditor.Desktop.Helpers
{
    public static class BitmapExtender
    {
        public static BitmapImage ConvertToBitmapImage(this Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public static BitmapWrapper ConvertToBitmapWrapper(this Bitmap bitmap)
        {
            BitmapData sourceData = bitmap.LockBits(new Rectangle(0, 0,
                                     bitmap.Width, bitmap.Height),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            bitmap.UnlockBits(sourceData);
            BitmapWrapper bmpWrapper = new BitmapWrapper(pixelBuffer, bitmap.Height, bitmap.Width);
            return bmpWrapper;
        }

        public static void Fill(this Bitmap bitmap, Color color)
        {
            using (Graphics gfx = Graphics.FromImage(bitmap))
            using (SolidBrush brush = new SolidBrush(color))
            {
                gfx.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);
            }
        }

        public static void DrawLine(this Bitmap bitmap, Vertex v1, Vertex v2, Color color)
        {
            int x = v1.X;
            int y = v1.Y;
            int x2 = v2.X;
            int y2 = v2.Y;

            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                if(x >= 0 && x < bitmap.Width && y>=0 && y<bitmap.Height)
                bitmap.SetPixel(x, y, color);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }

        public static void DrawCircle(this Bitmap bitmap, Vertex v, int radius, Color color)
        {
            using (Graphics gfx = Graphics.FromImage(bitmap))
            using (SolidBrush brush = new SolidBrush(color))
            {
                gfx.FillEllipse(brush, v.X-radius, v.Y-radius, radius*2, radius*2);
            }
        }

        public static void DrawText(this Bitmap bitmap, Vertex v, string text, Color color)
        {
            using (Graphics gfx = Graphics.FromImage(bitmap))
            using (SolidBrush brush = new SolidBrush(color))
            {
                gfx.DrawString(text, new Font(FontFamily.GenericMonospace, 12), brush, v.X, v.Y);
            }
        }
    }
}
