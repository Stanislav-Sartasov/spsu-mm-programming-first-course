using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace Filter.ClientApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void listenDelegate();

        private BitmapSource sourceImage = null;
        private BitmapSource resultImage = null;
        private StructOfImage DataOfImage;
        private TcpClient client = null;
        private NetworkStream networkStream = null;
        private bool isConnect = false;
        private bool isListening = false;
        private List<string> availableFilters;

        // black - проблемы с подключением, yellow - не удалось получить изображение из массива байтов
        public MainWindow()
        {
            InitializeComponent();
            availableFilters = new List<string>();
        }

        private void DownloadImage(object sender, RoutedEventArgs e)
        {
            // C:\Users\pavel\Pictures\wallpapers\1.jpg
            // C:\Users\pavel\Desktop\test.png      10*9
            SourceImage.Children.Clear();
            try
            {
                string path = FileAddress.Text;
                sourceImage = new BitmapImage(new Uri(path));
                SecondaryFunctions.FillStructOfImage(sourceImage, out DataOfImage);

                Image image = new Image();
                image.Source = sourceImage;
                image.Width = DataOfImage.Width;
                image.Height = DataOfImage.Height;

                SourceImage.Children.Add(image);

                ControlAction.Background = Brushes.Green;
            }
            catch
            {
                ControlAction.Background = Brushes.Red;
            }
        }

        // 1 - filter, 2 - image, 3 - stop
        private void SendImage(object sender, RoutedEventArgs e)
        {
            if (!isConnect)
            {
                ControlAction.Background = Brushes.Red;
                return;
            }
            networkStream = client.GetStream();
            if (networkStream.CanWrite)
            {
                ControlAction.Background = Brushes.Green;

                byte[] chosenFilter = SecondaryFunctions.TranslateToByteArrayUnicode((string)MyComboBox.SelectedItem, 1);


                byte[] pixels = null;
                bool isSuccess = SecondaryFunctions.ImageToByteArray(sourceImage, out pixels);
                byte[] sendBytes = new byte[1 + pixels.Length];
                sendBytes[0] = 2;
                for (int i = 1; i < sendBytes.Length; i++)
                    sendBytes[i] = pixels[i - 1];
                if (isSuccess)
                {
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    isListening = true;
                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new listenDelegate(Listen));
                }
                else
                    ControlAction.Background = Brushes.Red;

                
            }
            else
            {
                ControlAction.Background = Brushes.Red;
                client.Close();
                networkStream.Close();
                return;
            }
        }

        private void Listen()
        {
            if (networkStream.CanRead)
            {
                byte[] bytes = new byte[client.ReceiveBufferSize];
                networkStream.Read(bytes, 0, (int)client.ReceiveBufferSize);
                if (bytes[0] == 1) // image
                {
                    bytes = bytes.Skip(1).ToArray();
                    bool isSuccess = SecondaryFunctions.ByteArrayToImage(bytes, DataOfImage, out resultImage);
                    if (isSuccess)
                    {
                        Image image = new Image();
                        image.Source = resultImage;
                        ResultImage.Children.Add(image);
                    }
                    else
                    {
                        ControlAction.Background = Brushes.Yellow;
                    }
                    isListening = false;
                }
                if (bytes[0] == 2) //progress bar
                {
                    Progress.Value = bytes[1];
                    isListening = true;
                }
                if (bytes[0] == 3) //list of filters
                {
                    string temp = SecondaryFunctions.TranslateByteArrayToStringUnicode(bytes);
                    availableFilters = temp.Split(' ').ToList();
                    MyComboBox.ItemsSource = availableFilters;
                    isListening = false;
                }
            }
            else
            {
                client.Close();
                networkStream.Close();
                ControlAction.Background = Brushes.Black;
                isListening = false;
                return;
            }
            if (isListening)
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new listenDelegate(Listen));
        }

        private void ConnectToServer(object sender, RoutedEventArgs e)
        {
            IPEndPoint ip = null;
            bool isSuccess = SecondaryFunctions.TryParseIp(ServerAddress.Text, out ip);
            if (isSuccess)
            {
                client = new TcpClient(ip);
                ControlAction.Background = Brushes.Green;
                isConnect = true;
            }
            else
            {
                ControlAction.Background = Brushes.Red;
                isConnect = false;
            }
        }

        private void SendStop(object sender, RoutedEventArgs e)
        {
            if (!isConnect)
            {
                ControlAction.Background = Brushes.Red;
                return;
            }
            networkStream = client.GetStream();
            if (networkStream.CanWrite)
            {
                ControlAction.Background = Brushes.Green;
                networkStream.Write(new byte[1] { 3 }, 0, 1);
                Progress.Value = 0;
            }
            else
            {
                ControlAction.Background = Brushes.Red;
                client.Close();
                networkStream.Close();
                return;
            }
        }
    }
}
