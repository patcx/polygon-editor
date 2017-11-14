using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Media = System.Windows.Media;

namespace PolygonEditor.Desktop.Helpers
{
    public static class WindowsMediaColor
    {

        public static Color ConvertToDrawingColor(this Media.Color col)
        {
            return Color.FromArgb(col.R, col.G, col.B);
        }
    }
}
