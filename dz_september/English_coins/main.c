#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "../mylib/functionToGo.h"

int main()
{
    int sum = 0;
    int coin[8] = {1, 2, 5, 10, 20, 50, 100, 200};
    do
    {
        printf("input your sum (more then 0): "); //input data
        sum = (int)saveInInt();
    }
    while (sum < 1);
    ++sum;

    long long **a = (long long**)malloc((unsigned long long)((int)sizeof(long long*) * 8));  //memory allocation
    for (int i = 0; i < 8; ++i)
        a[i] = (long long*)malloc((unsigned long long)((int)sizeof (long long) * sum));

    for (int i = 0; i < 8; ++i)
        for (int k = 0; k < sum; ++k)
            a[i][k] = 0;
    for (int i = 0; i < sum; ++i)           //first initialization of allocated array
        a[0][i] = 1;
    for (int i = 0; i < 8; ++i)
    {
        a[i][0] = 1;
        a[i][1] = 1;
    }


    for (int i = 2; i < sum; ++i)
    {
        for (register int k = 1; k < 8; ++k)
        {
            a[k][i] = a[k - 1][i] + (i - coin[k] >= 0 ? a[k][i - coin[k]] : 0);  //array filling in
        }
    }

//    for (int i = 0; i < 8; ++i)
//    {
//        for (int k = 0; k < sum; ++k)
//            printf("%lld ", a[i][k]);
//        printf("\n");
//    }
    printf("number of variants: %lld", a[7][sum-1]); //answer output

    for (int i = 0; i < 8; ++i)
        free(a[i]);                //memory cleaning up
    free(a);
    return 0;
}
