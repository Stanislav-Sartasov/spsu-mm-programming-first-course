#include <stdio.h>
#include "allocator.h"


int main()
{
	char* a = myMalloc(10);
	for (int i = 0; i < 10; i++)
	{
		a[i] = i;
	}

	a = myRealloc(a, 13);
	for (int i = 10; i < 13; i++)
	{
		a[i] = -i;
	}

	for (int i = 0; i < 13; i++)
	{
		printf("a[%d] = %d\n", i, a[i]);
	}
	int* c = myMalloc(4 * sizeof(int));
	for (int i = 0; i < 4; i++)
	{
		c[i] = i * 5;
		printf("c[%d] = %d\n", i, c[i]);
	}
	myFree(a);
	char* b = myMalloc(13);
	for (int i = 0; i < 13; i++)
	{
		b[i] = 2 * i;
		printf("b[%d] = %d\n", i, b[i]);
	}
	char* d = myMalloc(16344);		//Out of memory
	free(memory_block);
	return 0;
}