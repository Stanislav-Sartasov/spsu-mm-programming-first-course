#include <stdio.h>
#include <math.h>
main()
{
	int n, k, r, merc;
	for (n = 2; n < 32; n++)
	{
		r = 0;
		k = 0;

		merc = 1;
		for (int i = 2; i < (sqrt(n) + 1); i++)	/* Проверяем только простые показатели, иначе разность n-ых степеней имеет вид (2^x-1)*(...) */
		{
			if (n % i == 0) k = 1;

		}
		if ((k == 0) || (n == 2))
		{
			for (int l = 1; l < n + 1; l++)
			{
				merc = merc * 2;
			}
			merc = merc - 1;
			for (int j = 2; j < (sqrt(merc) + 1); j++)
			{
				if ((merc % j) == 0) r = 1;
			}
			if (r == 0) printf("%d\n", merc);
		}

	}
}
