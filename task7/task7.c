#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <malloc.h>
#include <string.h>
#include <stdlib.h>
#include "hash_table.h"

int correct_input(char str[])
{
	char digits[11] = "0123456789";
	int digit_true;
	if (str[0] != '0')
	{

		for (int l = 0; (l < strlen(str)); l++)
		{
			digit_true = 0;
			for (int n = 0; (n < 10); n++)
			{
				if (str[l] == digits[n]) digit_true = 1;
			}
			if (digit_true == 0) return 1;
		}
	}
	else return 1;
	return 0;
}

int input(char string[])
{
	int j = 0;
	while (j != 1)
	{
		(void)scanf("%s", string);
		if (correct_input(string) != 0) printf("Incorrect input. Enter a positive integer.\n");
		else j = 1;
	}
	return atoi(string);
}



int main()
{
	int exit = 1;
	int choice = 0;
	int key;
	int value;
	char s[21];

	struct hash_table p;
	create(&p);

	printf("\nMENU \n1.Insert \n2.Search \n3.Delete \n4.Exit \n");
	while (exit == 1)
	{
		printf("\nSelect menu item ");
		choice = input(s);
		if (choice == 1)
		{
			printf("\nEnter the value ");

			value = input(s);
			printf("\nEnter the key ");
			key = input(s);
			insert(&p, value, key);
		}
		else if (choice == 2)
		{
			printf("\nEnter the key: ");
			key = input(s);
			if (search(&p, key) == -1) printf("\nThe key searched was not found in the hash table");
			else printf("Searched element = %d\n", p.values[search(&p, key)]);
		}
		else if (choice == 3)
		{
			printf("\nEnter the key: ");
			key = input(s);
			remove_element(&p, key);
		}
		else if (choice == 4)
		{
			exit = 0;
		}
		else printf("Incorrrect input.\n");
	}

	free(p.keys);
	free(p.values);

	return 0;
}