using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MathCruves;
using System.Drawing;
using System.ComponentModel;

namespace Wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Logic Logic
        {
            get;
            set;
        }
        public MainWindow()
        {
            InitializeComponent();
            Logic = new Logic();
            Logic.width = (float)pictureBox.Width;
            Logic.height = (float)pictureBox.Height;
            this.DataContext = Logic;
        }
        private void NumberInCoord(float number, float x, float y)
        {
            /*TextBlock textBlock = new TextBlock
            {
                Text = number.ToString(),
                Fontsize = 10
            };
            Canvas.SetLeft(textBlock, Logic.width + number * pixels - 4);
            Canvas.SetTop(textBlock, Logic.height + 4);
            canvas.Children.Add(textBlock);

            number = (float)Math.Round(number, 1);
            var font = new Font(Font.FontFamily, 9);
            graphics.DrawString(number.ToString(), font, Brushes.Black, x, y);
            */
        }
        private void SysCoord()
        {
            DrawLine(Logic.width / 2, 0, Logic.width / 2, Logic.height);
            DrawLine(0, Logic.height / 2, Logic.width, Logic.height / 2);
            DrawLine(Logic.width / 2, 0, (Logic.width / 2) - 5, 7);
            DrawLine(Logic.width / 2, 0, (Logic.width / 2) + 5, 7);
            DrawLine(Logic.width, Logic.height / 2, Logic.width - 7, (Logic.height / 2) - 5);
            DrawLine(Logic.width, Logic.height / 2, Logic.width - 7, (Logic.height / 2) + 5);

            float step = (float)((Logic.width / 22));
            float x = Logic.width / 2;
            float y = Logic.height / 2;
            float number = 0;
            bool chet = true;
            for (float i = 1; i <= 10; ++i)
            {
                x = x + step;
                number += Logic.size;
                DrawLine(x, y - 5, x, y + 5);
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
            x = Logic.width / 2;
            number = 0;
            chet = true;
            for (float i = 1; i <= 10; ++i)
            {
                x = x - step;
                number -= Logic.size;
                DrawLine(x, y - 5, x, y + 5);
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
            step = (float)((Logic.height / 22));
            x = Logic.width / 2;
            number = 0;
            int delta = 0;
            chet = true;
            for (float i = 1; i <= 10; ++i)
            {
                y = y - step;
                number += Logic.size;
                if ((number.ToString().Length > 3) && (chet))
                {
                    delta += 10;
                    chet = false;
                }
                DrawLine(x - 5, y, x + 5, y);
                NumberInCoord(number, x - 25 - delta, y - 5);
            }
            y = Logic.height / 2;
            delta = 0;
            chet = true;
            number = 0;
            for (float i = 1; i <= 10; ++i)
            {
                y = y + step;
                number -= Logic.size;
                if ((number.ToString().Length > 3) && (chet))
                {
                    delta += 10;
                    chet = false;
                }
                DrawLine(x - 5, y, x + 5, y);
                NumberInCoord(number, x - 25 - delta, y);
            }
        }
        private void DrawLine(float x1, float y1, float x2, float y2)
        {
            Line line = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = System.Windows.Media.Brushes.Black
            };
           pictureBox.Children.Add(line);
        }
        private void DrawCruve(Cruve cruve)
        {
            bool error = false;
            float step = (float)((Logic.width / 22) / Logic.size);
            List<float> xLst = new List<float>();
            for (float i = -Logic.size * 11; i < Logic.size * 11; i += Logic.size / 4)
            {
                xLst.Add(i);
            }

            List<PointF> lst = cruve.Raschet(xLst, out error);
            lst[0] = new PointF(Logic.width / 2 + step * lst[0].X, Logic.height / 2 - step * lst[0].Y);
            for (int i = 1; i < lst.Count; ++i)
            {
                lst[i] = (new PointF(Logic.width / 2 + step * lst[i].X, Logic.height / 2 - step * lst[i].Y));
                DrawLine(lst[i - 1].X, lst[i - 1].Y, lst[i].X, lst[i].Y);
                //DrawLine(lst[i - 1], lst[i]);
            }
        }
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            //Cruve cr = (Cruve)comboBoxCurves.SelectedItem;
            Cruve cr = (Cruve)Logic.SelectedObj;
            pictureBox.Children.Clear();
            SysCoord();
            if (cr != null)
            {
                DrawCruve(cr);
            }
        }
    }
}
