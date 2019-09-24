#include <stdio.h>
#include <math.h>
#include <malloc.h>
#include "inputing.h"

int* set_len(int* array, int* size, int new_size)
{
	int* auxiliary_array = (int*)malloc(new_size * sizeof(int));
	while (auxiliary_array == '\0')
		auxiliary_array = (int*)malloc(new_size * sizeof(int));
	if (*size)
	{
		for (int i = 0; (i < *size) && (i < new_size); i++)
			auxiliary_array[i] = array[i];
		free(array);
	}
	*size = new_size;
	return auxiliary_array;
}

int main()
{
	printf("This program for the entered number determines the continued fraction of its square root\n\n");
	int n;
	do
	{
		in_int("Enter number: ", &n);
		if (n <= 0)
			printf("positive value expected\n");
	}
	while (n <= 0);
	printf("\n");

	int whole_part = trunc(sqrt(n));

	printf("%d", whole_part);

	if (trunc(whole_part) * trunc(whole_part) == n)
	{
		printf("\n");
		return 0;
	}

	int t_fraction = 1;
	int b_fraction = 0; // top fraction, bottom fraction

	int* c_fraction = 0;
	int c_fraction_len = 0;

	for (int i = 1;; i++)
	{
		int t = whole_part - b_fraction;	//intermediate variable
		b_fraction = t + whole_part;
		t_fraction = (n - t * t) / t_fraction;
		c_fraction = set_len(c_fraction, &c_fraction_len, i);
		c_fraction[i - 1] = b_fraction / t_fraction;
		printf(", %d", c_fraction[i - 1]);
		b_fraction = b_fraction % t_fraction;
		if (c_fraction[i - 1] == whole_part * 2)
		{
			char flag = 1;
			for (int j = 0; j < c_fraction_len / 2; j++)
				if (c_fraction[j] != c_fraction[c_fraction_len - 2 - j])
				{
					flag = 0;
					break;
				}
			if (flag)
			{
				printf("\n\n");
				return 0;
			}
		}
	}
}
