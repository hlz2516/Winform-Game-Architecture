using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CoreLib.Interfaces
{
    public interface IRegion
    {
        GraphicsPath OriginVertexes { get; }
        float ScaleInfactor { get; set; }
        /// <summary>
        /// 形状的逻辑定义
        /// 默认正面朝下
        /// </summary>
        void SetRegion();
    }

    public interface IRotate : IRegion
    {
        float RotateAngle { get; set; }
        GraphicsPath RotatedVertexes { get; }
        /// <summary>
        /// 给出旋转中心点的计算逻辑，在改变朝向时会调用
        /// </summary>
        PointF CalculateRotateCenter(PointF[] points);
        /// <summary>
        /// 计算最大外接圆直径，用于后续设置旋转后的工作区大小
        /// </summary>
        int CalculMaxCircumcircleDiameter(PointF[] points, PointF rotateCenter);
        /// <summary>
        /// 在OnPaint事件中调用
        /// </summary>
        /// <param name="e"></param>
        /// <param name="rotatedRegion"></param>
        void PaintRotatedRegion(PaintEventArgs e,GraphicsPath rotatedRegion);
    }

    public interface IMove : IRegion
    {
        float MoveSpeed { get; set; }
        float MoveDirection { get; set; }
        PointF CurrPosF { get; }
        void MoveOneStep();
    }
}
