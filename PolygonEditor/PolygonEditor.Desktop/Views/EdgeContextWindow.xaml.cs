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
    /// Interaction logic for VertexContextWindow.xaml
    /// </summary>
    public partial class EdgeContextWindow : Window
    {
        public enum EdgeContextResult
        {
            Cancel,
            AddVerticalConstraint,
            AddHorizontalConstraint,
            DeleteConstraints
        }

        private EdgeContextResult result;

        private EdgeContextWindow()
        {
            InitializeComponent();
        }

        public static EdgeContextResult Show(int x, int y)
        {
            var window = new EdgeContextWindow();
            window.Top = y;
            window.Left = x;
            window.ShowDialog();
            return window.result;
        }

        private void DeleteConstraints(object sender, RoutedEventArgs e)
        {
            result = EdgeContextResult.DeleteConstraints;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            result = EdgeContextResult.Cancel;
            Close();
        }

        private void AddHorizontalConstraint(object sender, RoutedEventArgs e)
        {
            result = EdgeContextResult.AddHorizontalConstraint;
            Close();
        }

        private void AddVerticalConstraint(object sender, RoutedEventArgs e)
        {
            result = EdgeContextResult.AddVerticalConstraint;
            Close();
        }
    }
}
