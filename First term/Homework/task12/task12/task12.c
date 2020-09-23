#include <stdio.h>
#include <stdlib.h>
#include <math.h>

int main()
{
	int count_of_digits = (5000 * (log(3) / log(16)) + 1);
	int* digits = (int*)malloc(count_of_digits * sizeof(int));
	memset(digits, 0, count_of_digits * sizeof(int));
	digits[0] = 1;

	for (int i = 0; i < 5000; i++)
	{
		for (int j = 0; j < count_of_digits; j++)
		{
			digits[j] *= 3;
		}
		for (int pos = 0; pos < count_of_digits; pos++)
		{
			if (digits[pos] >= 16)
			{
				digits[pos + 1] += (digits[pos] / 16);
				digits[pos] %= 16;
			}
		}
	}

	int pos = count_of_digits - 1;

	for (pos; pos > -1; pos--)
	{
		printf("%X", digits[pos]);
	}

	free(digits);

	return 0;
}
