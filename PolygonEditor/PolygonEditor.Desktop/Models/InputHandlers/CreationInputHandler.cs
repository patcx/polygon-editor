using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Desktop.Models.InputHandlers
{
    public class CreationInputHandler : InputHandler
    {
        public CreationInputHandler(Polygon polygon) : base(polygon)
        {
        }

        public override bool MouseLeftDown(int mouseX, int mouseY)
        {
            var vertexOnPoint = polygon.GetVertex(mouseX, mouseY);
            var vertexesBetweenPoint = polygon.GetVertexesBetween(mouseX, mouseY);

            //check if is trying to close polygon
            if (vertexOnPoint != null && polygon.GetVertexes().FirstOrDefault() == vertexOnPoint)
            {
                polygon.SetClosed();
            }
            //check if polygon is not closed and if point is not on a edge
            else if (!polygon.IsClosed &&  (vertexesBetweenPoint.Item1 == null && vertexesBetweenPoint.Item2 == null))
            {
                polygon.AddVertex(mouseX, mouseY);
            }

            return vertexOnPoint != null || (vertexesBetweenPoint.p1 == null && vertexesBetweenPoint.p2 == null);
        }

        public override void MouseLeftUp(int mouseX, int mouseY)
        {
        }

        public override bool MouseRightDown(int mouseX, int mouseY)
        {
            return polygon.RemoveVertex(mouseX, mouseY);
        }

        public override bool MouseMove(int mouseX, int mouseY)
        {
            return false;
        }
    }
}
