using System;
using System.Windows.Forms;

namespace Task9WF
{
    public partial class NewConnectionForm : Form
    {
        public NewConnectionForm()
        {
            InitializeComponent();
        }

        public string Address
        {
            get
            {
                return addressTextBox.Text;
            }
        }

        public string Port
        {
            get
            {
                return portTextBox.Text;
            }
        }

        private void AddressTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (Address != "" && Port != "")
                {
                    e.SuppressKeyPress = true;
                    tryConnectButton.Focus();
                }
                else if (Address != "" && Port == "")
                {
                    e.SuppressKeyPress = true;
                    portTextBox.Focus();
                }
        }

        private void PortTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (Address != "" && Port != "")
                {
                    e.SuppressKeyPress = true;
                    tryConnectButton.Focus();
                }
        }

        private void TryConnectButton_Click(object sender, EventArgs e)
        {
            if (Address == "" || Port == "")
                DialogResult = DialogResult.No;
        }
    }
}
