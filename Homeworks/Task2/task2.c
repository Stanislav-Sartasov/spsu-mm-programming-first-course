#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>

int get_largest_common_factor(int num1, int num2)
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

	return min_num;
}

int main()
{
	int numbers[3] = { -1, -1, -1 };
	char strs[3][1000];

	for (;;)
	{
		printf("Please enter three natural numbers( for example, enter 4, 5, 6):");
		scanf("%s %s %s", strs[0], strs[1], strs[2]);

		numbers[0] = atoi(strs[0]);
		numbers[1] = atoi(strs[1]);
		numbers[2] = atoi(strs[2]);

		if (numbers[0] > 0 && numbers[1] > 0 && numbers[2] > 0)
		{
			break;
		}
		printf("You entered incorrect numbers\n");
	}

	if (numbers[0] * numbers[0] + numbers[1] * numbers[1] == numbers[2] * numbers[2] ||
		numbers[1] * numbers[1] + numbers[2] * numbers[2] == numbers[0] * numbers[0] ||
		numbers[2] * numbers[2] + numbers[0] * numbers[0] == numbers[1] * numbers[1])
	{
		printf("The numbers are Pythagorean triple.\n");

		if (1 == get_largest_common_factor(numbers[0], numbers[1]) &&
			1 == get_largest_common_factor(numbers[1], numbers[2]) &&
			1 == get_largest_common_factor(numbers[2], numbers[0]))
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
