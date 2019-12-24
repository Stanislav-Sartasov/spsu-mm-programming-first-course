#define _CRT_SECURE_NO_WARNINGS

#include <stdio.h>
#include <malloc.h>

int main()
{
	int numberOfPences;

	printf("Enter pences quantity: ");

	scanf("%d", &numberOfPences);

	int i, j;
	int coins[8] = {1, 2, 5, 10, 20, 50, 100, 200};
	int tempArr[8] = {1, 0, 0, 0, 0, 0, 0, 0};

	for (i = 0; i < 8; i++)
	{
		for (j = coins[i]; j < numberOfPences+1; j++)
		{
			tempArr[j] = tempArr[j] + tempArr[j - coins[i]];
		}
	}

	printf("Number of variants: %d", tempArr[numberOfPences]);

	return(0);
}