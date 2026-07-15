using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 贪吃蛇
{
    public partial class Form1 : Form
    {
        Snake snake;
        public Form1()
        {
            InitializeComponent();
            snake = new Snake();
            snake.Left = 200;
            snake.Top = 200;
            snake.SetRegion();
            this.Controls.Add(snake);
            snake.StartMove();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //// Create a path and add two ellipses.
            //GraphicsPath myPath = new GraphicsPath();
            //myPath.AddEllipse(0, 0, 100, 100);
            //myPath.AddEllipse(100, 0, 100, 100);

            //// Draw the original ellipses to the screen in black.
            //e.Graphics.DrawPath(Pens.Black, myPath);

            //// Widen the path.
            //Pen widenPen = new Pen(Color.Black, 10);
            //Matrix widenMatrix = new Matrix();
            //widenMatrix.Translate(50, 50);
            //myPath.Widen(widenPen, widenMatrix, 1.0f);

            //// Draw the widened path to the screen in red.
            //e.Graphics.FillPath(new SolidBrush(Color.Red), myPath);
        }
    }
}
