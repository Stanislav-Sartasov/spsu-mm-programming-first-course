using System.Windows.Forms;

namespace Task9WF
{
	public partial class StartPort : Form
	{
		public StartPort()
		{
			InitializeComponent();
		}

		public int Port
		{
			get
			{
				try
				{
					return int.Parse(portTextBox.Text);
				}
				catch
				{
					return -1;
				}
			}
		}

		private void PortTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				if (Port != -1)
				{
					e.SuppressKeyPress = false;
					startButton.Focus();
				}
		}
	}
}
