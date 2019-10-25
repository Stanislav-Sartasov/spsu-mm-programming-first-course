#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include<math.h>
int main()
{
	char number[100];
	int i, begin, end,k=0,mist;
	printf("%s", "Enter the number \n");
	for (;;)  //safty input
	{
		k = 0;
		mist = 0;
		scanf("%s", number);
		for (i = 0; i < number[i] != '\0'; i++)
		{
			if (number[i] == ',' || number[i] == '.')
				k++;
			if (k > 1 || (number[i] < 48 && number[i] != '.' && number[i] != ',') || (number[i] > 57 && number[i] != '.' && number[i] != ','))
			{
				mist++;
				break;
			}
		}
		if (mist > 0) printf("%s", "Invalid numbers entered, try again\n");
		else break;
	}

	for (i = 0; i < 100; i++)
	{
		if (number[i] == ',' || number[i] == '.') 
			begin = i;
		if (number[i] == '\0')
		{
			end = i;
			break;
		}
	}
	//printf("%s\n", number);
	//printf("%s", "Begin is ");
	//printf("%d", begin);
	//printf("%s", " End is ");
	//printf("%d\n", end); 
	
	char firstPartOfNumber[100], secondPartOfNumber[100];


	for (i = 0; i <begin; i++) firstPartOfNumber[i] = number[i];
	firstPartOfNumber[begin] = '\0';

	for (i = begin+1; i < end; i++) secondPartOfNumber[i-begin-1] = number[i];
	secondPartOfNumber[end-begin-1] = '\0';

	//printf("%s\n", firstPartOfNumber);
	//printf("%s\n", secondPartOfNumber);

	long long int fst, snd;
	sscanf(firstPartOfNumber, "%lld", &fst);
	sscanf(secondPartOfNumber, "%lld", &snd);
	//printf("\n%lld\n", fst);
	//printf("%lld\n", snd);
	
	long long int power;
	power = pow(10, end-begin-1);
	fst = fst * power+snd;
	//printf("%lld\n", power);
	//printf("%lld\n", fst);


	printf("%s\n", "Sequence is");

	while (fst!=0)
	{
		k++;
		printf("%lld%s", fst / power," ");
		snd = power;
		power = fst % power;
		fst = snd;
		if (fst <= 1|| power <= 1) break;
	}
	k--;
	printf("\n%s\n", "Period is ");
	printf("%d\n", k);
	system("pause");
	return 0;
}