using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Net;
using System.Windows.Threading;

namespace Task7
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		IPEndPoint adress = null;
		SendingRoutine sending = SendingRoutine.GetFinished();

		private void ChangeFileButtonClick(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == true)
				StartImage.Source = new BitmapImage(new Uri(dialog.FileName));
			SendButton.IsEnabled = SendAvailable;
		}

		private void ServerConnectButtonClick(object sender, RoutedEventArgs e)
		{
			ConnectionWindow dialog = new ConnectionWindow();
			if (dialog.ShowDialog() == true)
			{
				Abort(sender, new RoutedEventArgs());
				FilterSelectComboBox.ItemsSource = null;
				FilterSelectComboBox.ItemsSource = dialog.Filters;
				FilterSelectComboBox.IsEnabled = true;
				adress = dialog.Adress;
				Window.Title = $"Main Window, last connection: {adress}";
			}
			SendButton.IsEnabled = SendAvailable;
		}
		private void FilterSelectComboBoxSelected(object sender, SelectionChangedEventArgs e)
		{
			SendButton.IsEnabled = SendAvailable;
		}

		private void Send(object sender, RoutedEventArgs e)
		{
			if (sending.Finished)
			{
				sending = new SendingRoutine(
					ProgressBarChange,
					MessageBoxShow,
					SetImage,
					SendingFinishedRoutine
					);
				AbortButton.IsEnabled = true;
				SendButton.IsEnabled = false;
				FilterSelectComboBox.IsEnabled = false;
				var image = (BitmapImage)StartImage.Source.Clone();
				var addr = adress;

				byte[] data;
				JpegBitmapEncoder encoder = new JpegBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(image));
				using (MemoryStream ms = new MemoryStream())
				{
					encoder.Save(ms);
					data = ms.ToArray();
				}

				sending.Send(data, addr, (string)((string)FilterSelectComboBox.SelectedItem).Clone());
			}
		}

		void ProgressBarChange(double value)
		{
			Dispatcher.Invoke(() => ProgressBar.Value = value);
		}
		void MessageBoxShow(Exception ex)
		{
			Dispatcher.Invoke(() => MessageBox.Show(ex.Message, "Error"));
		}
		void SetImage(MemoryStream image)
		{
			Dispatcher.Invoke(() =>
			{
				BitmapImage bitmap = new BitmapImage();
				bitmap.BeginInit();
				bitmap.StreamSource = image;
				bitmap.EndInit();

				FilterImage.Source = bitmap;
				SaveButton.IsEnabled = true;
			});
		}
		void SendingFinishedRoutine()
		{
			Dispatcher.Invoke(() =>
			{
				AbortButton.IsEnabled = false;
				ProgressBar.Value = 0;
				FilterSelectComboBox.IsEnabled = true;
				SendButton.IsEnabled = SendAvailable;
			});
		}

		private void Abort(object sender, RoutedEventArgs e)
		{
			sending.Abort();
			SendingFinishedRoutine();
		}

		private void SaveImage(object sender, RoutedEventArgs e)
		{
			BitmapImage image = (BitmapImage)FilterImage.Source.Clone();

			SaveFileDialog dialog = new SaveFileDialog()
			{
				FileName = "Image",
				DefaultExt = ".png"
			};

			if (dialog.ShowDialog() == true)
			{
				BitmapEncoder encoder = new JpegBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(image));

				using (var fileStream = new FileStream(dialog.FileName, FileMode.Create))
					encoder.Save(fileStream);
			}
		}

		private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Abort(sender, new RoutedEventArgs());
			if (FilterImage.Source != null)
				if (MessageBox.Show("Would you like to save image?", "Save", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
					SaveImage(sender, new RoutedEventArgs());
		}

		bool SendAvailable
		{
			get
			{
				return StartImage.Source != null && adress != null && FilterSelectComboBox.SelectedItem != null;
			}
		}
	}
}
