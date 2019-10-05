#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
int PythTh(int x, int y, int z)
{
	unsigned short int isTrue;
	if((x*x+y*y)==(z*z)) isTrue=1;
	else if ((x*x+z*z)==(y*y)) isTrue=1;
	else if ((y*y+z*z)==(x*x)) isTrue=1;
	else isTrue=0;
	return isTrue;
}
int comprNum(int x, int y, unsigned short int isComprNum)
{
	while (x != y)
	{
		if (x > y) x -= y;
		else y -= x;
	}
	if (x==1) isComprNum= isComprNum+1;
	return isComprNum;
}
int main()
{
	system("chcp 1251");
	int x,y,z;
	scanf("%d%d%d",&x,&y,&z);
	unsigned short int isTrue,isComprNum=0;
	isTrue=PythTh(x,y,z);
	if (isTrue==1) printf("%s\n","Числа являются пифагоровой тройкой");
	else printf("%s\n", "Числа не являются пифагоровой тройкой");
	isComprNum=comprNum (x,y,isComprNum);
	isComprNum=comprNum (x,z,isComprNum);
	isComprNum=comprNum (z,y,isComprNum);
	if (isComprNum==3) printf("%s\n", "Простая пифагорова тройка");
	else printf("%s\n", "Не простая пифагорова тройка");

	system("pause");
	return 0;
}