using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics
{
	class DynArray<T>
	{

		public int Total
		{
			get;
			set;
		} = 0;

		private T[] data = null;
		public DynArray(int _cap)
		{
			if (_cap < 1 || _cap > 1000000000)
			{
				Console.WriteLine("Error: Start capacity");
				_cap = 10;
			}
			data = new T[_cap];
		}

		public int FindIndexFromStart(T value)
		{
			for (int i = 0; i < Total; i++)
			{
				if (data[i].Equals(value))
				{
					return i;
				}
			}
			return -1;
		}

		public int FindIndexFromEnd(T value)
		{
			for (int i = Total - 1; i >= 0; i--)
			{
				if (data[i].Equals(value))
				{
					return i;
				}
			}
			return -1;
		}

		public bool Insert(T value, int index)
		{
			if (index > Total - 1 || index < 0)
			{
				return false;
			}
			if (Total < data.Length)
			{
				for (int i = Total - 1; i >= index; i--)
				{
					data[i + 1] = data[i];
				}
				data[index] = value;
				Total++;
				return true;
			}
			else
			{
				int newCap = (int)(data.Length * 1.5);

				T[] newData = new T[newCap];
				for (int i = 0; i < index; i++)
				{
					newData[i] = data[i];
				}
				for (int i = index; i < data.Length; i++)
				{
					newData[i + 1] = data[i];
				}
				newData[index] = value;
				data = newData;
				Total++;
				return true;
			}
		}

		public void PushBack(T value)
		{
			if (Total < data.Length)
				data[Total] = value;
			else
			{
				int newCap = (int)(data.Length * 1.5);
				T[] newData = new T[newCap];
				for (int i = 0; i < data.Length; i++)
				{
					newData[i] = data[i];
				}
				newData[Total] = value;
				data = newData;
			}
			Total++;
		}

		public void PushFront(T value)
		{
			if (Total < data.Length)
			{
				for (int i = Total - 1; i >= 0; i--)
				{
					data[i + 1] = data[i];
				}
				data[0] = value;
				Total++;
			}
			else
			{
				int newCap = (int)(data.Length * 1.5);
				T[] newData = new T[newCap];
				for (int i = 0; i < data.Length; i++)
					newData[i + 1] = data[i];
				newData[0] = value;
				data = newData;
				Total++;
			}
		}

		public void Reverse()
		{
			for (int i = 0; i < Total/2; i++)
			{
				T t = data[i];
				data[i] = data[Total - i - 1];
				data[Total - i - 1] = t;
			}
		}
	
		public void Print()
		{
			Console.Write("[");
			for (int i = 0; i < Total; i++)
			{
				Console.Write(data[i].ToString());
				if (i < Total - 1)
					Console.Write(", ");
			}
			Console.WriteLine("]");
		}

		public bool Remove(int index)
		{
			if (index < 0 || index >= Total)
			{
				return false;
			}
			else
			{
				for (int i = index; i < Total - 1; i++)
				{
					data[i] = data[i + 1];
				}
				Total--;
				return true;
			}
		}

		public void Clear()
		{
			T[] newData = new T[10];
			data = newData;
			
			Total = 0;
		}

	}
}
