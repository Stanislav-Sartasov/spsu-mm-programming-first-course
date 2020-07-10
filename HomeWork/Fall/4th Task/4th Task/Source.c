#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include<stdlib.h>
#include <math.h>

int isSimple(int x)
{
	for (long long i = 2; i * i <= x; i++)
		if ((x % i) == 0) return 0;
	return 1;
}

int main()
{
	int x;
	for (int i = 2; i <= 31; i++)
	{
		x = pow(2, i) - 1;
		if (isSimple(x) == 1) printf("%d\n", x);
	}

	system("pause");
	return 0;
}
