using System;
using System.ServiceModel;
using System.Drawing;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using ServiceReference;

namespace FilterClient
{
    public partial class MainWindow : Window, IFilterServiceCallback
    {
        FilterServiceClientBase client;
        volatile bool isWorking;
        Bitmap image;
        byte[] arrayOfBytes;

        public MainWindow()
        {
            InitializeComponent();
            client = new FilterServiceClientBase(new InstanceContext(this));
            isWorking = true;

            try
            {
                string[] filters = client.GetFilters();
                foreach (string f in filters)
                {
                    filtersComboBox.Items.Add(f);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ImageCallback(byte[] bytes)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {
                Bitmap filteredImage = null;
                arrayOfBytes = bytes;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    filteredImage = (Bitmap)System.Drawing.Image.FromStream(ms);
                }
                BitmapSource imgFormat = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(filteredImage.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(image.Width, image.Height));
                windowImage.Source = imgFormat;
                progressBar.Value = 0;
            }));

        }

        public void ProgressCallback(int progress)
        {
            if (isWorking)
            {
                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate { progressBar.Value = progress; }));
            }
        }

        private void SelectImageButtonClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Images(*.jpg; *.png; *.bmp) | *.jpg; *.png; *.bmp"
            };

            string path = (bool)dialog.ShowDialog() ? dialog.FileName : null;

            try
            {
                image = new Bitmap(path);
                BitmapSource imgFormat = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(image.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(image.Width, image.Height));
                windowImage.Source = imgFormat;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ApplyFilterButtonClicked(object sender, RoutedEventArgs e)
        {
            if (image != null)
            {
                byte[] bytes = null;

                if (!isWorking)
                {
                    client = new FilterServiceClientBase(new InstanceContext(this));
                }

                isWorking = true;

                if (filtersComboBox.SelectedItem != null)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                            bytes = ms.GetBuffer();
                        }

                        client.ApplyFilter(bytes, (string)filtersComboBox.SelectedItem);
                        cancelButton.IsEnabled = true;
                        cancelButton.Visibility = Visibility.Visible;
                    }
                    catch
                    {
                        MessageBox.Show("Host connection lost");
                        Application.Current.Shutdown();
                    }
                }
            }
            else
            {
                MessageBox.Show("No image selected");
                return;
            }
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            progressBar.Value = 0;
            client.Abort();
            isWorking = false;
        }

        private void SaveImage(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveSelection = new SaveFileDialog
            {
                Title = "Save the image",
                Filter = "All Files| *.*"
            };

            if ((bool)saveSelection.ShowDialog())
            {
                using MemoryStream ms = new MemoryStream(arrayOfBytes);
                Bitmap savingImage = (Bitmap)System.Drawing.Image.FromStream(ms);
                savingImage.Save(saveSelection.FileName);
            }
        }
    }
}