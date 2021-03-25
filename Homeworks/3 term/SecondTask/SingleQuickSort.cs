using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondTask
{
	public static class SingleQuickSort
	{
		public static void QuickSort(List<int> arr, int min, int max)
		{
			int i = min;
			int j = max;
			int r = arr[(i + j) / 2];

			while (i <= j)
			{
				while (arr[i] < r)
				{
					i++;
				}
				while (arr[j] > r)
				{
					j--;
				}

				if (i <= j)
				{
					if (arr[i] > arr[j])
					{
						var t = arr[i];
						arr[i] = arr[j];
						arr[j] = t;
					}

					i++;
					j--;
				}
			}

			if (min < j)
			{
				QuickSort(arr, min, j);
			}
			if (max > i)
			{
				QuickSort(arr, i, max);
			}
		}
	}
}
