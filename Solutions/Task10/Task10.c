#include <stdio.h>
#include "inputing.h"

int find_amount(int number, int money_biggest_type, int* money_types)
{
	if (money_biggest_type == 0)
		return 1;
	if (!number)
		return 1;
	if (money_biggest_type == 1)
		return number / 2 + 1;
	int count = 0;
	for (int type_count = 1; money_types[money_biggest_type] * type_count <= number; type_count++)
		if (money_types[money_biggest_type] * type_count == number)
			count++;
		else
			for (int j = 1; j < money_biggest_type; j++)
				count += find_amount(number - money_types[money_biggest_type] * type_count, j, money_types);
	return count;
}

int main()
{
	int money;
	printf("This program for the entered amount of money in pensions\nis determined by how many ways you can collect this amount using standard English coins\n\n");
	do
	{
		in_int("Enter amount of money: ", &money);
		if (money <= 0)
			printf("Please enter a natural value\n");
	} 
	while (money <= 0);

	int money_types[8] = { 1, 2, 5, 10, 20, 50, 100, 200 };

	long long count = 0;

	for (int i = 1; (money / money_types[i] > 0) && (i < 8); i++)
	{
		count += find_amount(money, i, money_types);
	}

	printf("\n%lld\n", count);

	return 0;
}
