#include <stdio.h>
#include <stdlib.h>

int main()
{
	int n = 5000;
	int a[2500];
	a[0] = 1;

	printf("The program calculates the value 3^5000 and displays it in the hexadecimal number system.\n");
	printf("3^5000 = ");
	for (int i = 1; i <= 2499; i++)
	{
		a[i] = -1;
	}

	for (int i = 0; i <= n - 1; i++)
	{
		short f = 0; //units discharge flag
		int j = 0;
		while (a[j] > -1)
		{
			if ((f == 0) || (j == 0))
				a[j] = a[j] * 3;
			if (a[j + 1] == -1)
				f = 1;
			else
				f = 0;
			if ((a[j + 1] == -1) && (a[j] != 0))
				a[j+1]++;
			if (j > 0)
				if (a[j-1] >= 16)
				{
					div_t r = div(a[j-1], 16);
					a[j] = a[j] + r.quot;
					a[j-1] = r.rem;
				}
			if ((a[j + 1] == -1) && (a[j] == 0))
				a[j]--;
			j++;
		}
	}

	short f = 0;//first digit flag
	for (int i = 2499; i >= 0; i--)
	{
		if ((a[i] > 0) && (f == 0))
			f = 1;
		if (f == 1)
		{
			if ((a[i] >= 0) && (a[i] <= 9))
				printf("%d", a[i]);
			else
				printf("%c", (char)(a[i]+55));
		}
	}
	return 0;
} 