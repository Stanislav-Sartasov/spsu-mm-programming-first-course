#include "manager.h"

int main()
{
	init();
	int amount = 2;
	int* number;
	number = (int*)my_malloc(sizeof(int) * amount);
	if (number == NULL)
	{
		printf("Unable to allocate memory. NULL pointer returned");
		return 0;
	}
	number[0] = 1;
	number[1] = 2;
	printf("%d %d\n", number[0], number[1]);
	my_free(number);
	amount--;
	int* number2;
	number = my_realloc(NULL, sizeof(int) * amount);
	if (number == NULL)
	{
		printf("Unable to allocate memory. NULL pointer returned");
		return 0;
	}
	number2 = (int*)my_malloc(sizeof(int) * amount);
	if (number2 == NULL)
	{
		printf("Unable to allocate memory. NULL pointer returned");
		return 0;
	}
	number[0] = 1;
	number2[0] = 10;
	printf("%d %d\n", number[0], number2[0]);
	amount++;
	number = my_realloc(number, sizeof(int) * amount);
	if (number == NULL)
	{
		printf("Unable to allocate memory. NULL pointer returned\n");
		return 0;
	}
	number[1] = 2;
	printf("%d %d %d\n", number[0], number[1], number2[0]);
	amount++;
	number = my_realloc(number, sizeof(int) * amount);
	my_free(number);
	printf("%d\n", number2[0]);
	my_free(number2);
	initstop();
	return 0;
}
