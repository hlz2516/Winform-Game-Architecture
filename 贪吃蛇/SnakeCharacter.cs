using CoreLib;
using CoreLib.Entity;
using CoreLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 贪吃蛇
{
    public class SnakeCharacter : Snake, IRecvKeyInput
    {
        private Timer moveTimer = new Timer();
        volatile MoveDirectionInput currMoveDirection = 0;
        public SnakeCharacter()
        {
            moveTimer.Interval = 100;
            moveTimer.Tick += Move_Logic;
        }

        public void StartMove()
        {
            moveTimer.Start();
        }

        private void Move_Logic(object sender, EventArgs e)
        {
            //根据当前按下的方向键状态来更新MoveDirection
            //如果同时按下了上下或左右键，则不改变MoveDirection
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
                    //CheckRegionCollision();
                    return;
                }
                else
                {
                    MoveSpeed = DefaultMoveSpeed;
                    //两个组合方向键的情况
                    if (hasLeft && hasUp)
                    {
                        MoveDirection = 90 + 45;  //Y负轴为0，顺时针旋转
                    }
                    else if (hasLeft && hasDown)
                    {
                        MoveDirection = 45;
                    }
                    else if (hasRight && hasUp)
                    {
                        MoveDirection = 180 + 45;
                    }
                    else if (hasRight && hasDown)
                    {
                        MoveDirection = 360 - 45;
                    }
                    //Rotate();
                    //CheckRegionCollision();
                    MoveOneStep();
                    return;
                }
            }
            //单个/三个方向键的情况
            if (hasLeft && (hasLeft ^ hasRight))
            {
                MoveDirection = 90;
            }
            else if (hasRight && (hasLeft ^ hasRight))
            {
                MoveDirection = 270;
            }
            else if (hasUp && (hasUp ^ hasDown))
            {
                MoveDirection = 180;
            }
            else if (hasDown && (hasUp ^ hasDown))
            {
                MoveDirection = 0;
            }
            MoveSpeed = DefaultMoveSpeed;
           //CheckRegionCollision();
            MoveOneStep();
            //MapManager.UpdateObjectPosition(this);
        }

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
