#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
int PythTh(int x, int y, int z)
{
	unsigned short int isTrue;
	if ((x*x + y * y) == (z*z)) isTrue = 1;
	else if ((x*x + z * z) == (y*y)) isTrue = 1;
	else if ((y*y + z * z) == (x*x)) isTrue = 1;
	else isTrue = 0;
	return isTrue;
}
int comprNum(int x, int y, unsigned short int isComprNum)
{
	while (x != y)
	{
		if (x > y) x -= y;
		else y -= x;
	}
	if (x == 1) isComprNum = isComprNum + 1;
	return isComprNum;
}
int main()
{
	system("chcp 1251");
	int x, y, z;
	int res;

	do {
		res = scanf("%d%d%d", &x, &y, &z);
		while (getchar() != '\n');
		if (res == 3) break; //printf("%s", "’орошо!\n");
		else printf("%s", "¬ведены неверные значени€, попробуйте снова\n");
	} 
	while (res != 3);
	if (x <= 0 || y <= 0 || z <= 0)
	{
		printf("%s", "¬ведены неверные значени€, попробуйте снова\n");
		scanf("%d%d%d", &x, &y, &z);
	}

	unsigned short int isTrue, isComprNum = 0;

	isTrue = PythTh(x, y, z);

	if (isTrue == 1) printf("%s\n", "„исла €вл€ютс€ пифагоровыми тройками");
	else printf("%s\n", "„исла не €вл€ютс€ пифагоровыми тройками");

	isComprNum = comprNum(x, y, isComprNum);
	isComprNum = comprNum(x, z, isComprNum);
	isComprNum = comprNum(z, y, isComprNum);

	if (isComprNum == 3) printf("%s\n", "„исла €вл€ютс€ простыми пифагоровыми тройками");
	else printf("%s\n", "„исла не €вл€ютс€ простыми пифагоровыми тройками");

	system("pause");
	return 0;
}