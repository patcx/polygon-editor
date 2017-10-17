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


        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
