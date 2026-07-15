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
        /// <summary>
        /// 根据角度和距离计算目标点的坐标位置
        /// 在这个二维直角坐标系中，Y轴负方向为0度，顺时针旋转。与matrix的参考系保持一致。
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="distance"></param>
        /// <param name="angleClockwise"></param>
        /// <returns></returns>
        public static PointF GetPointByAngle(float startX, float startY, float distance, float angleClockwise)
        {
            double rad = -(angleClockwise - 90) * Math.PI / 180.0;
            float cosVal = (float)Math.Cos(rad);
            float sinVal = (float)Math.Sin(rad);

            float x = startX - distance * cosVal;
            float y = startY + distance * sinVal;

            return new PointF(x, y);
        }

        /// <summary>
        /// 角度 转 弧度
        /// 公式：rad = degree * Math.PI / 180
        /// </summary>
        /// <param name="degree">角度值，如90、180</param>
        /// <returns>对应弧度</returns>
        public static double DegreeToRad(double degree)
        {
            return degree * Math.PI / 180.0;
        }

        // 反向：弧度转角度
        public static double RadToDegree(double rad)
        {
            return rad * 180.0 / Math.PI;
        }
    }
}
