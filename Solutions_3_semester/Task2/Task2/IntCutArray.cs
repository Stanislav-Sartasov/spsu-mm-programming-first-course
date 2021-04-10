using System;
using System.Collections.Generic;
using System.Text;

namespace Task2
{
	[Serializable]
	class IntCutArray
	{
		public IntCutArray(int[] arr, int shift, int vertShift)
		{
			Shift = shift;
			VertShift = vertShift;
			content = arr;
		}
		public IntCutArray(int size, int shift, int vertShift)
		{
			Shift = shift;
			VertShift = vertShift;
			content = new int[size];
		}

		int[] content;
		public int Shift { get; }
		public int VertShift { get; }
		public int Count
		{
			get
			{
				return content.Length;
			}
		}
		public int Size
		{
			get
			{
				return Count + Shift;
			}
		}

		public int this[int ind]
		{
			get
			{
				int lockalInd = ind - Shift;
				if (lockalInd < 0 || lockalInd >= Count)
					throw new IndexOutOfRangeException();
				return content[lockalInd];
			}
			set
			{
				int lockalInd = ind - Shift;
				if (lockalInd < 0 || lockalInd >= Count)
					throw new IndexOutOfRangeException();
				content[lockalInd] = value;
			}
		}
		public bool IsInArray(int ind)
		{
			int lockalInd = ind - Shift;
			return lockalInd >= 0 && lockalInd < Count;
		}
	}
}
