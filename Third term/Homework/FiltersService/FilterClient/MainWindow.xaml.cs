using System;
using System.IO;
using System.Drawing;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using FilterClient.FilterService;
using System.Drawing.Imaging;
using System.Windows.Interop;
using System.ServiceModel;

namespace FilterClient
{
    public partial class MainWindow : Window, IFilterServiceCallback
    {
        private Bitmap selectedImage;
        private FilterServiceClient client;
        private volatile bool isImgSelected = false;

        public MainWindow()
        {
            InitializeComponent();
            client = new FilterServiceClient(new InstanceContext(this));
            try
            {
                filtersCB.ItemsSource = client.GetFilters();
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться к хосту");
                Application.Current.Shutdown();
            }
        }

        public void ImageCallback(byte[] img)
        {
            applyFilterButton.IsEnabled = true;
            if (!(img is null))
            {
                Bitmap bmp;
                using (var ms = new MemoryStream(img))
                {
                    bmp = new Bitmap(ms);
                }
                applyFilterButton.IsEnabled = true;
                progressBar.Value = 0;
                var imgSource = Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero,
                         Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height));
                windowImg.Source = imgSource;
            }
        }

        private void SelectImgButtonClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Images (*.jpg;*.png;*.bmp) | *.jpg;*.png;*.bmp"
            };

            if (dlg.ShowDialog() ?? false)
            {
                string filename = dlg.FileName;
                selectedImage = new Bitmap(filename);
                windowImg.Source = new BitmapImage(new Uri(filename));
                isImgSelected = true;
            }
        }

        private void SaveImgButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!isImgSelected)
            {
                MessageBox.Show("Изображение не выбрано!");
                return;
            }

            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "Jpeg image (*.jpg)|*.jpg|Bitmap image (*.bmp)|*.bmp|Png image (*.png)|*.png",
                Title = "Save an image file"
            };
            dlg.ShowDialog();

            if (!string.IsNullOrWhiteSpace(dlg.FileName))
            {
                BitmapEncoder encoder = null;
                switch (Path.GetExtension(dlg.FileName))
                {
                    case ".jpg":
                        encoder = new JpegBitmapEncoder();
                        break;

                    case ".bmp":
                        encoder = new BmpBitmapEncoder();
                        break;

                    case ".png":
                        encoder = new PngBitmapEncoder();
                        break;
                    default:
                        MessageBox.Show("Неподдерживаемый формат!");
                        break;
                }
                if (!(encoder is null))
                {
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)windowImg.Source));
                    using (FileStream stream = new FileStream(dlg.FileName, FileMode.Create))
                        encoder.Save(stream);
                }
            }
        }

        private void ApplyFilterButtonClicked(object sender, RoutedEventArgs e)
        {
            if (isImgSelected)
            {
                string filterName = (string)filtersCB.SelectedItem;
                if (!(filterName is null))
                {
                    try
                    {
                        client = new FilterServiceClient(new InstanceContext(this));
                        using (var ms = new MemoryStream())
                        {
                            selectedImage.Save(ms, ImageFormat.Bmp);
                            client.ApplyFilter(ms.ToArray(), filterName);
                            applyFilterButton.IsEnabled = false;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Соединение с хостом потеряно");
                        Application.Current.Shutdown();
                    }
                }
            }
            else
            {
                MessageBox.Show("Изображение не выбрано!");
                return;
            }
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                client.StopFiltering();
                applyFilterButton.IsEnabled = true;
            }
            catch
            {
                MessageBox.Show("Соединение с хостом потеряно");
                Application.Current.Shutdown();
            }
        }

        public void ProgressCallback(int progress) => progressBar.Value = progress;
    }
}
