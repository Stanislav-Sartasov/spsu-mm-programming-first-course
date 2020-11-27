using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathLibrary;

namespace UI.WF
{
    public partial class MainWindow : Form
    {
        private readonly Graphics canvas;

        public MainWindow()
        {
            InitializeComponent();
            canvas = DrawingCanvas.CreateGraphics();
            AbstractCurve[] curves = new AbstractCurve[]
            {
                new Parabola(1),
                new MathLibrary.Ellipse(1, 1),
                new Hyperbola(1, 1)
            };
            ChooseCurve.Items.AddRange(curves);
        }

        private void Build_Click(object sender, EventArgs e)
        {
            AbstractCurve curve = (AbstractCurve)ChooseCurve.SelectedItem;
            if (curve != null)
            {
                canvas.Clear(Color.White);
                PaintCoordinateSystem();
                PaintCurve(curve);
            }
        }

        private void BuildLine(double x1, double y1, double x2, double y2, double width)
        {
            canvas.DrawLine(new Pen(Color.Black, (float)width), (float)x1, (float)y1, (float)x2, (float)y2);
        }

        private void PaintCurve(AbstractCurve curve)
        {
            curve.A = float.TryParse(coeffA.Text, out float A) ? A : 1;
            curve.B = float.TryParse(coeffB.Text, out float B) ? B : 1;
            curve.P = float.TryParse(coeffP.Text, out float P) ? Math.Abs(P) : 1;

            List<PointF> points = curve.GetPoints(DrawingCanvas.Height, DrawingCanvas.Width + 150, scaleSlider.Value);
            for (int point = 1; point < points.Count; point++)
                if (point != points.Count / 2)
                    BuildLine(points[point - 1].X + DrawingCanvas.Width / 2, points[point - 1].Y + DrawingCanvas.Height / 2, points[point].X + DrawingCanvas.Width / 2, points[point].Y + DrawingCanvas.Height / 2, 1);
        }

        private void PaintCoordinateSystem()
        {
            float width = DrawingCanvas.Width;
            float height = DrawingCanvas.Height;
            float pixelsPerUnit = 2 * scaleSlider.Value;
            BuildLine(0, height / 2, width, height / 2, 0.2);
            BuildLine(width / 2, 0, width / 2, height, 0.2);

            for (double i = pixelsPerUnit; width / 2 + i < width - pixelsPerUnit; i += pixelsPerUnit)
            {
                if (scaleSlider.Value > 2 || (int)(i / pixelsPerUnit) % 2 == 0)
                {
                    canvas.DrawString(((int)(i / pixelsPerUnit)).ToString(), new Font("Arial", 7), new SolidBrush(Color.Black), new PointF(width / 2 + (float)i, height / 2));
                    canvas.DrawString(((int)(-i / pixelsPerUnit)).ToString(), new Font("Arial", 7), new SolidBrush(Color.Black), new PointF(width / 2 - (float)i, height / 2));
                }

                BuildLine(width / 2 + i, height / 2 - 2, width / 2 + i, height / 2 + 2, 0.2);
                BuildLine(width / 2 - i, height / 2 - 2, width / 2 - i, height / 2 + 2, 0.2);
            }
        }

        private void ScaleSlider_Scroll(object sender, EventArgs e)
        {
            this.textScaling.Text = $"Scaling: {scaleSlider.Value:0.0}";
        }
    }
}
