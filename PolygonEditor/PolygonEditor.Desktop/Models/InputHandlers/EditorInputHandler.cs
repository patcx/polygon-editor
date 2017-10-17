using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolygonEditor.Desktop.Models.Constraints;
using PolygonEditor.Desktop.Views;

namespace PolygonEditor.Desktop.Models.InputHandlers
{
    public class EditorInputHandler : InputHandler
    {
        private int lastMouseMoveX;
        private int lastMouseMoveY;
        private bool isLeftMousePressed;

        private Vertex selectedVertex;
        private (Vertex v1, Vertex v2) selectedEdge;


        public EditorInputHandler(Polygon polygon) : base(polygon)
        {
        }

        public override void MouseLeftDown(int mouseX, int mouseY)
        {
            lastMouseMoveX = mouseX;
            lastMouseMoveY = mouseY;
            isLeftMousePressed = true;

            selectedVertex = polygon.GetVertex(mouseX, mouseY);
            selectedEdge = polygon.GetVertexesBetween(mouseX, mouseY);


            if (selectedEdge.Item1 != null && selectedEdge.Item2 != null && polygon.IsClosed && selectedVertex == null)
            {
                selectedVertex = polygon.AddVertex(mouseX, mouseY, selectedEdge.Item1, selectedEdge.Item2);
            }
        }

        public override void MouseLeftUp(int mouseX, int mouseY)
        {
            isLeftMousePressed = false;
            selectedVertex = null;
            selectedEdge = (null, null);
        }

        public override void MouseRightDown(int mouseX, int mouseY)
        {
            var verticle = polygon.GetVertex(mouseX, mouseY);
            var edge = polygon.GetVertexesBetween(mouseX, mouseY);
            if (edge.Item1 != null && edge.Item2 != null)
            {
                var result = EdgeContextWindow.Show(mouseX, mouseY);
                if (result != EdgeContextWindow.EdgeContextResult.Cancel)
                {
                    polygon.RemoveConstraint(edge.Item1, edge.Item2);
                }

                if (result == EdgeContextWindow.EdgeContextResult.AddVerticalConstraint)
                {
                    polygon.AddConstraint(new VerticalEdgeConstraint(edge.Item1, edge.Item2));
                }
                else if (result == EdgeContextWindow.EdgeContextResult.AddHorizontalConstraint)
                {
                    polygon.AddConstraint(new HorizontalEdgeConstraint(edge.Item1, edge.Item2));
                }
            }

            if(polygon.GetVertexes().Count() > 3)
                polygon.RemoveVertex(mouseX, mouseY);
        }

        public override void MouseMove(int mouseX, int mouseY)
        {
            if(selectedVertex != null)
                polygon.SetVertexPosition(selectedVertex, mouseX, mouseY);
            else if (isLeftMousePressed)
            {
                polygon.MovePolygon(mouseX-lastMouseMoveX, mouseY-lastMouseMoveY);
                lastMouseMoveX = mouseX;
                lastMouseMoveY = mouseY;
            }
        }
    }
}
