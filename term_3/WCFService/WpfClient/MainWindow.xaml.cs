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
using System.ServiceModel;
using FilterServiceLibrary;
using System.Threading;
/// <summary>
using System.Drawing;


/// ///////////////
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows.Threading;
/// </summary>

namespace WpfClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource tokenSourceStop = null;
        private Task filterWaitTask;
        private Task statusWaitTask;
        private volatile bool isDone = false;
        private ChannelFactory<IContract> factory = null;
        private string sourcePath;
        private volatile IContract client = null;
        private volatile ImageChecker imageControl = null;
        private volatile ProgressChecker labelControl = null;
        private Bitmap imageSource = null;
        private volatile byte[] imageToSave = null;
        public MainWindow()
        {
            InitializeComponent();
            buttonImageSelect.IsEnabled = true;
            labelControl = new ProgressChecker();
            labelProgress.DataContext = labelControl;
            labelControl.Progress = "prgress precent";
            imageControl = new ImageChecker();
            boxImageShow.DataContext = imageControl;
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None, false);
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            factory = new ChannelFactory<IContract>(binding, new EndpointAddress("net.tcp://localhost:12345/srv"));
            client = factory.CreateChannel();

            try
            {
                string[] filters = client.GetFilters();
                foreach (var filter in filters)
                {
                    comboBoxFilter.Items.Add(filter);
                }
                if (filters.Length < 1)
                    throw new Exception("No Filters found");
                comboBoxFilter.SelectedItem = comboBoxFilter.Items[0];
                buttonImageSelect.IsEnabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void ParallelImageFilter(IContract client, byte[] inputImage, string filter, ImageChecker control, CancellationToken cancellation)
        {
            try
            {
                byte[] imgBytesRecived = client.ApplyFilter(filter, inputImage);
                imageToSave = imgBytesRecived;
                isDone = true;
                App.Current.Dispatcher.Invoke(() =>
                {
                    Bitmap imageTemp;
                    using (MemoryStream memoryStream = new MemoryStream(imageToSave))
                    {
                        imageTemp = (Bitmap)Bitmap.FromStream(memoryStream);
                    }
                    if (imageSource != null)
                    {
                        imageSource.Dispose();
                    }
                    imageSource = (Bitmap)imageTemp.Clone();
                    BitmapSource bitmapImage = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(imageTemp.GetHbitmap(), IntPtr.Zero,
                            Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(imageTemp.Width, imageTemp.Height));
                    if (imageToSave != null)
                        imageControl.Img = bitmapImage;
                });
            }
            catch (Exception e)
            {
                if (cancellation.IsCancellationRequested)
                {
                    return;
                }
                MessageBox.Show("Error in parallelFilterClient\n" + e.Message);
            }
            return;
        }
        private void ParallelImageStatus(IContract client, ProgressChecker checker, CancellationToken cancellation)
        {
            try
            {
                int progress = 0;
                Thread.Sleep(900);
                while (progress < 100)
                {
                    progress = client.GetStatus();
                    checker.Progress = progress.ToString() + " %";
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        if (progress < 100)
                            progressBar.Value = progress;
                        else
                        {
                            progressBar.Value = 100;
                            if (progress < 0)
                            {
                                checker.Progress = "Error in progress";
                                Thread.Sleep(3000);
                            }
                        }
                    });
                    Thread.Sleep(800);
                }
                checker.Progress = "all done";
                Thread.Sleep(800);
                App.Current.Dispatcher.Invoke(() =>
                {
                    buttonSend.IsEnabled = true;
                    buttonImageSelect.IsEnabled = true;
                    buttonSave.IsEnabled = true;
                    comboBoxFilter.IsEnabled = true;
                });
            }
            catch (Exception e)
            {
                if (cancellation.IsCancellationRequested)
                {
                    return;
                }
                MessageBox.Show("Error in parallelStatusClient\n" + e.Message);
            }
            return;
        }
        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog imageSelection = new OpenFileDialog();
            imageSelection.Title = "Select image";
            imageSelection.Filter = "Все файлы (*.*) | *.*";


            if ((bool)imageSelection.ShowDialog())
            {
                imageToSave = null;
                sourcePath = imageSelection.FileName;

                try
                {
                    if (imageSource != null)
                        imageSource.Dispose();
                    this.imageSource = new Bitmap(sourcePath);

                    BitmapSource bitmapImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(imageSource.GetHbitmap(), IntPtr.Zero,
                        Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(imageSource.Width, imageSource.Height));
                    imageControl.Img = bitmapImageSource;
                    boxImageShow.IsEnabled = true;
                    comboBoxFilter.IsEnabled = true;
                    buttonSend.IsEnabled = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxFilter.SelectedItem == null)
            {
                MessageBox.Show("No filter selected");
                return;
            }
            labelControl.Progress = "prgress precent";
            progressBar.Value = 0;
            buttonSend.IsEnabled = false;
            buttonImageSelect.IsEnabled = false;
            buttonSave.IsEnabled = false;
            comboBoxFilter.IsEnabled = false;
            byte[] bytesForSend = null;
            if (imageToSave != null)
                bytesForSend = imageToSave;
            else
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    imageSource.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                    bytesForSend = memoryStream.GetBuffer();
                }
            }
            if (tokenSourceStop != null)
            {
                tokenSourceStop.Dispose();
            }
            tokenSourceStop = new CancellationTokenSource();
            CancellationToken cancellToken = tokenSourceStop.Token;
            string filterSelected = comboBoxFilter.SelectedItem.ToString();
            filterWaitTask = new Task(() => ParallelImageFilter(client, bytesForSend, filterSelected, imageControl, cancellToken));
            statusWaitTask = new Task(() => ParallelImageStatus(client, labelControl, cancellToken));
            filterWaitTask.Start();
            statusWaitTask.Start();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveSelection = new SaveFileDialog();

            saveSelection.Title = "Save the image";
            saveSelection.Filter = "Все файлы (*.*) | *.*";

            string stamp = comboBoxFilter.SelectedItem.ToString() + sourcePath.Substring(sourcePath.LastIndexOf('.'));
            sourcePath = sourcePath.Remove(sourcePath.LastIndexOf('.'));
            sourcePath += stamp;
            saveSelection.FileName = stamp;// sourcePa

            if ((bool)saveSelection.ShowDialog())
            {
                try
                {
                    //labelControl.Progress = saveSelection.FileName;
                    Bitmap image;
                    using (MemoryStream memoryStream = new MemoryStream(imageToSave))
                    {
                        image = (Bitmap)Bitmap.FromStream(memoryStream);
                        image.Save(saveSelection.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void ButtonReboot_Click(object sender, RoutedEventArgs e)
        {
            labelControl.Progress = "prgress precent";
            progressBar.Value = 0;
            if (tokenSourceStop != null)
            {
                tokenSourceStop.Cancel();
                //tokenSourceStop.Dispose();
            }
            if (filterWaitTask != null)
            {
                if (filterWaitTask.IsCompleted)
                    filterWaitTask.Dispose();
                filterWaitTask = null;
            }
            if (statusWaitTask != null)
            {
                if (statusWaitTask.IsCompleted)
                    statusWaitTask.Dispose();
                statusWaitTask = null;
            }
            factory.Abort();
            client = null;
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None, false);
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            factory = new ChannelFactory<IContract>(binding, new EndpointAddress("net.tcp://localhost:12345/srv"));
            client = factory.CreateChannel();
            client.GetStatus();
            buttonSend.IsEnabled = true;
            buttonImageSelect.IsEnabled = true;
            buttonSave.IsEnabled = true;
            comboBoxFilter.IsEnabled = true;
            boxImageShow.IsEnabled = true;
            try
            {
                if (isDone)
                {
                    Bitmap imageTemp;
                    using (MemoryStream memoryStream = new MemoryStream(imageToSave))
                    {
                        imageTemp = (Bitmap)Bitmap.FromStream(memoryStream);
                        //image.Save("ShlapaF.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                    if (imageSource != null)
                    {
                        imageSource.Dispose();
                    }
                    imageSource = (Bitmap)imageTemp.Clone();
                    BitmapSource bitmapImage = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(imageTemp.GetHbitmap(), IntPtr.Zero,
                            Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(imageTemp.Width, imageTemp.Height));
                    if (imageToSave != null)
                        imageControl.Img = bitmapImage;
                    imageTemp.Dispose();
                }
            }
            catch(Exception exep)
            {
                isDone = false;
            }
            isDone = false;
            Thread.Sleep(900);
        }
    }
}
