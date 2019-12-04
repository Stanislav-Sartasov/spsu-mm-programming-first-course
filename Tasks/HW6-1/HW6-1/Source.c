#include <stdio.h>
#include <math.h>
#include <malloc.h>

int main()
{
	int* array;
	int counter_a, counter_b, digit_counter;

	digit_counter = (int)(5000 * log(3) / log(16));
	array = (int*)malloc((digit_counter + 1) * sizeof(int));

	for (counter_a = 0; counter_a <= digit_counter; counter_a++)
	{
		array[counter_a] = 0;
	}

	array[0] = 1;

	for (counter_a = 0; counter_a <= 4999; counter_a++)
	{
		for (counter_b = 0; counter_b <= digit_counter; counter_b++)
		{
			array[counter_b] = array[counter_b] * 3;
		}

		for (counter_b = 0; counter_b <= digit_counter - 1; counter_b++)
		{
			if (array[counter_b] >= 16)
			{
				array[counter_b + 1] = array[counter_b + 1] + array[counter_b] / 16;
				array[counter_b] = array[counter_b] % 16;
			}
		}
	}

	printf("Number 3^5000 in hexadecimal system :\n");

	for (counter_a = digit_counter; counter_a >= 0; counter_a--)
	{
		printf("%X", array[counter_a]);
	}

	printf("\n");
	free(array);
	return(0);
}