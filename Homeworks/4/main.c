#include <stdio.h>
#include <stdlib.h>
#include "alloc.h"


int main()
{
	int* a = myMalloc(sizeof(int) * 20);

	for (int i = 0; i < 20; i++)
	{
		a[i] = i;
	}

	for (int i = 0; i < 20; i++)
	{
		printf("%d ", a[i]);
	}
	printf("\n");

	a = myRealloc(a, 200);

	for (int i = 0; i < 250; i++)
	{
		a[i] = i * 2;
	}
	for (int i = 0; i < 250; i++)
	{
		printf("%d ", a[i]);
	}

	myFree(a);

	return 0;
}