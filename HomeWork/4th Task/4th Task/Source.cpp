#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include<stdlib.h>
#include <math.h>
int main()
{
	
	int i,x;
	for (i = 1; i <= 31; i++)
	{
		x=pow(2,i)-1;
		if(x==1||x==3||x==5||x==7||(x%2!=0 && x%3!=0 && x%5!=0 && x%7!=0))
			printf("%d\n",x);
	}

	system("pause");
	return 0;
}