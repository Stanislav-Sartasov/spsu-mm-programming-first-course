#include "stdio.h"
#include "math.h"

int is_sqrt(int number)
{
	for (int i = 1; i*i <= number; i++)
	{
		if (i * i == number)
			return 0;
	}
	return 1;
}

int get_fraction(int number)
{
	float frac_elem = sqrt(number);
	int int_part = frac_elem;

	printf("%d ", int_part);

	float next_elem = frac_elem;
	int next_int_part = int_part;

	int period = 0;
	while (2 * int_part != next_int_part)
	{
		next_elem = 1 / (next_elem - next_int_part);
		next_int_part = next_elem;

		printf("%d ", next_int_part);
		period++;
	}

	printf("\nPeriod length: %d\n", period);
}

int main()
{
	int number;

	printf("Input number: ");
	scanf("%d", &number);

	if (!is_sqrt(number))
	{
		printf("Incorrect number\n");
		return 0;
	}

	get_fraction(number);

	return 0;
}