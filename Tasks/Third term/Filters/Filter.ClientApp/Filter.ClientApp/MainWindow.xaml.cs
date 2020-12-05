using Filter.AdditionLib;

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
        private delegate void listenDelegate();

        private BitmapSource sourceImage = null;
        private BitmapSource resultImage = null;
        private StructOfImage DataOfImage;
        private volatile TcpClient client = null;
        private volatile NetworkStream networkStream = null;
        private volatile bool isConnect = false;
        private volatile bool isListening = false;
        private List<string> availableFilters;

        private Thread getStream;
        private volatile bool isReceive = false;
        private volatile List<byte> receivedData = null;

        public MainWindow()
        {
            InitializeComponent();
            availableFilters = new List<string>();
            receivedData = new List<byte>();
            getStream = null;
        }

        private void DownloadImage(object sender, RoutedEventArgs e)
        {
            // C:\Users\pavel\Pictures\wallpapers\1.jpg
            // C:\Users\pavel\Desktop\test.png      10*9
            // C:\Users\pavel\Desktop\1.jpg      many pixels
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
                image = null;
                ControlAction.Background = Brushes.Green;
            }
            catch (Exception ex)
            {
                ExceptionConsole.Text = $"{ex}";
                ControlAction.Background = Brushes.Red;
            }
        }

        private void SendImage(object sender, RoutedEventArgs e)
        {
            if (!isConnect)
            {
                ControlAction.Background = Brushes.Red;
                ConnectionStatus.Text = "Disconnection";
                return;
            }
            try
            {
                ResultImage.Children.Clear();
                networkStream = client.GetStream();
                ControlAction.Background = Brushes.Green;

                List<byte> sendingFilter = new List<byte>();
                byte[] chosenFilter = SecondaryFunctions.TranslateToByteArrayUnicode((string)MyComboBox.SelectedItem, (byte)Protocol.Filter);
                int lenFilter = 4 + chosenFilter.Length;
                sendingFilter.AddRange(BitConverter.GetBytes(lenFilter));
                sendingFilter.AddRange(chosenFilter);

                networkStream.Write(sendingFilter.ToArray(), 0, lenFilter);

                int lenImage = 4 + 1 + 4 + 4 + DataOfImage.Pixels.Length;
                List<byte> sendBytes = new List<byte>(lenImage);
                sendBytes.AddRange(BitConverter.GetBytes(lenImage));
                sendBytes.Add((byte)Protocol.Image);
                sendBytes.AddRange(BitConverter.GetBytes(DataOfImage.Height));
                sendBytes.AddRange(BitConverter.GetBytes(DataOfImage.Width));
                sendBytes.AddRange(DataOfImage.Pixels);

                networkStream.Write(sendBytes.ToArray(), 0, lenImage);
                Listen();

            }
            catch (Exception ex)
            {
                ExceptionConsole.Text = $"{ex}";
                ControlAction.Background = Brushes.Red;
                client.Close();
                networkStream.Close();
                ConnectionStatus.Text = "Disconnection";
                isConnect = false;
                return;
            }
        }

        private void GetStream()
        {
            networkStream = client.GetStream();
            try
            {
                byte[] bufferLenOfMessage = new byte[4];
                networkStream.Read(bufferLenOfMessage, 0, 4);
                int lenOfMessage = BitConverter.ToInt32(bufferLenOfMessage, 0);
                byte[] bytesToRead = new byte[lenOfMessage - 4];

                receivedData = new List<byte>(lenOfMessage);
                int amount = 0;
                int bytesRead = 0;
                while (amount < lenOfMessage - 4)
                {
                    bytesRead = networkStream.Read(bytesToRead, amount, lenOfMessage - 4 - amount);
                    amount += bytesRead;
                }
                receivedData.AddRange(bufferLenOfMessage);
                receivedData.AddRange(bytesToRead);

                isReceive = true;
            }
            catch
            {
                client.Close();
                networkStream.Close();
                //ControlAction.Background = Brushes.Black;
                isListening = false;
                isConnect = false;
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
            if (isReceive && isListening)
            {
                while (receivedData.Count != 0)
                {
                    int lenOfMessage = BitConverter.ToInt32(receivedData.ToArray(), 0);
                    byte[] data = new byte[lenOfMessage - 4];
                    receivedData.CopyTo(4, data, 0, lenOfMessage - 4);
                    receivedData.RemoveRange(0, lenOfMessage);

                    if (data[0] == (byte)Protocol.Image)
                    {
                        data = data.Skip(1).ToArray();
                        bool isSuccess = SecondaryFunctions.ByteArrayToImage(data, DataOfImage, out resultImage);
                        if (isSuccess)
                        {
                            Image image = new Image();
                            image.Source = resultImage;
                            image.Width = DataOfImage.Width;
                            image.Height = DataOfImage.Height;
                            ResultImage.Children.Add(image);
                            image = null;
                        }
                        else
                        {
                            ControlAction.Background = Brushes.Yellow;
                        }
                        isListening = false;
                    }
                    else if (data[0] == (byte)Protocol.Progress)
                    {
                        if (data[1] == (int)Protocol.StopCode)
                        {
                            isListening = false;
                            Progress.Value = 0;
                        }
                        else
                            Progress.Value = data[1];
                        if (isListening)
                            Listen();
                    }
                    else if (data[0] == (byte)Protocol.Filter)
                    {
                        string tempFilter = SecondaryFunctions.TranslateByteArrayToStringUnicode(data);
                        availableFilters = tempFilter.Split(' ').ToList();
                        MyComboBox.ItemsSource = availableFilters;
                        isListening = false;
                    }
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
                ConnectionStatus.Text = "Connection";
                Listen();
            }
            else
            {
                ControlAction.Background = Brushes.Red;
                isConnect = false;
                ConnectionStatus.Text = "Disconnection";
            }
        }

        private void SendStop(object sender, RoutedEventArgs e)
        {
            if (!isConnect)
            {
                ControlAction.Background = Brushes.Red;
                ConnectionStatus.Text = "Disconnection";
                return;
            }
            networkStream = client.GetStream();
            try
            {
                ControlAction.Background = Brushes.Green;
                List<byte> sendBytes = new List<byte>(6);
                int len = 4 + 1 + 1;
                sendBytes.AddRange(BitConverter.GetBytes(len));
                sendBytes.Add((byte)Protocol.Progress);
                sendBytes.Add((byte)Protocol.StopCode);
                networkStream.Write(sendBytes.ToArray(), 0, len);
                Listen();
            }
            catch (Exception ex)
            {
                ExceptionConsole.Text = $"{ex}";
                ControlAction.Background = Brushes.Red;
                client.Close();
                networkStream.Close();
                isConnect = false;
                ConnectionStatus.Text = "Disconnection";
                return;
            }
        }

        private void Disconnect(object sender, RoutedEventArgs e)
        {
            try
            {
                getStream.Abort();
                client.Close();
                networkStream.Close();
                isConnect = false;
                ConnectionStatus.Text = "Disconnection";
            }
            catch (Exception ex)
            {
                ExceptionConsole.Text = $"{ex}";
            }
        }
    }
}