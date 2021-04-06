using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows;

namespace WpfClient
{
    public class ImageChecker : INotifyPropertyChanged
    {
        //BitmapSource bitmapImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(imageSource.GetHbitmap(), IntPtr.Zero,
          //              Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(imageSource.Width, imageSource.Height));
        private BitmapSource _img;

        public BitmapSource Img
        {
            get
            {
                return _img;
            }

            set
            {
//                if (_img == value) return;

                _img = value;
                OnPropertyChanged("Img");
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
