using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PolygonEditor.Desktop.Models.Filler
{
    public class AnimatedLight
    {
        private int angleZ = 45;
        private int angleXY;

        private Timer timer;
        public Action<Vector3> LightChanged;

        public int Radius { get; set; }
        public Vector3 LightVector { get; set; }

        public AnimatedLight()
        {
            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            angleXY += 10;
            if (angleXY >= 360)
            {
                angleZ += 10;
                angleZ %= 90;
            }
            angleXY %= 360;
           
            double s = DegreeToRadians(angleXY);
            double t = DegreeToRadians(angleZ);

            double x = Radius * Math.Cos(s) * Math.Sin(t);
            double y = Radius * Math.Sin(s) * Math.Sin(t);
            double z = Radius * Math.Cos(t);
            LightVector = new Vector3((float)x, (float)y, (float)z);
            LightChanged?.Invoke(LightVector);
        }

        private double DegreeToRadians(int degree)
        {
            return degree * Math.PI / 180;
        }
    }
}
