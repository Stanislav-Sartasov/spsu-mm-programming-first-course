#include "new_malloc.h"
#define endl printf("\n")
#define SIZE 10

int main()
{
    int** A;
    int i, j;
    A = (int**)new_malloc(SIZE * sizeof(int*));

    new_free(A[4]);

    for (i = 0; i < SIZE; i++) {
        A[i] = (int*)new_malloc((i + 1) * sizeof(int));
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
        new_free(A[i]);
    }
    new_free(A);

    _getch();
}
