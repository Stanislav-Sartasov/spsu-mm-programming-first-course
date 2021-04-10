using System;
using System.Collections.Generic;
using System.Text;

namespace Task2
{
	class Graph
	{
		public Graph(IntCutArray[] content, int size, int realSize)
		{
			ElemCount = size;
			Content = content;
			RealGraphSize = realSize;
		}

		public int RealGraphSize { get; }
		public int ElemCount { get; }
		public IntCutArray[] Content { get; }
	}
}
