#include <stdio.h>
#include <stdlib.h>
#include <math.h>

int main()
{
	int n = (1 + (log(3) / log(256)) * 5000);
	unsigned char* block = (unsigned char*)calloc(n, sizeof(char));
	block[0] = 3;
	int over;
	int over_last = 0;
	for (int i = 0; i < 4999; i++)
	{
		for (int j = 0; j < n; j++)
		{
			if ((int)block[j] * 3 + over_last > 255)
			{
				over = block[j] * 3 + over_last;
				block[j] = (unsigned char)over % 256;
				over_last = over / 256;
			}
			else
			{
				block[j] = block[j] * 3 + over_last;
				over_last = 0;
			}
		}
	}
	printf("3^5000 = ");
	for (int i = (n - 1); i >= 0; i--)
	{
		printf("%x", block[i] >> 4 & 15);
		printf("%x", block[i] & 15);

	}
	free(block);
	return 0;
}