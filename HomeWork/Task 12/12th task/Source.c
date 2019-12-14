#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

void myPow(int *mas, int n, int pow, int quantity)
{
	int i, j;
	for (i = 0; i < pow; i++)
	{
		for (j = 0; j < quantity; j++) mas[j] *= n;
		for (j = 0; j < quantity; j++)
			if (mas[j] >= 16)
			{
				mas[j + 1] += mas[j] / 16;
				mas[j] %= 16;
			}
	}

}

int main()
{
	int n, i;
	n = (5000 * (log(3) / log(16)) + 1);
	int *mas = (int*)malloc(n * sizeof(int));
	mas[0] = 1;
	for (i = 1; i < n; i++) mas[i] = 0;

	myPow(mas, 3, 5000, n);

	for (i = n - 1; i >= 0; i--) printf("%x", mas[i]);
	printf("\n");
	free(mas);
	system("pause");
	return 0;
}
