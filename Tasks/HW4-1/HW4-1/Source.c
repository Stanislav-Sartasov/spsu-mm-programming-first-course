#include "newMalloc.h"
#define endl printf("\n")
#define SIZE 10

int main()
{
    int** A;
    int i, j;
    A = (int**)newMalloc(SIZE * sizeof(int*));

    for (i = 0; i < SIZE; i++) {
        A[i] = (int*)newMalloc((i + 1) * sizeof(int));
    }

    for (i = 0; i < SIZE; i++) 
    {
        for (j = i; j > 0; j--) 
        {
            A[i][j] = i * j;
        }
    }

    for (i = 0; i < SIZE; i++) 
    {
        for (j = i; j > 0; j--) 
        {
            printf("%d ", A[i][j]);
        }
        printf("\n");
    }

    for (i = SIZE - 1; i > 0; i--) 
    {
        newFree(A[i]);
    }
    newFree(A);

    _getch();
}
