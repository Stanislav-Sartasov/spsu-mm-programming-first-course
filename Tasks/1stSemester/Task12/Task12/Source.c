#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>


int check()
{
	int c, flag = 1;
	while ((c = getchar()) != '\n' && c != EOF)
		if (c != '\n' && c != ' ' && c != EOF)
			flag = 0;
	return flag;
}

void input(int* x)
{
	int result = scanf("%d", x);
	int flag = check();

	while ((flag == 0) || (result - 1) || (*x < 1))
	{
		printf("Invalid Input.Try Again.\n");
		result = scanf("%d", x);
		flag = check();
	}

}

void power(int* arr, int number, int pow)
{
	for (int i = 0; i < pow; i++)
	{
		for (int j = 0; j < pow; j++)
			arr[j] *= number;

		for (int j = 0; j < pow - 1; j++)
			if (arr[j] >= 16)
			{
				arr[j + 1] += arr[j] / 16;
				arr[j] %= 16;
			}
	}
}


int main()
{
	int n = 5000;

	/*
	printf("Enter the required degree of the triple:\n");
	input(&n);
	*/

	int* arr = (int*)malloc(n * sizeof(int));

	for (int i = 0; i < n; i++)
		arr[i] = 0;
	arr[0] = 1;

	power(arr, 3, n);

	int firstZero = -1;

	for (int i = n - 1; i >= 0; i--)
		if (arr[i] != 0)
		{
			firstZero = i + 1;
			break;
		}

	for (int i = firstZero - 1; i >= 0; i--)
		printf("%X", arr[i]);

	free(arr);
	return 0;
}