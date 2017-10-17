using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Desktop.Models
{
    public abstract class InputHandler
    {
        protected Polygon polygon;

        protected InputHandler(Polygon polygon)
        {
            this.polygon = polygon;
        }

        public abstract void MouseMove(int mouseX, int mouseY);
        public abstract void MouseLeftDown(int mouseX, int mouseY);
        public abstract void MouseLeftUp(int mouseX, int mouseY);
        public abstract void MouseRightDown(int mouseX, int mouseY);
    }
}
