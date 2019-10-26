#include <stdio.h>
#include <string.h>

int main()
{
	int list[335]; // 3^5000 < 16^2000, в блоке будем записывать остаток от деления на 16^6, т.е. шестизначное шестнадцатиричное число. 2000/6 ~ 335
	int i = 5000;
	int j = 334;
	int rest;
	int zero_amount = -1;
	int q = 0;
	int unit = 16777216; // = 16^6
	memset(list, 0, 335 * sizeof(int));
	list[334] = 1;
	printf("3^5000 in a hexadecimal number system has the form:\n");
	while (i > 0)
	{
		rest = 0;
		q = 0;
		j = 334;
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

	int l = 0;
	while (list[l] == 0)
	{
		zero_amount = l;
		l += 1;
	}

	for (int k = zero_amount + 1; k < 335; k++)
	{
		printf("%06x", list[k]);  // Каждый "блок" представляем в шестнадцатиричной системе с фиксированным числом знаков, т.к. хотим "склеить" блоки
	}

	return 0;
}