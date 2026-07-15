using CoreLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CoreLib.Entity
{
    public class MainCharacter : EllipseMoveObject, IRecvKeyInput
    {
        Timer moveTimer = new Timer();

        public MainCharacter()
        {
            MoveSpeed = 0; //初始不移动，后续通过定时器控制移动和旋转
            moveTimer.Interval = 30;
            moveTimer.Tick += Move_Logic;
        }

        private void Move_Logic(object sender, EventArgs e)
        {
            //根据当前按下的方向键状态来更新RotateAngle
            //如果同时按下了上下或左右键，则不改变RotateAngle
            bool hasLeft = (currMoveDirection & MoveDirectionInput.Left) == MoveDirectionInput.Left;
            bool hasRight = (currMoveDirection & MoveDirectionInput.Right) == MoveDirectionInput.Right;
            bool hasUp = (currMoveDirection & MoveDirectionInput.Up) == MoveDirectionInput.Up;
            bool hasDown = (currMoveDirection & MoveDirectionInput.Down) == MoveDirectionInput.Down;
            //如果没有按下任何方向键或者同时按下了所有方向键，或者同时按下了上下或左右键，则不移动
            if (hasLeft ^ hasRight ^ hasUp ^ hasDown == false)
            {
                if (hasLeft ^ hasRight == false && hasUp ^ hasDown == false)
                {
                    MoveSpeed = 0;
                    CheckRegionCollision();
                    return;
                }
                else
                {
                    MoveSpeed = DefaultMoveSpeed;
                    //两个组合方向键的情况
                    if (hasLeft && hasUp)
                    {
                        RotateAngle = 90 + 45;  //Y负轴为0，顺时针旋转
                    }
                    else if (hasLeft && hasDown)
                    {
                        RotateAngle = 45;
                    }
                    else if (hasRight && hasUp)
                    {
                        RotateAngle = 180 + 45;
                    }
                    else if (hasRight && hasDown)
                    {
                        RotateAngle = 360 - 45;
                    }
                    Rotate();
                    CheckRegionCollision();
                    MoveOneStep();
                    MapManager.UpdateObjectPosition(this);
                    return;
                }
            }
            //单个/三个方向键的情况
            if (hasLeft && (hasLeft ^ hasRight))
            {
                RotateAngle = 90;
            }
            else if (hasRight && (hasLeft ^ hasRight))
            {
                RotateAngle = 270;
            }
            else if (hasUp && (hasUp ^ hasDown))
            {
                RotateAngle = 180;
            }
            else if (hasDown && (hasUp ^ hasDown))
            {
                RotateAngle = 0;
            }
            MoveSpeed = DefaultMoveSpeed;
            Rotate();
            CheckRegionCollision();
            MoveOneStep();
            MapManager.UpdateObjectPosition(this);
        }

        public event Action<List<Point>> NotifyContainerRefreshDetectedRegion;

        private void CheckRegionCollision()
        {
            List<Point> regions = new List<Point>();
            bool outterbrk = false;
            foreach (var item in MapManager.GetSurroundObjects(this))
            {
                if (outterbrk)
                {
                    break;
                }
                var realPos = MapManager.CalculRealPosition(item.Key);
                regions.Add(realPos);
                foreach (var ctrl in item.Value)
                {
                    if (ctrl != this)
                    {
                        if (ctrl is Wall wall)
                        {
                            //如果根据当前朝向继续移动下一步是碰撞状态，则不移动，否则设置回默认速度
                            var nextOffset = TrigonometricFunctions.GetPointByAngle(0, 0, MoveSpeed, RotateAngle);
                            var nextPosF = PointF.Add(CurrPosF, nextOffset.ToSizeF());
                            var realNextPos = new Point((int)Math.Round(nextPosF.X), (int)Math.Round(nextPosF.Y));
                            bool collisioned = CollisionHelper.CheckRegionCollision(wall, this.Region, realNextPos);
                            //Debug.WriteLine($"下一步预测是否碰撞：{collisioned}");
                            MoveEnabled = collisioned ? false : true;
                        }
                    }
                    //if (ctrl != this && CollisionHelper.CheckRegionCollision(this, ctrl))
                    //{
                    //    //检测到碰撞，停止移动
                    //    Debug.WriteLine($"碰到了{ctrl.Name}");
                    //}
                }
            }

            NotifyContainerRefreshDetectedRegion?.Invoke(regions);
        }

        public void StartMove() => moveTimer.Start();

        /// <summary>
        /// 实现时必须调用SetRegionInner输入贝塞尔曲线列表，以实现Region设置，旋转和移动
        /// </summary>
        public override void SetRegion()
        {
            BezierPoints[] ellipse = new BezierPoints[4];
            PointF center = new PointF(Width * ScaleFactor / 2, Height * ScaleFactor / 2);
            float xRadius = Width * ScaleFactor / 2;
            float yRadius = Height * ScaleFactor / 2;
            float startAngle = 0; // 起始角度
            float sweepAngle = (float)(Math.PI / 2); // 扫过角度
            for (int i = 0; i < 4; i++)
            {
                ellipse[i] = CurveHelper.GetEllipseArcBezierPoints(center, xRadius, yRadius, startAngle, sweepAngle);
                startAngle += sweepAngle;
            }
            SetRegionInner(ellipse);
        }

        protected override void PaintRegionInner(PaintEventArgs e, GraphicsPath rotatedRegion)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            // 如果没有设置Region，直接退出
            if (this.Region == null)
                return;
            g.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);
            using (Pen borderPen = new Pen(Color.Red, 2)) // 边框画笔：红色、宽度2
            {
                // 绘制Region外边框
                g.DrawBeziers(borderPen, rotatedRegion.PathPoints);
                //根据旋转角度绘制当前朝向上的眼睛，用于指示当前朝向
                var offset = TrigonometricFunctions.GetPointByAngle(0, 0, Width / 4, RotateAngle);
                var eyePos = PointF.Add(rotateCenter, offset.ToSizeF());
                g.DrawEllipse(borderPen, eyePos.X - 2, eyePos.Y - 2, 4, 4);
                //绘制旋转中心
                //g.DrawEllipse(borderPen, RotateCenter.X - 2, RotateCenter.Y - 2, 4, 4);
            }
        }

        volatile MoveDirectionInput currMoveDirection = 0;
        public void GetCurrentKeyInput(KeyWrapper key)
        {
            //根据输入的键来更新currMoveDirection
            if (key.KeyCode == Keys.Up)
            {
                bool nextUp = key.State == KeyState.KeyUp ? false : true;
                if (nextUp)
                {
                    currMoveDirection |= MoveDirectionInput.Up;
                }
                else
                {
                    currMoveDirection &= ~MoveDirectionInput.Up;
                }
            }
            else if (key.KeyCode == Keys.Down)
            {
                bool nextDown = key.State == KeyState.KeyUp ? false : true;
                if (nextDown)
                {
                    currMoveDirection |= MoveDirectionInput.Down;
                }
                else
                {
                    currMoveDirection &= ~MoveDirectionInput.Down;
                }
            }
            else if (key.KeyCode == Keys.Left)
            {
                bool nextLeft = key.State == KeyState.KeyUp ? false : true;
                if (nextLeft)
                {
                    currMoveDirection |= MoveDirectionInput.Left;
                }
                else
                {
                    currMoveDirection &= ~MoveDirectionInput.Left;
                }
            }
            else if (key.KeyCode == Keys.Right)
            {
                bool nextRight = key.State == KeyState.KeyUp ? false : true;
                if (nextRight)
                {
                    currMoveDirection |= MoveDirectionInput.Right;
                }
                else
                {
                    currMoveDirection &= ~MoveDirectionInput.Right;
                }
            }
        }
    }
}
