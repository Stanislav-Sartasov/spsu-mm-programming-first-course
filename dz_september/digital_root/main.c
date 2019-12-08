#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int dr(int);

int main()
{

    int *a, max = 0, maxsum = 0;
    a = (int*)malloc(sizeof(int) * 999998);
    memset(a, 0, sizeof(int) * 999998);

    for (long i = 0; i < 999998; ++i)
    {
        //printf("%ld \n", i);
        max = 0;
        for (int k = 2; k * k <= (i + 2); ++k)
        {
            if (((i + 2) % k == 0) && (max <= a[((i + 2) / k) - 2] + a[k - 2]))
                 max = a[((i + 2) / k) - 2] + a[k - 2];
        }
        a[i] = max > dr(i+2) ? max : dr(i+2);

        maxsum += a[i];
    }
    printf("%d", maxsum);
    return 0;
}


int dr(int number)
{
    return (number % 9) == 0 ? 9 : (number % 9);
}
