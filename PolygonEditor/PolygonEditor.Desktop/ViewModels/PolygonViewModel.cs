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
                bitmap.DrawCircle(vertex, Color.Maroon);
            }

            BitmapCanvas = bitmap.ConvertToBitmapImage();
            bitmap.Dispose();
            RaisePropertyChanged("BitmapCanvas");
        }
    }
}
