#include "allocator.h"

int main()
{
	int* array = (int*)myMalloc(40 * sizeof(int));
	printf("Elements of the 1st array:\n");
	for (int i = 0; i < 40; i++)
	{
		array[i] = i;
		printf("%d ", array[i]);
	}
	printf("\n");

	int* array2 = (int*)myRealloc(array, 30 * sizeof(int));
	printf("Elements of the 2nd array:\n");
	for (int i = 0; i < 30; i++)
	{
		array2[i] = i;
		printf("%d ", array2[i]);
	}
	printf("\n");

	myFree(array2);

	int* array3 = (int*)myMalloc(50 * sizeof(int));
	printf("Elements of the 3d array:\n");
	for (int i = 0; i < 50; i++)
	{
		array3[i] = i;
		printf("%d ", array3[i]);
	}
	printf("\n");

	int* array4 = (int*)myMalloc(40 * sizeof(int));
	printf("Elements of the 4th array:\n");
	for (int i = 0; i < 40; i++)
	{
		array4[i] = i;
		printf("%d ", array4[i]);
	}
	printf("\n");

	int* array5 = (int*)myRealloc(NULL, 50 * sizeof(int));
	printf("Elements of the 5th array:\n");
	for (int i = 0; i < 50; i++)
	{
		array5[i] = i;
		printf("%d ", array5[i]);
	}
	printf("\n");

	myFree(array5);
	myFree(array3);
	myFree(array4);
	return 0;
}