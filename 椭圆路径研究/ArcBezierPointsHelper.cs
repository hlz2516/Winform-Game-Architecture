using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 椭圆路径研究
{
    public class ArcBezierPointsHelper
    {
        public static (PointF,PointF,PointF,PointF) GetArcBezierPoints(
            PointF center, float r, float startRad, float thetaRad)
        {
            // 通用k系数
            float k = 4f / 3f * (float)Math.Tan(thetaRad / 4d);
            float L = r * k;

            // 起点P0
            float a0 = startRad;
            float x0 = center.X + r * (float)Math.Cos(a0);
            float y0 = center.Y + r * (float)Math.Sin(a0);
            var p0 = new PointF(x0, y0);

            // 终点P3
            float a3 = startRad + thetaRad;
            float x3 = center.X + r * (float)Math.Cos(a3);
            float y3 = center.Y + r * (float)Math.Sin(a3);
            var p3 = new PointF(x3, y3);

            // P1：起点切线延伸
            float tx0 = -(float)Math.Sin(a0);
            float ty0 = (float)Math.Cos(a0);
            float x1 = x0 + L * tx0;
            float y1 = y0 + L * ty0;
            var p1 = new PointF(x1, y1);

            // P2：终点反向切线延伸
            float tx3 = (float)Math.Sin(a3);
            float ty3 = -(float)Math.Cos(a3);
            float x2 = x3 + L * tx3;
            float y2 = y3 + L * ty3;
            var p2 = new PointF(x2, y2);

            return (p0, p1, p2, p3);
        }
    }
}
