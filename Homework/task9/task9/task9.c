#include "allocator.h"

int main()
{
	int n = 60;
	init();
	int** a = (int**)myMalloc(sizeof(int*) * n * n);
	if (!a)
		printf("Memory allocation error!\n");
	a = (int**)myMalloc(sizeof(int*) * 8);
	if (a)
	{
		for (int i = 0; i < 8; i++)
		{
			a[i] = (int*)myMalloc(sizeof(int) * 8);
			if (a[i])
			{
				for (int j = 0; j < 8; j++)
				{
					a[i][j] = i + j;
					printf("%d\n", a[i][j]);
				}
			}
		}
		for (int i = 0; i < 8; i++)
		{
			myFree(a[i]);
		}
	}

	a = (int**)myMalloc(sizeof(int) * n * 4);
	int* b = (int*)myMalloc(sizeof(int) * 4);
	for (int i = 0; i < 4; i++)
	{
		b[i] = i;
		printf("%d\n", b[i]);
	}
	int* c = (int*)myMalloc(sizeof(int) * n);
	if (!c)
		printf("Memory allocation error!\n");
	myFree(a);
	c = (int*)myMalloc(sizeof(int) * n);
	if (!c)
		printf("Memory allocation error!\n");
	myFree(c);
	printf("%p\n", b);
	b = myRealloc(b, sizeof(int) * 5);
	printf("%p\n", b);
	b = myRealloc(b, sizeof(int) * (n + 5));
	printf("%p\n", b);

	for (int i = 4; i < n + 5; i++)
	{
		b[i] = i;
	}

	for (int i = 0; i < n + 5; i++)
	{
		printf("%d\n", b[i]);
	}
	myFree(a);
	myFree(b);
	myFree(c);
	uninit();
	return 0;
}
