using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolygonEditor.Desktop.Helpers;

namespace PolygonEditor.Desktop.Models.Intersections
{
    public class PolygonIntersection
    {
        public static Polygon GetIntersectedPolygon(Polygon subjectPolygon, Polygon clipPolygon)
        {
            var subjectPolygonTmp = subjectPolygon;

            if (!clipPolygon.IsConvex())
            {
                subjectPolygon = clipPolygon;
                clipPolygon = subjectPolygonTmp;
            }
            if (!clipPolygon.IsConvex())
            {
                return clipPolygon;
            }

            List<Vertex> output = new List<Vertex>(subjectPolygon.GetVertexes());
            foreach(var edge in clipPolygon.GetEdges())
            {
                List<Vertex> input = output;
                output = new List<Vertex>();
                if (!input.Any())
                    return subjectPolygonTmp;
                Vertex pp = input[input.Count - 1];
                foreach (var p in input)
                {
                    if (IsInnerPoint(p, clipPolygon.GetVertexes().ToArray(), edge))
                    {
                        if (!IsInnerPoint(pp, clipPolygon.GetVertexes().ToArray(), edge))
                        {
                            output.Add(GetIntersectionPoint((p, pp), edge));
                        }

                        output.Add(p);
                    }
                    else if (IsInnerPoint(pp, clipPolygon.GetVertexes().ToArray(), edge))
                    {
                        output.Add(GetIntersectionPoint((p, pp), edge));
                    }

                    pp = p;
                }
            }

            for (int i = 0; i < output.Count - 1; i++)
            {
                if (output[i] == output[i + 1])
                    output.RemoveAt(i--);

            }

            Polygon intersectedPolygon = new Polygon();
            foreach (var vertex in output)
            {
                intersectedPolygon.AddVertex(vertex.X, vertex.Y);
            }

            intersectedPolygon.SetClosed();
            return intersectedPolygon.GetVertexes().Any() ? intersectedPolygon : subjectPolygonTmp;
        }

        private static bool IsInnerPoint(Vertex p, Vertex[] array, (Vertex, Vertex) seg)
        {
            Vertex[] arr2 = new Vertex[3];
            arr2[0] = array[0] + new Vertex((array[1].X - array[0].X) / 2, (array[1].Y - array[0].Y) / 2);
            arr2[1] = array[1] + new Vertex((array[2].X - array[1].X) / 2, (array[2].Y - array[1].Y) / 2);
            arr2[2] = array[2] + new Vertex((array[2].X - array[0].X) / 2, (array[2].Y - array[0].Y) / 2);


            arr2[0] = arr2[0] + new Vertex((arr2[1].X - arr2[0].X) / 2, (arr2[1].Y - arr2[0].Y) / 2);
            arr2[1] = arr2[1] + new Vertex((arr2[2].X - arr2[1].X) / 2, (arr2[2].Y - arr2[1].Y) / 2);
            arr2[2] = arr2[2] + new Vertex((arr2[2].X - arr2[0].X) / 2, (arr2[2].Y - arr2[0].Y) / 2);

            return IsSameSide(p, arr2[0], seg);
        }

        public static bool IsSameSide(Vertex p1, Vertex p2, (Vertex, Vertex) s)
        {
           Vertex end = s.Item2 - s.Item1;
           Vertex pp1 = p1 - s.Item1;
           Vertex pp2 = p2 - s.Item1;

            
            double s1 = Vertex.CrossProduct(end, pp1);
            double s2 = Vertex.CrossProduct(end, pp2);

            if (Math.Abs(s1) < Double.Epsilon || Math.Abs(s2) < Double.Epsilon)
                return true;
            bool dir1 = s1 > 0;
            bool dir2 = s2 > 0;

            return dir1 == dir2;
        }

        public static Vertex GetIntersectionPoint((Vertex, Vertex) seg1, (Vertex, Vertex) seg2)
        {
            Vertex direction1 = new Vertex(seg1.Item2.X - seg1.Item1.X, seg1.Item2.Y - seg1.Item1.Y);
            Vertex direction2 = new Vertex(seg2.Item2.X - seg2.Item1.X, seg2.Item2.Y - seg2.Item1.Y);
            double dotPerp = (direction1.X * direction2.Y) - (direction1.Y * direction2.X);

            Vertex c = new Vertex(seg2.Item1.X - seg1.Item1.X, seg2.Item1.Y - seg1.Item1.Y);
            double t = (c.X * direction2.Y - c.Y * direction2.X) / dotPerp;

            return new Vertex((int)(seg1.Item1.X + (t * direction1.X)), (int)(seg1.Item1.Y + (t * direction1.Y)));
        }
    }
}
