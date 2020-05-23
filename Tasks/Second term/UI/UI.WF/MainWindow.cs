using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using UI.MathCurve;

namespace UI.WF
{
    public partial class MainWindow : System.Windows.Forms.Form
    {
        private Graphics image;
        public MainWindow()
        {
            InitializeComponent();
            image = drawingArea.CreateGraphics();

            string[] curves = new string[]
            {
                "Parabola",
                "Hyperbola",
                "Ellipse"
            };
            chooseCurve.Items.AddRange(curves);
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            image.Clear(Color.White);
        }

        private void Build_Click(object sender, EventArgs e)
        {
            string shapeName = (string)chooseCurve.SelectedItem;
            Curve shape;
            switch (shapeName)
            {
                case "Parabola":
                    {
                        shape = new MyParabola();
                        break;
                    }
                case "Hyperbola":
                    {
                        shape = new MyHyperbola();
                        break;
                    }
                default:
                    {
                        shape = new MyEllipse();
                        break;
                    }

            }
            if (shape != null)
            {
                image.Clear(Color.White);
                image = drawingArea.CreateGraphics();
                BuildFunction(shape);
                BuildCoordinateSystem();
            }
        }

        private void BuildFunction(Curve shape)
        {
            double offsetX = this.Size.Width / 2 - 80;
            double offsetY = this.Size.Height / 2;
            double scale;
            bool correctScale = double.TryParse(windowScale.Text, out scale);
            if (scale < 50 || scale > 200 || !correctScale)
                scale = 100;

            float tempA;
            float tempB;
            float tempP;
            bool correctA = float.TryParse(a.Text, out tempA);
            bool correctB = float.TryParse(b.Text, out tempB);
            bool correctC = float.TryParse(p.Text, out tempP);
            bool flagCalculate = false;
            if (shape.Name.Equals("Hyperbola") || shape.Name.Equals("Ellipse"))
                if (correctA && correctB && tempA > 0 && tempB > 0)
                {
                    shape.A = tempA;
                    shape.B = tempB;
                    flagCalculate = true;
                }
            if (shape.Name.Equals("Parabola"))
                if (correctC && tempP > 0)
                {
                    shape.P = tempP;
                    flagCalculate = true;
                }
            if (flagCalculate)
            {
                //double step = Math.Min(offsetX, offsetY) / 300;
                double step = scale / 100;
                var pointGroupList = shape.FindValues(step, offsetX, offsetY, scale);
                foreach (List<double[]> points in pointGroupList)
                    image.DrawCurve(new Pen(Color.Black), ListOfDoubleToArrayOfPointF(points));
            }
        }

        private void BuildLine(double x1, double y1, double x2, double y2)
        {
            image.DrawLine(new Pen(Color.Black), (float)x1, (float)y1, (float)x2, (float)y2);
        }
        private void BuildCoordinateSystem()
        {
            double offsetX = this.Size.Width / 2 - 80;
            double offsetY = this.Size.Height / 2;

            double leftBorder = offsetX / 5;
            double rightBorder = offsetX * 9 / 5;
            double upperBorder = offsetY / 5;
            double bottomBorder = offsetY * 9 / 5;

            BuildLine(leftBorder, offsetY, rightBorder, offsetY);
            BuildLine(offsetX, upperBorder, offsetX, bottomBorder);

            BuildLine(rightBorder, offsetY, rightBorder - 5, offsetY - 5);
            BuildLine(rightBorder, offsetY, rightBorder - 5, offsetY + 5);

            BuildLine(offsetX, upperBorder, offsetX - 5, upperBorder + 5);
            BuildLine(offsetX, upperBorder, offsetX + 5, upperBorder + 5);

            double scale;
            bool correctScale = double.TryParse(windowScale.Text, out scale);
            if (scale < 50 || scale > 200 || !correctScale)
                scale = 100;
            int step = (int)Math.Round(scale / 5);
            int sign = 0;
            for (int x = (int)Math.Round(leftBorder + 5); x < rightBorder - 5; x += step)
            {
                BuildLine(x, offsetY - 5, x, offsetY + 5);
                string text = ((int)Math.Round((x - offsetX) / (scale / 100))).ToString();
                image.DrawString(text, new Font(Font.FontFamily, 7), Brushes.Black, x - 10, (int)(offsetY + 10 * Math.Pow(-2, sign % 2)));
                sign++;
            }
            sign = 0;
            for (int y = (int)Math.Round(bottomBorder - 5); y > upperBorder + 5; y -= step)
            {
                BuildLine(offsetX - 5, y, offsetX + 5, y);
                string text = ((int)Math.Round((offsetY - y) / (scale / 100))).ToString();
                image.DrawString(text, new Font(Font.FontFamily, 7), Brushes.Black, (int)(offsetX + 15 * Math.Pow(-2, sign % 2)), y - 7);
                sign++;
            }
        }

        private PointF[] ListOfDoubleToArrayOfPointF(List<double[]> a)
        {
            PointF[] result = new PointF[a.Count];
            for (int i = 0; i < a.Count; i++)
                result[i] = new PointF((float)a[i][0], (float)a[i][1]);
            return result;
        }
    }
}
