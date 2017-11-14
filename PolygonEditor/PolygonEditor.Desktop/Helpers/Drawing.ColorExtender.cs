using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Drawing = System.Drawing;

namespace PolygonEditor.Desktop.Helpers
{
    public static class DrawingColor
    {

        public static Color ConvertToWindowsMediaColor(this Drawing.Color col)
        {
            return Color.FromRgb(col.R, col.G, col.B);
        }
    }
}
