using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public class TrigonometricFunctions
    {
        public static PointF GetPointByAngle(float startX, float startY, float distance, float angleClockwise)
        {
            double rad = -(angleClockwise - 90) * Math.PI / 180.0;
            float cosVal = (float)Math.Cos(rad);
            float sinVal = (float)Math.Sin(rad);

            float x = startX - distance * cosVal;
            float y = startY + distance * sinVal;

            return new PointF(x, y);
        }
    }
}
