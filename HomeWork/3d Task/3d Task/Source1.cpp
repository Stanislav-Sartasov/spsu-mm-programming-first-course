#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
void Tran(float a, float b, float c,unsigned int &isTran)
{
	if((a+b>c)&&(a+c>b)&&(b+c>a)) isTran=1;
	else  isTran=0;
}
void angle(float a, float b, float c)
{
	float cosa;
	cosa=(pow(a,2)+pow(b,2)-pow(c,2))/ (-2)*a*b;
	printf("%f\n",acos(cosa));
}

int main()
{
	system("chcp 1251");
	float a,b,c;
	unsigned int isTran=0;
	scanf("%f%f%f", &a,&b,&c);
	Tran(a,b,c,isTran);
	printf("%hu",isTran);


	float cosa;
	cosa=(a*a-b*b-c*c)/((-2)*b*c);
	cosa=acos(cosa)*180/3.14;
	printf("\n%f", cosa);


	system("pause");
	return 0;
}