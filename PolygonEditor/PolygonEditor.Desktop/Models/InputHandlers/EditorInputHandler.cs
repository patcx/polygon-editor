using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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

       

        public override bool MouseLeftDown(int mouseX, int mouseY)
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

            return selectedVertex != null || (selectedEdge.v1 != null && selectedEdge.v2 != null);
        }

        public override void MouseLeftUp(int mouseX, int mouseY)
        {
            isLeftMousePressed = false;
            selectedVertex = null;
            selectedEdge = (null, null);
        }

        public override bool MouseRightDown(int mouseX, int mouseY)
        {
            var vertex = polygon.GetVertex(mouseX, mouseY);
            var edge = polygon.GetVertexesBetween(mouseX, mouseY);
            if (vertex != null)
            {
                var result = VertexContextWindow.Show(mouseX, mouseY);
                if (result.Item1 == VertexContextWindow.VertexContextResult.DeleteVertex && polygon.GetVertexes().Count() > 3)
                    polygon.RemoveVertex(mouseX, mouseY);
                else if (result.Item1 == VertexContextWindow.VertexContextResult.DeleteConstraints)
                {
                    polygon.RemoveConstraint(vertex);
                }

                if (result.Item1 == VertexContextWindow.VertexContextResult.AddAngleConstraint 
                    && result.Item2 > 0)
                {
                    var neighbours = polygon.GetNeighbours(vertex);
                    polygon.AddConstraint(new AngleConstraint(result.Item2, neighbours.Item1, vertex, neighbours.Item2));
                }

            }
            else if (edge.Item1 != null && edge.Item2 != null)
            {
                var result = EdgeContextWindow.Show(mouseX, mouseY);
                if (result == EdgeContextWindow.EdgeContextResult.DeleteConstraints)
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

            return (edge.p1 != null && edge.p2 != null) || (vertex != null);

        }

        public override bool MouseMove(int mouseX, int mouseY)
        {
            if (selectedVertex != null)
            {
                polygon.SetVertexPosition(selectedVertex, mouseX, mouseY);
            }
            else if (isLeftMousePressed)
            {
                polygon.MovePolygon(mouseX - lastMouseMoveX, mouseY - lastMouseMoveY);
                lastMouseMoveX = mouseX;
                lastMouseMoveY = mouseY;
            }

            return (isLeftMousePressed || selectedVertex != null);
        }

        public override void ResetLeftMousePressed()
        {
            isLeftMousePressed = false;
        }

    }
}
