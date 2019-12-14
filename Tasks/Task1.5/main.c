#include "stdio.h"
#include "math.h"

int is_sqrt(int number)
{
	if (sqrt(number) == (int) sqrt(number))
		return 0;
	return 1;
}

void get_fraction(int number)
{
	int p = 0;
	int q = 1;
	int int_part = sqrt(number);
	int period = 0;

	int curr_elem = int_part;
	printf("%d ", curr_elem);

	while (curr_elem != 2 * int_part)
	{
		p = curr_elem * q - p;
		q = (number - p * p) / q;
		curr_elem = (int_part + p) / q;
		period++;
		printf("%d ", curr_elem);
	}
	printf("\nPeriod length: %d\n", period);
}

void consume_input()
{
	int symb;
	while ((symb = getchar()) != '\n' && symb != EOF);
}

void input(int* number)
{
	while (1)
	{
		printf("Input number: ");
		if (scanf("%d", number) != 1)
		{
			printf("Incorrect input\n\n");
			consume_input();
			continue;
		}

		if (*number < 0)
		{
			printf("Number is negative\n\n");
			consume_input();
			continue;
		}

		if (!is_sqrt(*number))
		{
			printf("Number is a full square\n\n");
			consume_input();
			continue;
		}
		break;
	}
}

int main()
{
	int number;

	input(&number);

	get_fraction(number);

	return 0;
}
