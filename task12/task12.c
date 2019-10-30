#include <stdio.h>
#include <string.h>
#include <math.h>
#include <stdlib.h>
#include <malloc.h>

int digit_amount(int base, int exp, int a) // Кол-во цифр в числе (base ^ exp) в системе счисления a. 
{
	return ceil(exp * (log(base) / log(a))); // Минимальное решение нер-ва (base ^ exp) < (a ^ x) для целого x. 

}

int main()
{
	int* list;
	int i = 5000;
	int j = 0;
	int l;
	int rest;
	int q = 0;
	int unit = 16777216; // = 16^6
	
	j = ceil(digit_amount(3, 5000, 16) / 6);
	list = (int*)malloc((j + 1) * sizeof(int));
	if (list == NULL)
	{
		printf("Memory not allocated.\n");
		exit(0);
	}
	else
	{
		memset(list, 0, (j + 1) * sizeof(int));
		list[j] = 1;
		printf("3^5000 in a hexadecimal number system has the form:\n");
		l = j;
		while (i > 0)
		{
			rest = 0;
			q = 0;
			j = l;
			while (j >= 0)
			{
				list[j] = list[j] * 3 + q;

				rest = list[j] % unit;
				q = (list[j] - rest) / unit;
				list[j] = rest;

				j -= 1;
			}

			i -= 1;
		}

		for (int k = 0; k <= l; k++)
		{
			if (k != 0)	printf("%06x", list[k]);  // Каждый "блок" представляем в шестнадцатиричной системе с фиксированным числом знаков, т.к. хотим "склеить" блоки
			else printf("%x", list[k]);
		}
	}
	free(list);
	return 0;
}