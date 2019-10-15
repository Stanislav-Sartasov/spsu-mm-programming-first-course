#include <stdlib.h>
#include <stdio.h>
#include <malloc.h>

int correct_input()
{
    int fl = 0, ans;
    char s[10];
    while (!fl)
    {
        scanf("%s", &s);
        ans = 0;
        for (int j = 0; s[j] != '\0'; j++)
        {
            if ('0' > s[j] || s[j] > '9')
                fl = 1;
            ans = ans * 10 + (s[j] - '0');
        }
        if (!fl)
            break;
        printf("Please, enter correct number\n");
        fl = 0;
    }
    return ans;
}

int main()
{
    printf("Please enter some amount of money in pensions to find out the number of ways to collect this amount in coins\n");
    int n = correct_input();
    long long *dp = malloc(sizeof(long long) * ((n + 1) * 8));
    int coins[] = {1, 2, 5, 10, 20, 50, 100, 200};
    dp[0] = 1;
    for (int i = 1; i <= n; i++)
    {
        for (int j = 0; j < 8; j++)
        {
            dp[i * 8 + j] = 0;
            if (i - coins[j] < 0)
                continue;
            for (int k = 0; k <= j; k++)
            {
                dp[i * 8 + j] += dp[(i - coins[j]) * 8 + k];
            }
        }
    }
    long long ans = 0;
    for (int i = 0; i < 8; i++)
        ans += dp[n * 8 + i];
    printf("%lld", ans);
    free(dp);
    return 0;
}
