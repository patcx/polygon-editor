using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PolygonEditor.Desktop.Models
{
    public class Vertex
    {
        public static int Epsilon = 30;

        public int X { get; set; }
        public int Y { get; set; }

        public bool IsLocked { get; set; }

        public Vertex(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool IsNear(int x, int y)
        {
            return (Math.Abs(X - x) <= Epsilon && Math.Abs(Y - y) <= Epsilon);
        }

        public static double AngleBetween(Vertex v1, Vertex middle, Vertex v2)
        {
            var angle = Math.Atan2(middle.Y - v1.Y, middle.X - v1.X) - Math.Atan2(middle.Y - v2.Y, middle.X - v2.X);
            var angleInDegree =  angle* 180/Math.PI;
            if (angleInDegree < 0)
                angleInDegree = 360 + angleInDegree;

            return angleInDegree;
        }

        public static Vertex Rotate(Vertex v, int angle)
        {
            var ca = Math.Cos((double)angle/180*Math.PI);
            var sa = Math.Sin((double)angle/180*Math.PI);
            var result =  new Vertex((int)(ca * v.X - sa * v.Y), (int)(sa * v.X + ca * v.Y));
            return result;
        }
    }
}
