using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Desktop.Models.Constraints
{
    public class AngleConstraint : IVertexConstraint
    {
        private Vertex side1;
        private Vertex middle;
        private Vertex side2;
        private int angle;

        public AngleConstraint(int angle, Vertex v1, Vertex middle, Vertex v2)
        {
            side1 = v1;
            this.middle = middle;
            side2 = v2;
            this.angle = angle;
        }

        public bool IsVertexInvolved(Vertex vertex)
        {
            if (side1 == vertex)
                return true;
            if (middle == vertex)
                return true;
            if (side2 == vertex)
                return true;

            return false;
        }

        public bool IsContraintValid()
        {
            var angle = Vertex.AngleBetween(side1, middle, side2);
            return Math.Abs(this.angle - (int) angle) <= 1;
        }

        public bool IsCollisionWithConstraints(IEnumerable<IVertexConstraint> otherConstraints)
        {
            foreach (var vertexConstraint in otherConstraints)
            {
                if (vertexConstraint.IsVertexInvolved(side1) && vertexConstraint.IsVertexInvolved(middle))
                    return true;
                if (vertexConstraint.IsVertexInvolved(side2) && vertexConstraint.IsVertexInvolved(middle))
                    return true;
            }

            return false;
        }

        public bool TryRepairConstraint(Polygon polygon)
        {
            if (IsContraintValid())
                return true;

            if (side1.IsLocked && side2.IsLocked)
                return false;

            var angle = Vertex.AngleBetween(side1, middle, side2);
            if (!side2.IsLocked)
            {
                var v = new Vertex(-middle.X + side2.X, -middle.Y + side2.Y);
                var newPosition = Vertex.Rotate(v, (int)(angle - this.angle));
                polygon.SetVertexPosition(side2, middle.X + newPosition.X, middle.Y + newPosition.Y);
            }
            else if (!side1.IsLocked)
            {
                var v = new Vertex(-middle.X + side1.X, -middle.Y + side1.Y);
                var newPosition = Vertex.Rotate(v, (int)(this.angle - angle));
                polygon.SetVertexPosition(side1, middle.X + newPosition.X, middle.Y + newPosition.Y);
            }
            return IsContraintValid();
        }

        public IEnumerable<Vertex> GetVertexes()
        {
            yield return side1;
            yield return middle;
            yield return side2;
        }
    }
}
