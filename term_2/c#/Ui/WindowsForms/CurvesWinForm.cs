using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathCruves;

namespace WindowsForms
{
    public partial class CurvesWinForm : Form
    {
        private Graphics graphics;
        private float size;
        private float height;
        private float width;
        /// <summary>

        private void NumberInCoord(float number, float x, float y)
        {
            number = (float)Math.Round(number, 1);
            var font = new Font(Font.FontFamily, 9);
            graphics.DrawString(number.ToString(), font, Brushes.Black, x, y);
        }
        private void SysCoord()
        {
            graphics.DrawLine(Pens.Black, width / 2, 0, width / 2, height);
            graphics.DrawLine(Pens.Black, 0, height / 2, width, height / 2);
            graphics.DrawLine(Pens.Black, width / 2, 0, (width / 2) - 5, 7);
            graphics.DrawLine(Pens.Black, width / 2, 0, (width / 2) + 5, 7);
            graphics.DrawLine(Pens.Black, width, height / 2, width - 7, (height / 2) - 5);
            graphics.DrawLine(Pens.Black, width, height / 2, width - 7, (height / 2) + 5);

            float step = (float)((width / 22));
            float x = width / 2;
            float y = height / 2;
            float number = 0;
            bool chet = true;
            for (float i = 1; i <= 10; ++i)
            {
                x = x + step;
                number += size;
                graphics.DrawLine(Pens.Black, x, y - 5, x, y + 5);
                if (number.ToString().Length > 2)
                {
                    if (chet)
                    {
                        NumberInCoord(number, x - 5, y + 7);
                        chet = false;
                    }
                    else
                        chet = true;
                }
                else
                    NumberInCoord(number, x - 5, y + 7);
            }
            x = width / 2;
            number = 0;
            chet = true;
            for (float i = 1; i <= 10; ++i)
            {
                x = x - step;
                number -= size;
                graphics.DrawLine(Pens.Black, x, y - 5, x, y + 5);
                if (number.ToString().Length > 2)
                {
                    if (chet)
                    {
                        NumberInCoord(number, x - 5, y + 7);
                        chet = false;
                    }
                    else
                        chet = true;
                }
                else
                    NumberInCoord(number, x - 5, y + 7);
            }
            step = (float)((height / 22));
            x = width / 2;
            number = 0;
            int delta = 0;
            chet = true;
            for (float i = 1; i <= 10; ++i)
            {
                y = y - step;
                number += size;
                if ((number.ToString().Length > 3) && (chet))
                {
                    delta += 10;
                    chet = false;
                }
                graphics.DrawLine(Pens.Black, x - 5, y, x + 5, y);
                NumberInCoord(number, x - 25 - delta, y - 5);
            }
            y = height / 2;
            delta = 0;
            chet = true;
            number = 0;
            for (float i = 1; i <= 10; ++i)
            {
                y = y + step;
                number -= size;
                if ((number.ToString().Length > 3) && (chet))
                {
                    delta += 10;
                    chet = false;
                }
                graphics.DrawLine(Pens.Black, x - 5, y, x + 5, y);
                NumberInCoord(number, x - 25 - delta, y);
            }
        }
        private void DrawCruve(Curve cruve)
        {
            bool error = false;
            float step = (float)((width / 22) / size);
            List<float> xLst = new List<float>();
            for (float i = -size * 11; i < size * 11; i += size / 4)
            {
                xLst.Add(i);
            }

            List<PointF> lst = cruve.Raschet(xLst, out error);
            lst[0]= new PointF(width / 2 + step * lst[0].X, height / 2 - step * lst[0].Y); 
            for (int i = 1; i  < lst.Count ; ++i)
            {
                lst[i] = (new PointF(width / 2 + step * lst[i].X, height / 2 - step * lst[i].Y));
                graphics.DrawLine(Pens.Blue, lst[i - 1], lst[i]);
            }
        }
        /// </summary>
        public CurvesWinForm()
        {
            InitializeComponent();
            size = 1;
            graphics = pictureBox.CreateGraphics();
            width = (float)pictureBox.Width;
            height = (float)pictureBox.Height;
            labelSize.Text = "Size: " + size.ToString();
            comboBoxCurves.Items.AddRange(new Curve[] {new Parabola(), new ClassicParabola(), new Circle()});
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            graphics.Clear(BackColor);
            SysCoord();
            Curve cruve = (Curve)comboBoxCurves.SelectedItem;
            if  (cruve != null)
            {
                DrawCruve(cruve);
            }
            
        }

        private void plusSize_Click(object sender, EventArgs e)
        {
            if (size + 0.1f < 10.1f)
                size = size + 0.1f;
            labelSize.Text = "Size: " + size.ToString();
        }

        private void minusSize_Click(object sender, EventArgs e)
        {
            if (size - 0.1f > 0.1f)
                size = size - 0.1f;
            labelSize.Text = "Size: " + size.ToString();
        }
    }
}
