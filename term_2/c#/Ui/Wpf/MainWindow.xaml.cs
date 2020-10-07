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

namespace Wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private float size;
        private float height;
        private float width;
        public MainWindow()
        {
            InitializeComponent();
            size = 1;
            graphics = pictureBox.CreateGraphics();
            width = (float)pictureBox.Width;
            height = (float)pictureBox.Height;
            labelSize.Text = "Size: " + size.ToString();
            comboBoxCurves.Items.AddRange(new Cruve[] { new Parabola(), new ClassicParabola(), new Circle() });
        }
    }
}
