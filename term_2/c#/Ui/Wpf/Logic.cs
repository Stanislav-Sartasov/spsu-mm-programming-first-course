using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MathCruves;
using System.Windows.Input;

namespace Wpf
{
    public class Logic : INotifyPropertyChanged
    {
        public Logic()
        {
            size = 1;
            width = 1;
            height = 1;
            SizeStr = "Size: 1";
            LstCombo = new Cruve[] { new Parabola(), new ClassicParabola(), new Circle() };
            SelectedObj = null;
        }
        public float size;
        public float width;
        public float height;
        public event PropertyChangedEventHandler PropertyChanged;

        private string str;
        public string SizeStr
        {
            get
            {
                return str;
            }
            private set
            {
                str = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SizeStr"));
            }
        }
        public object[] LstCombo
        {
            get;
            private set;
        }
        public object SelectedObj
        {
            get;
            set;
        }
        public ICommand PlusSizeCommand
        {
            get
            {
                return new DelegateCommandNoArg(PlusSize);
            }
        }
        public void PlusSize()
        {
            if (size + 0.1f < 10.1f)
                size = size + 0.1f;
            SizeStr = "Size: " + size.ToString();
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SizeStr"));
        }
        public ICommand MinusSizeCommand
        {
            get
            {
                return new DelegateCommandNoArg(MinusSize);
            }
        }
        public void MinusSize()
        {
            if (size - 0.1f > 0.1f)
                size = size - 0.1f;
            SizeStr = "Size: " + size.ToString();
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SizeStr"));
        }
    }
}
