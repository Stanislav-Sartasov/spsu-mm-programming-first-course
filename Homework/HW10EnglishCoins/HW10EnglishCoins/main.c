#define _CRT_SECURE_NO_WARNINGS

#include <stdio.h>
#include <malloc.h>

int main()
{
	int numberOfPences;

	printf("Enter pences quantity: ");

	scanf("%d", &numberOfPences);

	int i, j;
	int coins[8] = { 1, 2, 5, 10, 20, 50, 100, 200 };
	unsigned long long* tempArr;

	tempArr = (unsigned long long*)calloc((numberOfPences + 1), sizeof(unsigned long long));
	tempArr[0] = 1;

	for (i = 0; i <= 7; i++)
	{
		for (j = coins[i]; j < numberOfPences + 1; j++)
		{
			tempArr[j] = tempArr[j] + tempArr[j - coins[i]];
		}
	}

	printf("\nNumber of variants: %lld", tempArr[numberOfPences]);
	free(tempArr);
	return(0);
}