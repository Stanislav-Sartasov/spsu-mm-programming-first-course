#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>



int main()
{

	int x = 0; int n = 1; char eol = ' ';
	float numbersq = 0;
	int total = 0;
	float decimal = 0;
	float revdecimal = 0;
	int count = 0;
	
	printf("The program decomposes the number into a chain fraction and finds its period\n");
	printf("Input a positive number:\n");
	while (n == 1)
	{
		int xscan = scanf("%d%c", &x, &eol);
		numbersq = sqrt(x);
		total = (int) numbersq;

		if (xscan && eol == '\n')
		{
			if (x <= 0)
			{
				printf("You entered non-positive number - try again\n");
			}
			else if  (numbersq- total == 0) 
			{
				printf("A number is the square of an integer");

			}
			else
			{
				n = 0;
				printf("[%d", total);
				decimal = numbersq - (int)total;

				while (total != 2 * (int)numbersq)
				{
					revdecimal = (1 / decimal);
					total = (int)revdecimal;
					decimal = revdecimal - (int)revdecimal;
					count++;
					if (total == 2 * (int)numbersq)
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
