#include <stdio.h>
#include <stdlib.h>

int main()
{
	int* digits = (int*)malloc(2400 * sizeof(int));
	for (int i = 1; i < 2400; i++)
		digits[i] = 0;
	digits[0] = 1;

	for (int i = 0; i < 5000; i++)
	{
		for (int j = 0; j < 2400; j++)
		{
			digits[j] *= 3;
		}
		for (int pos = 0; pos < 2400; pos++)
		{
			if (digits[pos] >= 16)
			{
				digits[pos + 1] += (digits[pos] / 16);
				digits[pos] %= 16;
			}
		}
	}

	int pos = 2399;
	while (digits[pos] == 0)
		pos--;
	for (pos; pos > -1; pos--)
	{
		printf("%X", digits[pos]);
	}

	free(digits);

	return 0;
}
