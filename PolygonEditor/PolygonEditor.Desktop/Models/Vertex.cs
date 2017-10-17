using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
