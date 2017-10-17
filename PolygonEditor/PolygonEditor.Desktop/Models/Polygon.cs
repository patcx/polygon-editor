using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolygonEditor.Desktop.Models.Constraints;

namespace PolygonEditor.Desktop.Models
{
    public class Polygon
    {
        private readonly List<Vertex> vertexes;
        private readonly List<IVertexConstraint> constraints;

        public bool IsClosed { get; private set; } = false;

        public Polygon()
        {
            vertexes = new List<Vertex>();
            constraints = new List<IVertexConstraint>();
        }

        public Vertex AddVertex(int x, int y)
        {
            var vertex = new Vertex(x, y);
            vertexes.Add(vertex);
            return vertex;
        }

        public Vertex AddVertex(int x, int y, Vertex v1, Vertex v2)
        {
            for (int i = 0; i < vertexes.Count; i++)
            {
                if (vertexes[i] == v1 && vertexes[(i + 1) % vertexes.Count] == v2)
                {
                    var vertex = new Vertex(x, y);
                    vertexes.Insert(i+1, vertex);
                    return vertex;
                }
            }

            return null;
        }

        public void AddConstraint(IVertexConstraint constraint)
        {
            if (constraint.IsContraintValid() || constraint.TryRepairConstraint(this))
            {
                constraints.Add(constraint);
            }
        }

        public void SetVertexPosition(Vertex p, int x, int y)
        {
            p.IsLocked = true;

            int backupX = p.X;
            int backupY = p.Y;

            p.X = x;
            p.Y = y;

            foreach (var constraint in constraints)
            {
                if (constraint.IsVertexInvolved(p) && !constraint.IsContraintValid())
                {
                    if (!constraint.TryRepairConstraint(this))
                    {
                        p.X = backupX;
                        p.Y = backupY;
                    }
                }
            }

            p.IsLocked = false;
        }

        public void MovePolygon(int x, int y)
        {
            foreach (var vertex in vertexes)
            {
                vertex.X += x;
                vertex.Y += y;
            }
        }

        public void RemoveVertex(int x, int y)
        {
            vertexes.RemoveAll(p => Math.Abs(p.X - x) <= Vertex.Epsilon && Math.Abs(p.Y - y) <= Vertex.Epsilon);
        }

        public Vertex GetVertex(int x, int y)
        {
            return (from p in vertexes where p.IsNear(x, y) select p).FirstOrDefault();
        }

        public (Vertex p1, Vertex p2) GetVertexesBetween(int x, int y)
        {
            for (int i = 0; i < vertexes.Count; i++)
            {
                if (IsBetweenVertexes(x, y, vertexes[i], vertexes[(i + 1) % vertexes.Count]))
                {
                    return (vertexes[i], vertexes[(i + 1) % vertexes.Count]);
                }
            }

            return (null, null);
        }

        public IEnumerable<Vertex> GetVertexes()
        {
            return vertexes;
        }

        public IEnumerable<(Vertex v1, Vertex v2)> GetEdges()
        {
            for (int i = 0; i < vertexes.Count; i++)
            {
                if(i+1 < vertexes.Count || IsClosed)
                yield return (vertexes[i], vertexes[(i + 1)%vertexes.Count]);
            }
        }

        public void SetClosed()
        {
            IsClosed = true;
        }

        private bool IsBetweenVertexes(int x, int y, Vertex p1, Vertex p2)
        {
            if (x <= Math.Min(p1.X, p2.X) || x >= Math.Max(p1.X, p2.X))
                return false;
            if (y <= Math.Min(p1.Y, p2.Y) || y >= Math.Max(p1.Y, p2.Y))
                return false;

            int vec = (x - p1.X) * (p2.Y - p1.Y) - (p2.X - p1.X) * (y - p1.Y);
            double dist = Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
            return (Math.Abs(vec) <= dist*10);
        }
    }
}
