#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>

int main()
{
	int* mas = (int*)calloc(sizeof(int), 1);
	mas[0] = 3;
	int count = 1;

	printf("The program calculates 3^5000");
	for (int i = 0; i < 4999; i++)
	{
	

		for (int j = 0; j < count; j++)
		{
			mas[j] = mas[j] * 3;
		}
		
		if (mas[count - 1] > 15)
		{
			if (!mas)
			{
				printf("no");
			}
			else
			{
				mas = realloc(mas, (count + 1) * sizeof(int));
				mas[count] = 0;
				count = count + 1;
			}
		}

		for (int j = 0; j < count - 1 ; j++)
		{
			if (mas[j] > 15)
			{
				mas[j + 1] = mas[j + 1] + (int)(mas[j] / 16);
				mas[j] = mas[j] % 16;
			}
		}
	}




	for (int i = (count - 1); i >= 0 ; i--)
	{
		switch (mas[i])
		{
		case 10:
			printf("A");
			break;
		case 11:
			printf("B");
			break;
		case 12:
			printf("C");
			break;
		case 13:
			printf("D");
			break;
		case 14:
			printf("E");
			break;
		case 15:
			printf("F");
			break;
		default:
			printf("%d", mas[i]);
			break;
		}
		

	}
	free(mas);
	return 0;
}
