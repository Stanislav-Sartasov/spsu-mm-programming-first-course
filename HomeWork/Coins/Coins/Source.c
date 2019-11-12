#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>

int coins[8] = { 1,2,5,10,20,50,100,200 };

int answer(int n)
{
	int *mas = (int*)malloc((n + 1) * sizeof(int));
	int i;
	for (i = 0; i < n + 1; i++) mas[i] = 1;

	for (i = 1; i < 8; i++)
	{
		for (int j = coins[i]; j <= n; j++)
		{
			mas[j] += mas[j - coins[i]];
		}
	}
	int answ = mas[n];
	free(mas);
	return answ;
}

int main()
{
	int n, answ, res;
	//scanf("%d", &n);
	printf("%s", "Enter the amount of money ");
	do
	{
		res = scanf("%d", &n);
		if (n <= 0) res = 0;
		while (getchar() != '\n');
		if (res == 1) break;
		else printf("%s", "Invalid number entered, try again\n");
	} while (res != 1);

	answ = answer(n);
	printf("%d\n", answ);
	
	system("pause");
	return 0;
}
