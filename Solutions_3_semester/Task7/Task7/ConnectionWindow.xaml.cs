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
using System.Windows.Shapes;
using System.Net;

namespace Task7
{
	/// <summary>
	/// Логика взаимодействия для ConnectionWindow.xaml
	/// </summary>
	public partial class ConnectionWindow : Window
	{
		public ConnectionWindow()
		{
			InitializeComponent();
		}

		public IPEndPoint Adress { get; private set; } = null;
		public string[] Filters { get; private set; } = null;

		private void IpAdressTextBoxGetFocus(object sender, RoutedEventArgs e)
		{
			if (IpAdressTextBox.Text == "ip adress")
				IpAdressTextBox.Text = "";
		}

		private void IpAdressTextBoxLostFocus(object sender, RoutedEventArgs e)
		{
			if (IpAdressTextBox.Text == "")
				IpAdressTextBox.Text = "ip adress";
		}
		private void IpAdressTextBoxKeyPressed(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				ConnectButton.Focus();
				ConnectButtonClick(sender, new RoutedEventArgs());
			}
		}

		private void ConnectButtonClick(object sender, RoutedEventArgs e)
		{
			try
			{
				string[] input = IpAdressTextBox.Text.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
				if (input.Length != 2)
					throw new FormatException();
				IPAddress address = IPAddress.Parse(input[0]);
				int port = int.Parse(input[1]);
				IPEndPoint endPoint = new IPEndPoint(address, port);
				Filters = FilterGetter.GetFilters(endPoint);
				Adress = endPoint;
				DialogResult = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				IpAdressTextBox.Focus();
				IpAdressTextBox.SelectAll();
			}
		}
	}
}
