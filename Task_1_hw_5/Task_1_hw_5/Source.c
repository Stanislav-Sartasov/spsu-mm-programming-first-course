#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>

int find(int sum, int index)
{
	int mas[] = { 1 , 2 , 5 , 10 , 20 , 50 , 100 , 200 };
	int count = 0;

	if ((sum < 0) || (index == -1))
	{
		 count = 0;
	}
	else if ((sum == 0) || (index == 0))
	{
		 count = 1;
	}
	else 
	{
		count = find(sum, index - 1) + find(sum - mas[index], index);
	}
	return count;
}




int main()
{
	int sum = 0; int n = 0;
	char eol = 0;
	printf("The program solves the task \"English coin\"\n");
	printf("Please, enter the number\n");
	while (n == 0)
	{
		int sumscanf = scanf("%d%c", &sum, &eol);
		if (sumscanf &&  eol == '\n' && sum > 0)
		{
			n = 1;
			printf("%d", find(sum, 7));
		}
		else
		{
			printf("Error input. Try again\n");
			scanf("%*[^\n]");
		}
	}
		
	return 0;
}