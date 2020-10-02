#include <stdio.h>
#include <stdlib.h>

int main()
{
	char* block = (char*)calloc(1, sizeof(char));
	block[0] = 3;
	int count = 1;
	for (int i = 0; i < 4999; i++)
	{
		for (int j = 0; j < count; j++)
		{
			block[j] *= 3;
		}
		if (block[count - 1] > 15)
		{
			block = realloc(block, (count + 1) * sizeof(char));
			block[count] = 0;
			count++;
		}
		for (int j = 0; j < count - 1; j++)
		{
			if (block[j] > 15)
			{
				block[j + 1] += (char)(block[j] / 16);
				block[j] %= 16;
			}
		}
	}
	printf("3^5000 = ");
	for (int i = (count - 1); i >= 0; i--)
	{
		printf("%x", block[i]);
	}
	free(block);
	return 0;
}