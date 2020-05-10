#define _CRT_SECURE_NO_WARNINGS

#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>

void ins(long* p)
{
	char sres[15]; // inputed string
	long a; // number
	short check = 0;
	while (check == 0)
	{
		short fnum = 0;
		for (int k = 0; k < 1; k++)
		{
			scanf("%s", &sres);
			a = 0;
			for (int l = 0; sres[l] != '\0'; l++)
			{
				if (!((sres[l] >= '0') && (sres[l] <= '9')))
					check = 1;
				if (fnum == 0)
					a = a * 10 + (sres[l] - '0');
				if (sres[l] == '\0')
					fnum = 0;
			}
		}
		fnum = 1;
		for (int k = 0; k < 1; k++)
		{
			if (a <= 0)
				check = 1;
		}
		if (check == 0)
			break;
		else
		{
			printf("Incorrect input, please try again.\n");
			check = 0;
		}
	}
	*p = a;
}

int main()
{
	printf("The program calculates the number of ways to allocate the amount of money in exchange currency.\n");
	printf("Insert number: ");
	long n;

	ins(&n);

	int val[] = {1, 2, 5, 10, 20, 50, 100, 200};
	long* num = malloc(sizeof(long) * ((n + 1) * 8));
	num[0] = 1;

	for (long i = 0; i <= n; i++)
	{
		for (int j = 0; j <= 7; j++)
		{
			if ((i != 0) || (j != 0))
				num[i * 8 + j] = 0;
			if (i - val[j] >= 0)
			{
				for (int k = 0; k <= j; k++)
					num[i * 8 + j] = num[i * 8 + j] + num[(i - val[j]) * 8 + k];
			}
		}
	}

	long result = 0;
	for (long k = 0; k <= 7; k++)
	{
		result = result + num[n * 8 + k];
	}

	printf("The answer is %ld.\n", result);
	free(num);
	return 0;
}