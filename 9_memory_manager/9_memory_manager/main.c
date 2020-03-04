#include "allocator.h"

int main()
{
	int* first = myMalloc(sizeof(int) * 100);
	for (int i = 0; i < 100; i++)
	{
		first[i] = i % 17;
	}
	printf("Allocated first array: ");
	for (int i = 0; i < 100; i++)
	{
		printf("%d ", first[i]);
	}
	printf("\n");

	int* second = myMalloc(sizeof(int) * 100);
	for (int i = 0; i < 100; i++)
	{
		second[i] = i % 39;
	}
	printf("Allocated second array: ");
	for (int i = 0; i < 100; i++)
	{
		printf("%d ", second[i]);
	}
	printf("\n");

	first = myRealloc(first, sizeof(int) * 200);
	for (int i = 100; i < 200; i++)
	{
		first[i] = i;
	}
	printf("Reallocated first array: ");
	for (int i = 0; i < 200; i++)
	{
		printf("%d ", first[i]);
	}
	printf("\n");

	myFree(first);
	myFree(second);
	return 0;
}