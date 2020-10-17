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
        private delegate void messageHandlingDelegate();

        private BitmapSource sourceImage = null;
        private BitmapSource resultImage = null;
        private StructOfImage DataOfImage;
        private TcpClient client = null;
        private NetworkStream networkStream = null;
        private bool isConnect = false;
        private bool isListening = false;
        private List<string> availableFilters;

        private Thread getStream;
        private bool isReceive = false;
        private byte[] receivedData = null;

        // black - проблемы с подключением, yellow - не удалось получить изображение из массива байтов
        public MainWindow()
        {
            InitializeComponent();
            availableFilters = new List<string>();
            getStream = null;
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
            catch (Exception ex)
            {
                ExceptionConsole.Text = $"{ex}";
                ControlAction.Background = Brushes.Red;
            }
        }

        // отправляемые: 1 - filter, 2 - image, 3 - stop
        // получаемые: 1 - картинка, 2 - прогресс, 3 - filters
        private void SendImage(object sender, RoutedEventArgs e)
        {
            if (!isConnect)
            {
                ControlAction.Background = Brushes.Red;
                return;
            }
            networkStream = client.GetStream();
            try
            {
                ControlAction.Background = Brushes.Green;

                byte[] chosenFilter = SecondaryFunctions.TranslateToByteArrayUnicode((string)MyComboBox.SelectedItem, 1);


                byte[] pixels = null;
                bool isSuccess = SecondaryFunctions.ImageToByteArray(sourceImage, out pixels);
                byte[] sendBytes = new byte[1 + 4 + 4 + pixels.Length];
                sendBytes[0] = 2;

                byte[] temp = BitConverter.GetBytes(DataOfImage.Height);
                for (int i = 0; i < 4; i++)
                    sendBytes[i + 1] = temp[i];
                temp = BitConverter.GetBytes(DataOfImage.Width);
                for (int i = 0; i < 4; i++)
                    sendBytes[i + 5] = temp[i];

                for (int i = 0; i < pixels.Length; i++)
                    sendBytes[i + 9] = pixels[i];
                if (isSuccess)
                {
                    networkStream.Write(chosenFilter, 0, chosenFilter.Length);
                    Thread.Sleep(100); // против склейки буфера, исправить
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    DataOfImage.Pixels = null;
                    Listen();
                }
                else
                    ControlAction.Background = Brushes.Red;
            }
            catch (Exception ex)
            {
                ExceptionConsole.Text = $"{ex}";
                ControlAction.Background = Brushes.Red;
                client.Close();
                networkStream.Close();
                return;
            }
        }

        private void GetStream()
        {
            networkStream = client.GetStream();
            try
            {

                byte[] buffer = new byte[256];
                List<byte> data = new List<byte>(256);
                int temp = 0;
                int amount = 0;
                do
                {
                    temp = networkStream.Read(buffer, 0, buffer.Length);
                    data.AddRange(buffer);
                    amount += temp;
                }
                while (temp != 0 && networkStream.DataAvailable);
                data.RemoveRange(amount, data.Count - amount);
                receivedData = data.ToArray();
                isReceive = true;
            }
            catch
            {
                client.Close();
                networkStream.Close();
                ControlAction.Background = Brushes.Black;
                isListening = false;
                return;
            }
        }

        private void Listen()
        {
            getStream = new Thread(GetStream);
            isListening = true;
            isReceive = false;
            getStream.Start();
            if (isListening)
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new messageHandlingDelegate(MessageHandling));
        }

        private void MessageHandling()
        {
            if (isReceive)
            {
                if (receivedData[0] == 1) // image
                {
                    receivedData = receivedData.Skip(1).ToArray();
                    bool isSuccess = SecondaryFunctions.ByteArrayToImage(receivedData, DataOfImage, out resultImage);
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
                if (receivedData[0] == 2) //progress bar
                {
                    Progress.Value = receivedData[1];
                    Listen();
                }
                if (receivedData[0] == 3) //list of filters
                {
                    string tempFilter = SecondaryFunctions.TranslateByteArrayToStringUnicode(receivedData);
                    availableFilters = tempFilter.Split(' ').ToList();
                    MyComboBox.ItemsSource = availableFilters;
                    isListening = false;
                }
            }
            if (isListening)
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle, new messageHandlingDelegate(MessageHandling));
        }

        private void ConnectToServer(object sender, RoutedEventArgs e)
        {
            IPEndPoint ip = null;
            bool isSuccess = SecondaryFunctions.TryParseIp(ServerAddress.Text, out ip);
            if (isSuccess)
            {
                client = new TcpClient();
                client.Connect(ip);
                ControlAction.Background = Brushes.Green;
                isConnect = true;
                Listen();
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
            try
            {
                ControlAction.Background = Brushes.Green;
                networkStream.Write(new byte[1] { 3 }, 0, 1);
                Progress.Value = 0;
                isListening = false;
                getStream.Abort();
            }
            catch (Exception ex)
            {
                ExceptionConsole.Text = $"{ex}";
                ControlAction.Background = Brushes.Red;
                client.Close();
                networkStream.Close();
                return;
            }
        }

        private void Disconnect(object sender, RoutedEventArgs e)
        {
            try
            {
                getStream.Abort();
                client.Close();
            }
            catch (Exception ex)
            {
                ExceptionConsole.Text = $"{ex}";
            }
        }
    }
}