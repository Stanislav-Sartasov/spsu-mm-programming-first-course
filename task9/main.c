#include <stdio.h>
#include <stdlib.h>

#include "allocators.h"

int main()
{
    int *a = (int *)myMalloc(10 * sizeof(int));
    for (int i = 0; i < 10; i++)
        a[i] = i;
    for (int i = 0; i < 10; i++)
        printf("%d ", a[i]);
    printf("\n");
    a = myRealloc(a, 1000);
    a[995] = 5;
    for (int i = 0; i < 1000; i++)
        printf("%d ", a[i]);
    int *a1 = (int *)myMalloc(sizeof(int));
    myFree(a1);
    int *b1 = (int *)myMalloc(sizeof(int));
    myFree(a);
    myFree(b1);
    totalFree();
    return 0;
}
