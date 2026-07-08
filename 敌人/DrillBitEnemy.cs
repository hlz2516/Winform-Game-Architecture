using CoreLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 敌人
{
    public class DrillBitEnemy : StandardMoveObject
    {
        private Timer moveTimer;

        public DrillBitEnemy()
        {
            moveTimer = new Timer();
            moveTimer.Interval = 30;
            moveTimer.Tick += MoveTimer_Tick;
        }

        int times = 0;
        private void MoveTimer_Tick(object sender, EventArgs e)
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
            else if (times >= 120 && times < 150)
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

        public void StartMove() => moveTimer.Start();

        public override void SetRegion()
        {
            PointF[] points = new PointF[5];
            points[0] = new PointF(0, 0);
            points[1] = new PointF(this.Width * ScaleFactor, 0);
            points[2] = new PointF(this.Width * ScaleFactor, this.Height * ScaleFactor / 2);
            points[3] = new PointF(this.Width * ScaleFactor / 2, this.Height * ScaleFactor);
            points[4] = new PointF(0, this.Width * ScaleFactor/2);
            SetRegionInner(points);
        }
    }
}
