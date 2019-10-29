#include <stdio.h>
#include <stdlib.h>
#include <time.h>

const int n = 999999;

int find_max_root(int y, unsigned char *v)
{
    if (v[y] != 0)
            return v[y];
    int res = 1 + (y - 1) % 9;
    int i = 2;
    while (i * i <= y)
    {
        if (y % i == 0)
        {
            int div = y / i;
            int x1 = 1 + (i - 1) % 9;
            int c1 = v[div];
            if (x1 + c1 > res)
                res = x1 + c1;
        }
        i++;
    }
    v[y] = res;
    return res;
}


int main()
{
    unsigned char *v = (unsigned char*) malloc(sizeof(unsigned char) * (n + 1));
    int ans = 0;
    int i = 2;
    for (i; i < n + 1; i++)
    {
        v[i] = 0;
        ans += find_max_root(i, v);
    }
    printf("sum of digital roots = %d", ans);
    free(v);
    return 0;
}
