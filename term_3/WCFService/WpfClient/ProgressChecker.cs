using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient
{
    public class ProgressChecker : INotifyPropertyChanged
    {
        private string _progress;

        public string Progress
        {
            get
            {
                return _progress;
            }

            set
            {
                if (_progress == value) return;

                _progress = value;
                OnPropertyChanged("Progress");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
