#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>

int is_primitive(int num1, int num2)
{
	int max_num, min_num, t;

	if (num1 <= 0 || num2 <= 0)
	{
		return 0;
	}

	max_num = num1; if (max_num < num2) max_num = num2;
	min_num = num1; if (min_num > num2) min_num = num2;

	while (min_num > 1)
	{
		t = max_num % min_num;
		if (t == 0)
		{
			break;
		}
		max_num = min_num;
		min_num = t;
	}

	if (min_num > 1)
	{
		return 0;
	}

	return 1;
}

int main()
{
	int numbers[3] = {-1, -1, -1};

	for (;;)
	{
		printf("Please enter three natural numbers( for example, enter 4, 5, 6):");
		scanf("%d %d %d", &numbers[0], &numbers[1], &numbers[2]);

		if (numbers[0] > 0 && numbers[1] > 0 && numbers[2] > 0)
		{
			break;
		}
		printf("You entered incorrect numbers\n");
	}

	if (numbers[0] * numbers[0] + numbers[1] * numbers[1] == numbers[2] * numbers[2])
	{
		printf("The numbers are Pythagorean triple.\n");

		if (is_primitive(numbers[0], numbers[1]) && 
			is_primitive(numbers[1], numbers[2]) && 
			is_primitive(numbers[2], numbers[0]))
		{
			printf("Pythagorean triple are primitive.\n");
		}
		else
		{
			printf("Pythagorean triple are not primitive.\n");
		}
	}
	else
	{
		printf("The numbers are not Pythagorean triple.\n");
	}


	return 0;
}
