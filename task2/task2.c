#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
int correct_input(char str[])
{
	char digits[11] = "0123456789";
	int digit_true, exit = 0;
	if (str[0] != '0')
	{

		for (int l = 0; (l < strlen(str)) && (exit != 1); l++)
		{
			digit_true = 0;
			for (int n = 0; (n < 10) && (exit != 1); n++)
			{
				if (str[l] == digits[n]) digit_true = 1;
			}
			if (digit_true == 0) exit = 1;
		}
	}
	else exit = 1;
	if (exit == 1) return 1;
	else return 0;
}

main()	
{
	int a[3] = { 0, 0, 0 };
	int k;
	char s[21];
	printf("Is it a primitive Pythagorean triple?\n");
	for (int i = 0; (i < 3); i++)
	{
		printf("Number %d = ", i + 1);	
		(void)scanf("%s", &s);
		if (correct_input(s) == 0) a[i] = atoi(s);
		else
		{
			printf("Incorrect input. Enter a positive integer.\n");
			i--; // Повторный ввод с текущего места. При вводе с самого начала i = -1.
		}
	}
	
	for (int i = 0; i < 2; i++)
	{
		for (int j = 0; j < (2 - i); j++)
		{
			if (a[j] > a[j + 1])
			{
				k = a[j];
				a[j] = a[j + 1];
				a[j + 1] = k;
			}
		}
	}
	if ((a[0] * a[0] + a[1] * a[1]) == (a[2] * a[2]))
	{
		while (a[0] != a[1] && (a[0] > 1) && (a[1] > 1))
		{
			if (a[1] > a[0]) a[1] = (a[1] % a[0]);
			else a[0] = (a[0] % a[1]);
		}
		if (a[0] == 1 || a[1] == 1) printf("Yes, it is.");
		else printf("No, it is not.");
	}
	else printf("It is not a Pythagorean triple.");
	return 0;
}
