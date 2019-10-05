#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
int toBin(unsigned short *mas, int n, int init)
{
	if (init >= 0)
	{
		for (int i = 0; i < n; i++)
		{
			if (init)
			{
				mas[i] = init % 2;
				init /= 2;
			}
			else mas[i] = 0;
		}
	}
	else
	{
		init = init * -1;
		for (int i = 0; i < n; i++)
		{
			if (init)
			{
				if (init % 2 == 1) mas[i] = 0;
				else mas[i] = 1;
				init /= 2;
			}
			else mas[i] = 1;
		}
	}
	return 0;
}
char toBin(char *mas, int n, int init)
{
	if (init >= 0)
	{
		for (int i = 0; i < n; i++)
		{
			if (init)
			{
				mas[i] = init % 2;
				init /= 2;
			}
			else mas[i] = 0;
		}
	}
	else
	{
		init = init * -1;
		for (int i = 0; i < n; i++)
		{
			if (init)
			{
				if (init % 2 == 1) mas[i] = 0;
				else mas[i] = 1;
				init /= 2;
			}
			else mas[i] = 1;
		}
	}
	return 0;
}
int main() {
	const char name[] = "Danil";
	const char surname[] = "Kostennikov";
	const char fathersname[] = "Vyacheslavovich";
	int leName = 0, leSurname = 0, leFathersname = 0, comp, i = 0;

	while (name[leName] != '\0') leName++;
	printf("%d\n", leName);

	while (surname[leSurname] != '\0') leSurname++;
	printf("%d\n", leSurname);

	while (fathersname[leFathersname] != '\0') leFathersname++;
	printf("%d\n", leFathersname);
	comp = leName * leFathersname*leSurname;
	printf("%d\n", comp);
	unsigned short bit32[32];


	toBin(bit32, 32, comp);
	for (i = 0; i < 32; i++) printf("%hu", bit32[31 - i]);
	i = 0;

	while (i >= 0)
	{
		if (bit32[i] == 0)
		{
			bit32[i] = 1;
			break;
		}
		else bit32[i] = 0;
		i++;
	}

	char bit32IE[33];
	int comp2 = comp;
	//toBin(bit32IE, 32, comp);

	for (i = 0; i < 21; i++)
	{
		bit32IE[i] = '0';
	}
	bit32IE[21] = '.';
	for (i = 22; i < 32; i++)
	{
		if (comp2)
		{
			bit32IE[i] = comp2 % 2 + '0';
			comp2 /= 2;
		}
		else bit32IE[i] = '0';
	}
	bit32IE[32] = '0';

	printf("\n%s\n", "Version A");
	for (i = 0; i < 32; i++) printf("%hu", bit32[31 - i]);
	printf("\n%s\n", "Version B");
	for (i = 0; i < 33; i++) printf("%c", bit32IE[32 - i]);

	comp2 = comp;
	char bit64IE[65];
	for (i = 0; i < 51; i++)
	{
		bit64IE[i] = '1';
	}
	bit64IE[51] = '.';
	for (i = 52; i < 64; i++)
	{
		if (comp2)
		{
			bit64IE[i] = comp2 % 2 + '0';
			comp2 /= 2;
			if (bit64IE[i] == '0') bit64IE[i] = '1';
			else bit64IE[i] = '0';
		}
		else bit64IE[i] = '1';
	}
	bit64IE[64] = '1';
	i = 52;
	while (i >= 0)
	{
		if (bit64IE[i] == '0')
		{
			bit64IE[i] = '1';
			break;
		}
		else bit64IE[i] = '0';
		i++;
	}
	printf("\n%s\n", "Version C");
	for (i = 0; i < 65; i++) printf("%c", bit64IE[64 - i]);


	system("pause");
	return 0;
}