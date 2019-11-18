#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>
#include <stdlib.h>

int main()
{
	int penny[8] = { 1,2,5,10,20,50,100,200 };
	long* methods;
	int num;

	scanf_s("%d", &num);

	methods = (long*)malloc((num + 1) * sizeof(long));
		
	for (int i = 0; i <= num; i++)
		methods[i] = 1;

	for (int i = 1; i < 8; i++) 
	{
		for (int j = penny[i]; j <= num; j++)
		{
			methods[j] = methods[j - penny[i]] + methods[j];
		}
	}
	printf("All methods is: %d", methods[num]);

	return 0;
}