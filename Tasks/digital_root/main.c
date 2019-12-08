#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

const int n = 1000000;

int find_next_permutation(int k, int n, int *digits)
{
    int i;
    int overflow;
    for (i = k - 1, overflow = n; i != -1;)
    {
        digits[i]++;
        if (digits[i] < overflow)
            break;
        i--;
        overflow--;
    }
    if (i != -1)
    {
        for (i = i + 1; i < k; i++)
            digits[i] = digits[i - 1] + 1;
        return 1;
    }
    return 0;
}

int find_next_divider(int *y)
{
    if (*y % 2 == 0)
    {
        *y /= 2;
        return 2;
    }
    int x = 3;
    while (x * x <= *y)
    {
        if (*y % x == 0)
        {
            *y /= x;
            return x;
        }
        x += 2;
    }
    return 1;
}

int find_max_root(int y, unsigned char *a)
{
    if (a[y] != 0)
        return a[y];
    int res = 1 + (y - 1) % 9;
    int i = 0;
    int j = y;
    int v[20];
    int div = 0;
    while (j != 1 && div != 1)
    {
        div = find_next_divider(&j);
        if (div != 1)
        {
            v[i] = div;
            i++;
        }
    }
    for (int e = 1; e < i + 1; e++)
    {
        int combination[e];
        for (int k = 0; k < e; k++)
            combination[k] = k;
        int existence_next_permutation = 1;
        while (existence_next_permutation)
        {
            int x1 = 1;
            for (int k = 0; k < e; k++)
                x1 *= v[combination[k]];
            int c1 = y / x1;
            if (x1 != 1)
                if (a[x1] + a[c1] > res)
                    res = a[x1] + a[c1];
            existence_next_permutation = find_next_permutation(e, i , combination);
        }
    }
    a[y] = res;
    return res;
}

int main()
{
    clock_t begin = clock();
//    unsigned char *v = (unsigned char*) malloc(sizeof(unsigned char) * n);
//    memset(v, 0, n);
    unsigned char *v = (unsigned char*) calloc(n,sizeof(unsigned char));
    int ans = 0;
    for (int i = 2; i < n; i++)
    {
//        v[i] = 0;
        ans += find_max_root(i, v);
    }
    printf("sum of digital roots = %d", ans);
    free(v);
    clock_t end = clock();
    double time_spent = (double)(end - begin) / CLOCKS_PER_SEC;
    printf("\nTime of compilation = %f", time_spent);
    return 0;
}
