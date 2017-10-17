using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
            using (Graphics gfx = Graphics.FromImage(bitmap))
            using (Pen pen = new Pen(color))
            {
                gfx.DrawLine(pen, v1.X, v1.Y, v2.X, v2.Y);
            }
        }

        public static void DrawCircle(this Bitmap bitmap, Vertex v, Color color)
        {
            using (Graphics gfx = Graphics.FromImage(bitmap))
            using (SolidBrush brush = new SolidBrush(color))
            {
                gfx.FillEllipse(brush, v.X-3, v.Y-3, 6, 6);
            }
        }
    }
}
