#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include <malloc.h>

int main()
{
    int base = 3;
    int pow = 5000;
    int n = (log(base) / log(16)) * pow + 1;
    int *a = (int*) malloc(n * sizeof(int));
    memset(a, 0, n * sizeof(int));
    a[0] = 1;
    for (int i = 0; i < pow; i++)
    {
        int prev = 0;
        for (int j = 0; j < n; j++)
        {
            a[j] = a[j] * 3 + prev;
            prev = a[j] / 16;
            a[j] %= 16;
        }
    }
    for (int i = n - 1; i >= 0; i--)
        printf("%x", a[i]);
    free(a);
    return 0;
}
