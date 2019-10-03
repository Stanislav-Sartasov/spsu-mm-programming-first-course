#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <locale.h>

int gcd(int x, int y)
{
	if (y == 0)
	{
		return x;
	}
	else
	{
		int m = x % y;
		return gcd(y, m);
	}
}

int check(int x, int y, int z)
{
	if (x * x - (y * y + z * z) == 0) return 1;
	if (y * y - (z * z + x * x) == 0) return 1;
	if (z * z - (x * x + y * y) == 0) return 1;
	return 0;

}


int main()
{	
	
	setlocale(LC_ALL, "Rus");
	printf("Программа получает на вход тройку чисел и определяет, является ли эта тройка Пифагоровой\n");
	printf("Введите последовательно 3 числа\n");
	int x = 0; int z = 0; int y = 0; int i = 0; char eol = 0; int n = 1;
	
	while (n == 1)
	{
		int xscan = scanf("%d", &x);
		int yscan = scanf("%d", &y);
		int zscan = scanf("%d%c", &z , &eol);
		if (xscan && yscan && zscan && eol == '\n')
		{
			if ((x <= 0) || (y <= 0) || (z<=0))
			{
				printf("Вы ввели не положительные числа - попробуйте еще раз\n");
			}
			else 
			{
				n = 0;
				if (check(x, y, z))
				{
					printf("Да, это Пифагорова тройка\n");
					if ((gcd(x, y) == 1) && (gcd(y, z) == 1))
					{
						printf("Да, это примитивная тройка");
					}

				}
				else
				{
					printf("Нет это не Пифагорова тройка");
				}
				
			}

		}
		else
		{
			printf("Вы ввели строку - попробуйте еще раз\n");
			scanf("%*[^\n]");
		}
	}
	
}