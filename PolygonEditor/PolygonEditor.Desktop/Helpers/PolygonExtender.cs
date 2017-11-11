using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolygonEditor.Desktop.Models;

namespace PolygonEditor.Desktop.Helpers
{
    public static class PolygonExtender
    {
        public static bool IsConvex(this Polygon p)
        {
            bool sign = false;
            var vertexes = p.GetVertexes().ToArray();
            for (int i = 0; i < p.GetVertexes().Count() - 2; i++)
            {
                int dx1 = vertexes[i + 1].X - vertexes[i].X;
                int dy1 = vertexes[i + 1].Y - vertexes[i].Y;
                int dx2 = vertexes[i + 2].X - vertexes[i + 1].X;
                int dy2 = vertexes[i + 2].Y - vertexes[i + 1].Y;
                int crossproduct = dx1 * dy2 - dy1 * dx2;
                if (i == 0)
                {
                    sign = crossproduct < 0;
                }
                else if(sign && crossproduct > 0 || !sign && crossproduct < 0)
                    return false;
            }

            return true;
        }

    }
}
