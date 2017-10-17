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

        public override void MouseLeftDown(int mouseX, int mouseY)
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
        }

        public override void MouseLeftUp(int mouseX, int mouseY)
        {
            
        }

        public override void MouseRightDown(int mouseX, int mouseY)
        {
            polygon.RemoveVertex(mouseX, mouseY);
        }

        public override void MouseMove(int mouseX, int mouseY)
        {
            
        }
    }
}
