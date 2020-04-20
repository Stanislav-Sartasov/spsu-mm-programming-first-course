#include <stdio.h>
#include <string.h>
#include <math.h>
#include <stdlib.h>


void binOutput(int n, long long num)
{
	int i = 0;
	int* block;
	block = malloc(n * sizeof(int));

	while (num != 0)
	{
		block[i] = num % 2;
		num /= 2;
		i++;
	}

	for (i = n - 1; i >= 0; i--)
	{
		printf("%d", block[i]);
	}

	free(block);

}

int main()
{
	char name[] = "Michael";
	char surname[] = "Frolchenko";
	char patronymic[] = "Viktorovich";
	int multiply = strlen(name) * strlen(surname) * strlen(patronymic);
	printf("The product of numbers: %d\n", multiply);
	long long x = powl(2, 32) - multiply;
	printf("Negative 32bit integer: ");
	binOutput(32, x);

	float y = multiply;
	int f = *((int*)&y);
	printf("\nPositive floating point unit precision: 0");
	binOutput(31, f);

	double z = multiply;
	long long d = *((long long*)&z);
	printf("\nNegative double precision floating point: 1");
	binOutput(63, d);

}