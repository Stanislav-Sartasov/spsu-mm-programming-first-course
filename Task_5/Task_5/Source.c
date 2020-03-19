#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>


int main()
{
	int n = 1; char eol = ' ';
	int x = 0;
	int total = 0; 
	int num = 0;
	int remainder = 1;
	int count = 0;

	printf("The program decomposes the number into a chain fraction and finds its period\n");
	printf("Input a positive number:\n");

	while (n == 1)
	{
		int xscan = scanf("%d%c", &x, &eol);
		total = (int)sqrt(x);
		if (xscan && eol == '\n')
		{
			if (x <= 0)
			{
				printf("You entered non-positive number - try again\n");
			}
			else if (total - sqrt(x) == 0)
			{
				printf("A number is the square of an integer");
			}
			else
			{
				n = 0;
				printf("[%d", total);

				while (total != 2 * (int)sqrt(x))
				{

					num = num - total * remainder;
					remainder = (int)((x - pow(num , 2)) / remainder);
					num = -num;
					total = (int)((sqrt(x) + num) / remainder);
					count++;
					
					if (total == 2 * (int)sqrt(x))
					{
						printf(", %d] period is %d", total, count);
					}
					else
					{
						printf(", %d", total);
					}
				}
			}
		}
		else
		{
			printf("You entered a string-try again\n");
			scanf("%*[^\n]");
		}
	}
}

