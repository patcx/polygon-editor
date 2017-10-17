using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Desktop.Models
{
    public class EditorInputHandler : InputHandler
    {
        private int lastMouseMoveX;
        private int lastMouseMoveY;
        private bool isLeftMousePressed;

        private Vertex selectedvertex;


        public EditorInputHandler(Polygon polygon) : base(polygon)
        {
        }

        public override void MouseLeftDown(int mouseX, int mouseY)
        {
            lastMouseMoveX = mouseX;
            lastMouseMoveY = mouseY;
            isLeftMousePressed = true;

            selectedvertex = polygon.GetVertex(mouseX, mouseY);

            var edge = polygon.GetVertexesBetween(mouseX, mouseY);
            if (edge.Item1 != null && edge.Item2 != null && polygon.IsClosed && selectedvertex == null)
            {
                selectedvertex = polygon.AddVertex(mouseX, mouseY, edge.Item1, edge.Item2);
            }
        }

        public override void MouseLeftUp(int mouseX, int mouseY)
        {
            isLeftMousePressed = false;
            selectedvertex = null;
        }

        public override void MouseRightDown(int mouseX, int mouseY)
        {
            if(polygon.GetVertexes().Count() > 3)
                polygon.RemoveVertex(mouseX, mouseY);
        }

        public override void MouseMove(int mouseX, int mouseY)
        {
            if(selectedvertex != null)
                polygon.SetVertexPosition(selectedvertex, mouseX, mouseY);
            else if (isLeftMousePressed)
            {
                polygon.MovePolygon(mouseX-lastMouseMoveX, mouseY-lastMouseMoveY);
                lastMouseMoveX = mouseX;
                lastMouseMoveY = mouseY;
            }
        }
    }
}
