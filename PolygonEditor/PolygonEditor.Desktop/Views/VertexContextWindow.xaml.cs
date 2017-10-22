using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PolygonEditor.Desktop.Views
{
    /// <summary>
    /// Interaction logic for ContextWindow.xaml
    /// </summary>
    public partial class VertexContextWindow: Window
    {
        public VertexContextWindow()
        {
            InitializeComponent();
        }


        public enum VertexContextResult
        {
            Cancel,
            AddAngleConstraint,
            DeleteVertex,
            DeleteConstraints
        }

        private VertexContextResult result;
        private int angleValue = -1;


        public static (VertexContextResult, int) Show(int x, int y)
        {
            var window = new VertexContextWindow();
            window.Top = Application.Current.MainWindow.Top + y;
            window.Left = Application.Current.MainWindow.Left + x;
            window.ShowDialog();
            return (window.result, window.angleValue);
        }

        private void DeleteConstraints(object sender, RoutedEventArgs e)
        {
            result = VertexContextResult.DeleteConstraints;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            result = VertexContextResult.Cancel;
            Close();
        }

        private void DeleteVertex(object sender, RoutedEventArgs e)
        {
            result = VertexContextResult.DeleteVertex;
            Close();
        }

        private void AddAngleConstraint(object sender, RoutedEventArgs e)
        {
            result = VertexContextResult.AddAngleConstraint;
            try
            {
                angleValue = Int32.Parse(angle.Text);
            }
            catch
            {
                angleValue = -1;
            }
            Close();
        }
    }
}
