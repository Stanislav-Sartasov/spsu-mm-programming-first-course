#include <stdio.h>
#include <malloc.h>
#include <string.h>

void printf_binary_integer(char* prefix, unsigned long long num, int c)
{
	int i, *ar;

	if (c > 0)
	{
		ar = (int*)malloc(sizeof(int*) * c);
		if (ar)
		{
			for (i = 0; i < c; i++)
			{
				ar[i] = num % 2;
				num = num / 2;
			}
			printf("%s is ", prefix);
			for (i = c - 1; i >= 0; i--)
			{
				printf("%d", ar[i]);
			}
			printf("\n");

			free(ar);
		}
	}
}

int main()
{
	int product;
	float float_product;
	double double_product;
	const char first_name[] = "Yunus";
	const char middle_name[] = "Emre";
	const char last_name[] = "Cakici";

	product = strlen(first_name) * strlen(middle_name) * strlen(last_name);
	printf("The product is %d\n", product);

	// print negative 32bit integer
	printf_binary_integer("negative 32bit integer", (unsigned long long) - product, 32);

	// print positive floating-point number
	float_product = (float)product;
	printf_binary_integer("positive floating-point number of unit precision according to the IEEE 754 standard", *((int*)&float_product), 32);

	// print negative double-precison floating-point number according to the IEEE 754 standard,
	double_product = (double)-product;
	printf_binary_integer("negative double-precison floating-point number according to the IEEE 754 standard", *((long long*)&double_product), 64);
}
