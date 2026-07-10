using System;
using System.Drawing;

namespace CoreLib
{
    public class CurveHelper
    {
        public static BezierPoints GetArcBezierPoints(
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

            return new BezierPoints(p0, p1, p2, p3);
        }

        /// <summary>
        /// 计算一段椭圆圆弧的三次贝塞尔四点 P0,P1,P2,P3
        /// </summary>
        /// <param name="center">椭圆中心点</param>
        /// <param name="rx">X轴半轴宽度</param>
        /// <param name="ry">Y轴半轴高度</param>
        /// <param name="startRad">起始角度(弧度)</param>
        /// <param name="thetaRad">圆弧圆心角(弧度，正数逆时针，负数顺时针)</param>
        /// <param name="p0">圆弧起点</param>
        /// <param name="p1">贝塞尔控制点1</param>
        /// <param name="p2">贝塞尔控制点2</param>
        /// <param name="p3">圆弧终点</param>
        public static BezierPoints GetEllipseArcBezierPoints(
            PointF center, float rx, float ry,
            float startRad, float thetaRad)
        {
            double theta = thetaRad;
            double k = 4.0 / 3.0 * Math.Tan(theta / 4.0);

            double a0 = startRad;
            double a3 = startRad + theta;

            // 1. 起点 P0
            float x0 = center.X + rx * (float)Math.Cos(a0);
            float y0 = center.Y + ry * (float)Math.Sin(a0);
            var p0 = new PointF(x0, y0);

            // 2. 终点 P3
            float x3 = center.X + rx * (float)Math.Cos(a3);
            float y3 = center.Y + ry * (float)Math.Sin(a3);
            var p3 = new PointF(x3, y3);

            // 3. P1：起点切线延伸控制点
            float dx0 = -rx * (float)k * (float)Math.Sin(a0);
            float dy0 = ry * (float)k * (float)Math.Cos(a0);
            var p1 = new PointF(x0 + dx0, y0 + dy0);

            // 4. P2：终点反向切线延伸控制点
            float dx3 = rx * (float)k * (float)Math.Sin(a3);
            float dy3 = -ry * (float)k * (float)Math.Cos(a3);
            var p2 = new PointF(x3 + dx3, y3 + dy3);

            return new BezierPoints(p0, p1, p2, p3);
        }

        /// <summary>
        /// Catmull-Rom / Cardinal样条转三次贝塞尔四点
        /// </summary>
        /// <param name="vm1">V[i-1]</param>
        /// <param name="v0">V[i] 曲线起点</param>
        /// <param name="v1">V[i+1] 曲线终点</param>
        /// <param name="v2">V[i+2]</param>
        /// <param name="s">张力，Catmull-Rom标准0.5</param>
        /// <param name="p0">贝塞尔起点</param>
        /// <param name="p1">贝塞尔控制点1</param>
        /// <param name="p2">贝塞尔控制点2</param>
        /// <param name="p3">贝塞尔终点</param>
        public static void CardinalToBezier(
            PointF vm1, PointF v0, PointF v1, PointF v2, float s,
            out PointF p0, out PointF p1, out PointF p2, out PointF p3)
        {
            p0 = v0;
            p3 = v1;

            float k = s / 3f;
            // P1 = v0 + k*(v1 - vm1)
            float p1x = v0.X + k * (v1.X - vm1.X);
            float p1y = v0.Y + k * (v1.Y - vm1.Y);
            p1 = new PointF(p1x, p1y);

            // P2 = v1 - k*(v2 - v0)
            float p2x = v1.X - k * (v2.X - v0.X);
            float p2y = v1.Y - k * (v2.Y - v0.Y);
            p2 = new PointF(p2x, p2y);
        }

        public static void BezierToCardinal(
            PointF p0, PointF p1, PointF p2, PointF p3, float s,
            out PointF vm1, out PointF v0, out PointF v1, out PointF v2)
        {
            v0 = p0;
            v1 = p3;
            float k = 3f / s;

            // V[i-1] = v1 - k*(p1 - v0)
            float vm1x = v1.X - k * (p1.X - v0.X);
            float vm1y = v1.Y - k * (p1.Y - v0.Y);
            vm1 = new PointF(vm1x, vm1y);

            // V[i+2] = v0 + k*(v1 - p2)
            float v2x = v0.X + k * (v1.X - p2.X);
            float v2y = v0.Y + k * (v1.Y - p2.Y);
            v2 = new PointF(v2x, v2y);
        }
    }

    /// <summary>
    /// 三段贝塞尔曲线结构
    /// </summary>
    public struct BezierPoints
    {
        public PointF P0 { get; set; }
        public PointF P1 { get; set; }
        public PointF P2 { get; set; }
        public PointF P3 { get; set; }

        public BezierPoints(PointF p0, PointF p1, PointF p2, PointF p3)
        {
            P0 = p0;
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }
    }
}
