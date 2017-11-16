using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using PolygonEditor.Desktop.Helpers;
using Color = System.Windows.Media.Color;

namespace PolygonEditor.Desktop.Models.Filler
{
    public class PolygonFillingSettings
    {
        private Vector2 mousePosition;
        public event Action LightMoved;

        private AnimatedLight animatedLight = new AnimatedLight();

        public bool UseFixedObjectColor { get; set; } = true;
        public bool UseFixedLightSource { get; set; } = true;
        public bool UseEmptyDisturbanceVector { get; set; } = true;
        public bool UseFixedNormalVector { get; set; } = true;
        public bool UseMouseFollowNormalVector { get; set; }
        public float Factor { get; set; }

        public int Radius
        {
            get { return animatedLight.Radius; }
            set { animatedLight.Radius = value; }
        }
        public Color ObjectColor { get; set; } = Color.FromRgb(0, 0, 0);
        public Vector3 LightColor { get; set; } = Vector3.One;
        public BitmapWrapper ObjectTexture { get; set; }
        public BitmapWrapper NormalMap { get; set; }
        public BitmapWrapper HeightMap { get; set; }

        public PolygonFillingSettings()
        {
            animatedLight.LightChanged += LightChanged;
            Radius = 5;
        }

        private void LightChanged(Vector3 vector3)
        {
            if(!UseFixedLightSource)
                LightMoved?.Invoke();
        }

        public Color GetObjectColorOrTexture(int x, int y)
        {
            if (!UseFixedObjectColor && ObjectTexture != null)
                return ObjectTexture.GetPixel(y % ObjectTexture.Height, x % ObjectTexture.Width).ConvertToWindowsMediaColor();
            else
                return ObjectColor;
        }

        public Vector3 GetLightVector()
        {
            if (UseFixedLightSource)
                return new Vector3(0, 0, 1);
            else
                return animatedLight.LightVector;
        }

        public Vector3 GetNormalVector(int x, int y)
        {
            Vector3 normalVec;
            Vector3 disturbanceVector;

            if (!UseFixedNormalVector && !UseMouseFollowNormalVector && NormalMap != null)
            {
                var normalColor = NormalMap.GetPixel(y%NormalMap.Height, x%NormalMap.Width);
                normalVec = new Vector3(normalColor.R - 127, normalColor.G - 127, normalColor.B - 127);
                normalVec.X /= 127;
                normalVec.Y /= 127;
                normalVec.Z /= 127;
                normalVec = Vector3.Normalize(normalVec);
            }
            else if (UseMouseFollowNormalVector)
            {
                var dx = mousePosition.X - x;
                var dy = mousePosition.Y - y;

                var z = 100 * 100 - dx * dx - dy * dy;
                z = z < 0 ? 0 : (float)Math.Sqrt(z);

                normalVec = Vector3.Normalize(new Vector3(dx, dy, z));
            }
            else
                normalVec = new Vector3(0, 0, 1);

            if (!UseEmptyDisturbanceVector && HeightMap != null)
            {
                var hCol = HeightMap.GetPixel(y % HeightMap.Height, x % HeightMap.Width);
                var rightHCol = HeightMap.GetPixel(y % HeightMap.Height, (x + 1) % HeightMap.Width);
                var downHCol = HeightMap.GetPixel((y +1) % HeightMap.Height, x % HeightMap.Width);

                var dhx = (rightHCol.R - hCol.R + rightHCol.G - hCol.G + rightHCol.B - hCol.B) * Factor;
                var dhy = (downHCol.R - hCol.R + downHCol.G - hCol.G + downHCol.B - hCol.B) * Factor;

                disturbanceVector =  new Vector3(dhx, 0, -normalVec.X+dhx) + new Vector3(dhy, 0, -normalVec.Y*dhy);
            }
            else
                disturbanceVector = new Vector3(0, 0, 0);

            var normalPrim = normalVec + disturbanceVector;
            return Vector3.Normalize(normalPrim);
        }

        public void SetMouse(int x, int y)
        {
            mousePosition.X = x;
            mousePosition.Y = y;
        }
    }
}
