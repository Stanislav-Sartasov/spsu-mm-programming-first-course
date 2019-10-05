#define _CRT_SECURE_NO_WARNINGS
#include<stdio.h>
#include<stdlib.h>
int five(int num, int &k)
{
	k++;
	int tmp = num;
	while (true)
	{
		tmp -= 5;
		if (tmp > 0)
		{	k++;
			printf("k= %d\n", k);
			
			k += tmp / 2;
		}
		else break;
	}
	return 0;
}

int ten(int num, int &k)
{
	k++;
	five(num,k);
	int tmp = num;
	while (true)
	{
		tmp -= 10;
		if (tmp > 0)
		{
			printf("k= %d\n", k);
			k += tmp / 2;
		}
		else if(tmp==0) k++;
		else break;
	}
	tmp=num;
	while (tmp > 0)
	{
		tmp-=10;
		if(tmp>=5) five(tmp,k);
		else break;
	}

	return 0;
}

int twenty(int num, int &k)
{
	k++;
	ten(num,k);
	int tmp=num;
	while (true)
	{
		tmp -= 20;
		if (tmp > 0)
		{
			printf("k= %d\n", k);
			k += tmp / 2;
		}
		else if (tmp == 0) k++;
		else break;
	}
	tmp = num;
	while (tmp > 0)
	{
		tmp -= 20;
		if (tmp >= 5) five(tmp, k);
		else break;
	}
	tmp = num;
	while (tmp > 0)
	{
		tmp -= 20;
		if (tmp >= 10) ten(tmp, k);
		else break;
	}
	return 0;
}
int fifty(int num, int &k)
{
	k++;
	twenty(num, k);
	int tmp = num;
	while (true)
	{
		tmp -= 50;
		if (tmp > 0)
		{
			printf("k= %d\n", k);
			k += tmp / 2;
		}
		else if (tmp == 0) k++;
		else break;
	}
	tmp = num;
	while (tmp > 0)
	{
		tmp -= 50;
		if (tmp >= 5) five(tmp, k);
		else break;
	}
	tmp = num;
	while (tmp > 0)
	{
		tmp -= 50;
		if (tmp >= 10) ten(tmp, k);
		else break;
	}
	tmp = num;
	while (tmp > 0)
	{
		tmp -= 50;
		if (tmp >= 10) twenty(tmp, k);
		else break;
	}
	return 0;
}
int oneHundred(int num,int &k)
{
	k++;
	fifty(num, k);
	int tmp = num;
	while (true)
	{
		tmp -= 100;
		if (tmp > 0)
		{
			printf("k= %d\n", k);
			k += tmp / 2;
		}
		else if (tmp == 0) k++;
		else break;
	}
	tmp = num;
	while (tmp > 0)
	{
		tmp -= 100;
		if (tmp >= 5) five(tmp, k);
		else break;
	}
	tmp = num;
	while (tmp > 0)
	{
		tmp -= 100;
		if (tmp >= 5) ten(tmp, k);
		else break;
	}
	tmp = num;
	while (tmp > 0)
	{
		tmp -= 100;
		if (tmp >= 5) twenty(tmp, k);
		else break;
	}
	tmp = num;
	while (tmp > 0)
	{
		tmp -= 100;
		if (tmp >= 5) fifty(tmp, k);
		else break;
	}
	return 0;
}
int twoHundred(int num, int &k)
{
	k++;
	oneHundred(num, k);
	int tmp = num;
	while (true)
	{
		tmp -= 200;
		if (tmp > 0)
		{
			printf("k= %d\n", k);
			k += tmp / 2;
		}
		else if (tmp == 0) k++;
		else break;
	}
	tmp = num;
	while (tmp > 0)
	{
		tmp -= 200;
		if (tmp >= 5) five(tmp, k);
		else break;
	}
	tmp = num;
	while (tmp > 0)
	{
		tmp -= 200;
		if (tmp >= 5) ten(tmp, k);
		else break;
	}
	tmp = num;
	while (tmp > 0)
	{
		tmp -= 200;
		if (tmp >= 5) twenty(tmp, k);
		else break;
	}
	tmp = num;
	while (tmp > 0)
	{
		tmp -= 200;
		if (tmp >= 5) fifty(tmp, k);
		else break;
	}
	tmp = num;
	while (tmp > 0)
	{
		tmp -= 200;
		if (tmp >= 5) oneHundred(tmp, k);
		else break;
	}
	return 0;
}
int main()
{
	int num,k=1;
	scanf("%d",&num);
	k=1+num/2;
	if(num >= 5 && num < 10) five(num,k);
	if(num==10) ten(num, k)-1;
	if(num > 10 && num <= 20) ten(num, k);
	if(num > 20 && num <= 50) twenty(num, k);
	if(num > 50 && num <= 100) fifty(num, k);
	if(num > 100 && num <= 200) oneHundred(num, k);
	if(num > 200) twoHundred(num, k);
	printf("k= %d\n", k);
	system("pause");
	return 0;
}