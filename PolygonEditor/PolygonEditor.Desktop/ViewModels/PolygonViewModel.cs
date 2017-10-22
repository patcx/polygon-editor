using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using PolygonEditor.Desktop.Helpers;
using PolygonEditor.Desktop.Models;
using PolygonEditor.Desktop.Models.Constraints;
using PolygonEditor.Desktop.Models.InputHandlers;

namespace PolygonEditor.Desktop.ViewModels
{
    public class PolygonViewModel : ObservableObject
    {
        private  Polygon polygon = new Polygon();
        private InputHandler inputHandler;
        public BitmapImage BitmapCanvas { get; set; }

        public PolygonViewModel()
        {
            inputHandler = new CreationInputHandler(polygon);
        }


        public ICommand DeleteKeyDown => new RelayCommand(() =>
        {
            polygon = new Polygon();
            inputHandler = new CreationInputHandler(polygon);
            RedrawPolygon();
        });

        public ICommand MouseLeftDown => new RelayCommand<MouseButtonEventArgs>(x =>
        {
            int mouseX = (int) x.GetPosition(Application.Current.MainWindow).X;
            int mouseY = (int) x.GetPosition(Application.Current.MainWindow).Y;

            inputHandler.MouseLeftDown(mouseX, mouseY);
            if(polygon.IsClosed && inputHandler is CreationInputHandler)
                inputHandler = new EditorInputHandler(polygon);
           
            RedrawPolygon();
        });

        public ICommand MouseLeftUp => new RelayCommand<MouseButtonEventArgs>(x =>
        {
            int mouseX = (int)x.GetPosition(Application.Current.MainWindow).X;
            int mouseY = (int)x.GetPosition(Application.Current.MainWindow).Y;

            inputHandler.MouseLeftUp(mouseX, mouseY);

            RedrawPolygon();
        });

        public ICommand MouseRightDown => new RelayCommand<MouseButtonEventArgs>(x =>
        {
            int mouseX = (int)x.GetPosition(Application.Current.MainWindow).X;
            int mouseY = (int)x.GetPosition(Application.Current.MainWindow).Y;

            inputHandler.MouseRightDown(mouseX, mouseY);

            RedrawPolygon();
        });

        public ICommand MouseMove => new RelayCommand<MouseEventArgs>(x =>
        {
            int mouseX = (int)x.GetPosition(Application.Current.MainWindow).X;
            int mouseY = (int)x.GetPosition(Application.Current.MainWindow).Y;

            inputHandler.MouseMove(mouseX, mouseY);

            RedrawPolygon();
        });

        public ICommand Resize => new RelayCommand(() =>
        {
            RedrawPolygon();
        });

        

        private void RedrawPolygon()
        {
            var bitmap = new Bitmap((int)Application.Current.MainWindow.ActualWidth, (int)Application.Current.MainWindow.ActualHeight);
            bitmap.Fill(Color.White);

            foreach(var edge in polygon.GetEdges())
            {
                bitmap.DrawLine(edge.Item1, edge.Item2, Color.Black);
            }

            foreach (var vertex in polygon.GetVertexes())
            {
                var neighbours = polygon.GetNeighbours(vertex);
                bitmap.DrawCircle(vertex, 3, Color.Maroon);

                var angle = Vertex.AngleBetween(neighbours.Item1, vertex, neighbours.Item2);
                bitmap.DrawText(vertex, angle.ToString(), Color.ForestGreen);
            }

            foreach (var vertexConstraint in polygon.GetConstraints())
            {
                var vertexes = vertexConstraint.GetVertexes().ToArray();

                if (vertexConstraint is VerticalEdgeConstraint)
                {
                    int x = (vertexes[0].X + vertexes[1].X) / 2 + 10;
                    int y = (vertexes[0].Y + vertexes[1].Y) / 2;
                    x += 10;
                    bitmap.DrawLine(new Vertex(x, y - 5), new Vertex(x, y + 5), Color.ForestGreen);
                    bitmap.DrawLine(new Vertex(x + 1, y - 5), new Vertex(x + 1, y + 5), Color.ForestGreen);
                }
                else if (vertexConstraint is HorizontalEdgeConstraint)
                {
                    int x = (vertexes[0].X + vertexes[1].X) / 2;
                    int y = (vertexes[0].Y + vertexes[1].Y) / 2 + 10;
                    y += 10;
                    bitmap.DrawLine(new Vertex(x - 5, y), new Vertex(x + 5, y), Color.ForestGreen);
                    bitmap.DrawLine(new Vertex(x - 5, y + 1), new Vertex(x + 5, y + 1), Color.ForestGreen);
                }
                else if (vertexConstraint is AngleConstraint)
                {
                    var v = vertexConstraint.GetVertexes().ToArray();
                    Vertex middle = v[1];

                    bitmap.DrawCircle(middle, 5, Color.ForestGreen);
                }



            }

            BitmapCanvas = bitmap.ConvertToBitmapImage();
            bitmap.Dispose();
            RaisePropertyChanged("BitmapCanvas");
        }
    }
}
