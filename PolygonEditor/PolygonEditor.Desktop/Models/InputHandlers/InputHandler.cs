using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Desktop.Models.InputHandlers
{
    public abstract class InputHandler
    {
        protected Polygon polygon;

        protected InputHandler(Polygon polygon)
        {
            this.polygon = polygon;
        }

        public abstract bool MouseMove(int mouseX, int mouseY);
        public abstract bool MouseLeftDown(int mouseX, int mouseY);
        public abstract void MouseLeftUp(int mouseX, int mouseY);
        public abstract bool MouseRightDown(int mouseX, int mouseY);

        public virtual void ResetLeftMousePressed() { }
    }
}
