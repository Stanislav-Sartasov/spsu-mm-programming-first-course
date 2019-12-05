#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>

int main()
{
	int period = 0, res, p = 0, q = 1, x;
	double n;

	printf("Enter the number \n");
	do
	{
		res = scanf("%lf", &n);
		if (n <= 0) res = 0;
		if (sqrt(n) == (int)sqrt(n)) res = 0;
		while (getchar() != '\n');
		if (res == 1) break;
		else printf("%s", "Invalid numbers entered, try again\n");
	} while (res != 1);

	x = sqrt(n);
	
	for (;;)
	{
		period++;
		p = x * q - p;
		q = (n - p * p) / q;
		x = (sqrt(n) + p) / q;
		printf("%d", x);
		printf("%c", ' ');
		if (q == 1) break;
	}
	printf("\n%s", "Period is ");
	printf("%d\n", period);

	system("pause");
	return 0;
}
