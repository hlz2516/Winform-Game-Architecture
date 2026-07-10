using CoreLib;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace 敌人
{
    public class TriangleEnemy : PolygonMoveObject
    {
        Timer rotateTimer = new Timer();
        int times = 0;

        public TriangleEnemy()
        {
            rotateTimer.Interval = 30;
            rotateTimer.Tick += Move_Logic;
        }

        private void Move_Logic(object sender, EventArgs e)
        {
            if (times < 30)
            {
                MoveOneStep();
            }
            else if (times >= 30 && times < 120)
            {
                Rotate();
                RotateAngle += 2;
            }
            else if(times >= 120 && times < 150)
            {
                MoveOneStep();
                if (times == 149)
                {
                    times = 0;
                    return;
                }
            }

            times++;
        }

        public void StartMove() => rotateTimer.Start();

        public override void SetRegion()
        {
            PointF[] points = new PointF[3];
            points[0] = new PointF(0, 0);
            points[1] = new PointF(this.Width * ScaleFactor, 0);
            points[2] = new PointF(this.Width * ScaleFactor / 2, this.Width * ScaleFactor / 2);
            SetRegionInner(points);
        }
    }
}
