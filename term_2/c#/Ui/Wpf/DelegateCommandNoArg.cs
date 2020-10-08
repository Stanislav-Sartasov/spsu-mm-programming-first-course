using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wpf
{
	public class DelegateCommandNoArg : ICommand
	{
		Action act;
		public DelegateCommandNoArg(Action action)
		{
			act = action;
		}
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			act?.Invoke();
		}
	}
}
