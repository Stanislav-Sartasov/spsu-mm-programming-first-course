#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>
#include <math.h>
#include <string.h>

int main()
{
	int b = 3;
	int n = 5000;
	int size = (log(b) / log(16)) * n + 1;
	int *a = (int*) malloc(size * sizeof(int));
	memset(a, 0, size * sizeof(int));
	a[0] = 1;

	printf("The program calculates the value 3^5000 and displays it in the hexadecimal number system.\n");
	printf("3^5000 = ");

	for (int i = 0; i <= n - 1; i++)
	{
		int j = 0;
		int ten = 0;  // last ten
		while (j < size)
		{
			a[j] = a[j] * b + ten;
			div_t r = div(a[j], 16);
			ten = r.quot;
			a[j] = r.rem;
			j++;
		}
	}

	for (int i = (size - 1); i >= 0; i--)
	{
		if ((a[i] >= 0) && (a[i] <= 9))
			printf("%d", a[i]);
		else
			printf("%c", (char)(a[i]+55));
	}

	free(a);
	return 0;
} 


