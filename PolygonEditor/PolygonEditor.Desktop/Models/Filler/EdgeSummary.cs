using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Desktop.Models.Filler
{
    public class EdgeSummary
    {
        public int YMax { get; set; }
        public double X { get; set; }
        public double Factor { get; set; }

        public EdgeSummary(Vertex v1, Vertex v2)
        {
            YMax = Math.Max(v1.Y, v2.Y);
            X = v1.Y < v2.Y ? v1.X : v2.X;

            Factor = (double)(v1.X - v2.X)/ (v1.Y - v2.Y);
        }

        public EdgeSummary(EdgeSummary edgeSummary)
        {
            YMax = edgeSummary.YMax;
            X = edgeSummary.X;
            Factor = edgeSummary.Factor;
        }
    }
}
