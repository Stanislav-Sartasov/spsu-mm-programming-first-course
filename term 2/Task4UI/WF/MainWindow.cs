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

namespace WF
{
    public partial class MainWindow : Form
    {
        private Graphics graphics;
        private double pixelsPerUnit;
        private int width, height;
        private bool buildButtonWasClicked = false;
        private double scale = 10;
        public MainWindow()
        {
            InitializeComponent();
            string[] curves = new string[] { "Ellipse", "Parabola", "Hyperbola" };
            collectionOfCurves.Items.AddRange(curves);
            graphics = graphArea.CreateGraphics();
        }

        private void buildButton_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            BuildCoordinateSystem();
            string curveName = (string)collectionOfCurves.SelectedItem;
            AbstractCurve curveToDraw;
            
            if (curveName == "Ellipse")
            {
                buildButtonWasClicked = true;
                curveToDraw = new Ellipse(2, 1);
            }
            else if (curveName == "Parabola")
            {
                buildButtonWasClicked = true;
                curveToDraw = new Parabola(2);
            }
            else if (curveName == "Hyperbola")
            {
                buildButtonWasClicked = true;
                curveToDraw = new Hyperbola(1, 1);
            }    
            else
            {
                buildButtonWasClicked = false;
                MessageBox.Show("Choose curve");
                return;
            }
            BuildGraph(curveToDraw);
        }
        private void BuildGraph(AbstractCurve curveToDraw)
        {
            Pen pen = new Pen(Color.Black);
            List<PointF> pointsToDraw = new List<PointF>();
            foreach (double[] point in curveToDraw.GetPointsToDraw(pixelsPerUnit, width, height))
            {
                pointsToDraw.Add(new PointF((float)point[0], (float)point[1]));
            }
            graphics.DrawPolygon(pen, pointsToDraw.ToArray());

            if (curveToDraw.Name == "Hyperbola")
            {
                List<PointF> symmetryPoints = new List<PointF>();
                foreach (PointF point in pointsToDraw)
                {
                    symmetryPoints.Add(new PointF(width - point.X, point.Y));
                }
                graphics.DrawPolygon(pen, symmetryPoints.ToArray());
            }
        }
        private void BuildCoordinateSystem()
        {
            Pen pen = new Pen(Color.Black);
            Font drawFont = new Font("Arial", 7);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            graphics = graphArea.CreateGraphics();
            width = graphArea.Width;
            height = graphArea.Height;
            pixelsPerUnit = width / 15 * scale / 10;

            graphics.DrawLine(pen, 0, height / 2,
                width, height / 2);
            graphics.DrawLine(pen, width / 2, 0,
                width / 2, height);
            graphics.DrawString("0", drawFont, drawBrush, new PointF(width / 2, height / 2));

            float xCentre = width / 2;
            for (double i = pixelsPerUnit; xCentre < width - pixelsPerUnit; i += pixelsPerUnit)
            {
                xCentre += (float)pixelsPerUnit;
                graphics.DrawLine(pen, new PointF(xCentre, height / 2 + 2), new PointF(xCentre, height / 2 - 2));
                graphics.DrawString(((int)(i / pixelsPerUnit)).ToString(), drawFont, drawBrush, new PointF(xCentre, height / 2));
                graphics.DrawLine(pen, new PointF(width - xCentre, height / 2 + 2), new PointF(width - xCentre, height / 2 - 2));
                graphics.DrawString(((int)(-i / pixelsPerUnit)).ToString(), drawFont, drawBrush, new PointF(width - xCentre, height / 2));
            }

            float yCentre = height / 2;
            for (double i = pixelsPerUnit; yCentre < height - pixelsPerUnit; i += pixelsPerUnit)
            {
                yCentre += (float)pixelsPerUnit;
                graphics.DrawLine(pen, new PointF(width / 2 + 2, yCentre), new PointF(width / 2 - 2, yCentre));
                graphics.DrawString(((int)(-i / pixelsPerUnit)).ToString(), drawFont, drawBrush, new PointF(width / 2, yCentre));
                graphics.DrawLine(pen, new PointF(width / 2 + 2, height - yCentre), new PointF(width / 2 - 2, height - yCentre));
                graphics.DrawString(((int)(i / pixelsPerUnit)).ToString(), drawFont, drawBrush, new PointF(width / 2, height - yCentre));
            }


        }

        private void plusScaleButton_Click(object sender, EventArgs e)
        {
            
            if (buildButtonWasClicked)
            {
                scale += 1;
                boxScale.Text = (scale / 10).ToString();
                BuildCoordinateSystem();
                buildButton_Click(sender, e);
            }

        }

        private void minusScaleButton_Click(object sender, EventArgs e)
        {
            if (buildButtonWasClicked)
            {
                if (scale == 5)
                {
                    MessageBox.Show("Scale is minimal");
                }
                else
                    scale -= 1;
                boxScale.Text = (scale / 10).ToString();
                BuildCoordinateSystem();
                buildButton_Click(sender, e);
            }
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            if (buildButtonWasClicked)
            {
                BuildCoordinateSystem();
                buildButton_Click(sender, e);
            }
        }
    }
}
