#include <stdio.h>
#include <stdlib.h>
#include <math.h>

#define NUM			5000
#define BASE_NUM	16

int main()
{
	int i, j, array_size;
	int* digit;

	array_size = (int)(NUM * log10(3.0) / log10(16.0)) + 1;
	digit = (int*)calloc(array_size, sizeof(int));
	if (!digit)
	{
		printf("Can't alloc the digit's table.\n");
		return -1;
	}

	digit[0] = 1;

	for (i = 0 ; i < NUM ; i++)
	{
		for (j = 0 ; j < array_size ; j++)
		{
			digit[j] = digit[j] * 3;
		}

		for (j = 0 ; j < array_size - 1 ; j++)
		{
			digit[j + 1] = digit[j + 1] + digit[j] / BASE_NUM;
			digit[j] = digit[j] % BASE_NUM;
		}
	}

	printf("the sum for 3 ^ %d is \n", NUM);
	for (i = array_size - 1 ; i >= 0 ; i = i - 1)
	{
		if (digit[i] > 0)
		{
			printf("%X", digit[i]);
		}
	}
	printf("\n");

	free(digit);
	
	return 0;
}