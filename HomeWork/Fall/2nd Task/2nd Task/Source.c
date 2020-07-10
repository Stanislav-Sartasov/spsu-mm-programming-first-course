#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>

int PythTh(int x, int y, int z)
{
	unsigned short int isTrue;
	if ((x * x + y * y) == (z * z)) isTrue = 1;
	else if ((x * x + z * z) == (y * y)) isTrue = 1;
	else if ((y * y + z * z) == (x * x)) isTrue = 1;
	else isTrue = 0;
	return isTrue;
}

int coprimeNum(int x, int y, unsigned short int isCoprimeNum)
{
	while (x != y)
	{
		if (x > y) x -= y;
		else y -= x;
	}
	if (x == 1) isCoprimeNum = isCoprimeNum + 1;
	return isCoprimeNum;
}

int main()
{
	int x, y, z, res;
	printf("%s", "Enter 3 numbers \n");
	do
	{
		res = scanf("%d%d%d", &x, &y, &z);
		if (x <= 0 || y <= 0 || z <= 0) res = 0;
		while (getchar() != '\n');
		if (res == 3) break; 
		else printf("%s", "Invalid numbers entered, try again\n");
	} 
	while (res != 3);

	unsigned short int isTrue, isCoprimeNum = 0;

	isTrue = PythTh(x, y, z);

	if (isTrue == 1) printf("%s\n", "Numbers are Pythagorean triples");
	else printf("%s\n", "Numbers are not Pythagorean triples");

	isCoprimeNum = coprimeNum(x, y, isCoprimeNum);
	isCoprimeNum = coprimeNum(x, z, isCoprimeNum);
	isCoprimeNum = coprimeNum(z, y, isCoprimeNum);

	if (isCoprimeNum == 3) printf("%s\n", "Numbers are simple Pythagorean triples");
	else printf("%s\n", "Numbers are not simple Pythagorean triples");

	system("pause");
	return 0;
}
