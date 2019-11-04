#include <stdlib.h>
#include <stdio.h>
#include <malloc.h>
#include <math.h>

#define max(X, Y) (((X) > (Y)) ? (X) : (Y))

int sum(int n)
{
    while (n >= 10)
    {
        int m = 0;
        while (n)
        {
            m += n % 10;
            n /= 10;
        }
        n = m;
    }
    return n;
}

int main()
{
    int n = 999999;
    int *dp = malloc(sizeof(int) * (n + 1));
    int ans = 0;
    for (int i = 2; i <= n; i++)
    {
        dp[i] = sum(i);
        for (int j = 2; j <= (int)sqrt(i); j++)
        {
            if (i % j == 0)
            {
                dp[i] = max(dp[i], dp[j] + dp[i / j]);
            }
        }
        ans += dp[i];
    }
    printf("%d", ans);
    free(dp);
    return 0;
}
