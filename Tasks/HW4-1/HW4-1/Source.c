#include "newMalloc.h"
#define endl printf("\n")
#define SIZE 10

//int main()
//{
//	int* arr1 = newMalloc(10 * sizeof(int));
//	for (int i = 0; i < 10; i++)
//	{
//		arr1[i] = 1;
//		printf("%d", arr1[i]);
//	}
//	endl;
//	char* arr2 = newMalloc(10 * sizeof(char));
//	for (int i = 0; i < 10; i++)
//	{
//		arr2[i] = 'a';
//		printf("%c", arr2[i]);
//	}
//	endl;
//
//	endl;
//
//	for (int i = 0; i < 10; i++) printf("%d", arr1[i]);
//	endl;
//	for (int i = 0; i < 10; i++)printf("%c", arr2[i]);
//
//	endl; endl;
//	float* arr3 = newMalloc(5 * sizeof(float));
//	for (int i = 0; i < 5; i++)
//	{
//		arr3[i] = 1.1 + i;
//		printf("%f%c", arr3[i], ' ');
//	}
//
//	newFree(arr2);
//
//	arr1 = newRealloc(arr1, 15 * sizeof(int));
//	arr3 = newRealloc(arr3, 10 * sizeof(float));
//	endl;
//
//	for (int i = 10; i < 15; i++) arr1[i] = 1;
//	for (int i = 0; i < 15; i++) printf("%d", arr1[i]);
//	endl; endl;
//
//	for (int i = 5; i < 10; i++) arr3[i] = 1.1 + i;
//	for (int i = 0; i < 10; i++) printf("%f%c", arr3[i], ' ');
//	endl;
//
//	system("pause");
//} 

int main()
{
    int** A;
    int i, j;
    A = (int**)newMalloc(SIZE * sizeof(int*));

    for (i = 0; i < SIZE; i++) {
        A[i] = (int*)newMalloc((i + 1) * sizeof(int));
    }

    for (i = 0; i < SIZE; i++) {
        for (j = i; j > 0; j--) {
            A[i][j] = i * j;
        }
    }

    for (i = 0; i < SIZE; i++) {
        for (j = i; j > 0; j--) {
            printf("%d ", A[i][j]);
        }
        printf("\n");
    }

    for (i = SIZE - 1; i > 0; i--) {
        newFree(A[i]);
    }
    newFree(A);

    _getch();
}