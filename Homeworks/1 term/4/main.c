#include <stdio.h>
#include <stdlib.h>
#include "alloc.h"


int main()
{
	int* a = (int*)myMalloc(sizeof(int) * 20);

	for (int i = 0; i < 20; i++)
	{
		a[i] = i;
	}

	for (int i = 0; i < 20; i++)
	{
		printf("%d ", a[i]);
	}
	printf("\n");

	a = (int*)myRealloc(a, sizeof(int) * 200);

	for (int i = 20; i < 250; i++)
	{
		a[i] = i;
	}
	for (int i = 0; i < 250; i++)
	{
		printf("%d ", a[i]);
	}

	myFree(a);

	return 0;
}