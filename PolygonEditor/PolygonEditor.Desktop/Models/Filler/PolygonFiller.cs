using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PolygonEditor.Desktop.Helpers;

namespace PolygonEditor.Desktop.Models.Filler
{
    public class PolygonFiller
    {
        private Polygon polygon;
        private List<EdgeSummary>[] edgesBucket;

        public PolygonFiller(Polygon polygon)
        {
            this.polygon = polygon;
            polygon.VertexAdded += (v) => { RefreshEdgesBucket(); };
            polygon.VertexMoved += (v) => { RefreshEdgesBucket(); };
            polygon.Moved += (x, y) => { RefreshEdgesBucket(); };

            RefreshEdgesBucket();
        }

        public void Fill(Bitmap bitmap)
        {
            BitmapData sourceData = bitmap.LockBits(new Rectangle(0, 0,
                                      bitmap.Width, bitmap.Height),
                                      ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            bitmap.UnlockBits(sourceData);
            BitmapWrapper bmpWrapper = new BitmapWrapper(pixelBuffer, bitmap.Height, bitmap.Width);

            
            List<EdgeSummary> ActiveEdges = new List<EdgeSummary>();
            int y = 0;
            for (y = 0; y < edgesBucket.Length; y++)
            {
                if (edgesBucket[y] != null && edgesBucket[y].Any())
                    break;
            }

            for (; y < edgesBucket.Length; y++)
            {
                if (edgesBucket[y] != null)
                {
                    foreach (var edgeSummary in edgesBucket[y])
                    {
                        ActiveEdges.Add(new EdgeSummary(edgeSummary));
                    }
                }

                ActiveEdges = ActiveEdges.OrderBy(x => x.X).ToList();
                ActiveEdges.RemoveAll(x => x.YMax <= y);

                for (int i = 1; i < ActiveEdges.Count; i++)
                {
                    if(i%2 == 0)
                        continue;

                    for (int x = (int) ActiveEdges[i - 1].X; x <= ActiveEdges[i].X; x++)
                    {
                        bmpWrapper.SetPixel(y, x, Color.Black);
                    }
                }


                foreach (var edgeSummary in ActiveEdges)
                {
                    edgeSummary.X += edgeSummary.Factor;
                }

                if (!ActiveEdges.Any())
                    break;
            }

            BitmapData resultData = bitmap.LockBits(new Rectangle(0, 0,
                                       bitmap.Width, bitmap.Height),
                                       ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            bitmap.UnlockBits(resultData);
        }

        private void RefreshEdgesBucket()
        {
            if (!polygon.GetEdges().Any())
            {
                edgesBucket = new List<EdgeSummary>[1];
                return;
            }

            int maxY = polygon.GetEdges().Max(v => Math.Max(v.v1.Y, v.v2.Y));
            edgesBucket = new List<EdgeSummary>[maxY+1];
            foreach (var edge in polygon.GetEdges())
            {
                EdgeSummary edgeSummary = new EdgeSummary(edge.Item1, edge.Item2);
                int y = Math.Min(edge.Item1.Y, edge.Item2.Y);
                if(edgesBucket[y] == null)
                    edgesBucket[y] = new List<EdgeSummary>();

                edgesBucket[y].Add(edgeSummary);
            }
        }
    }
}
