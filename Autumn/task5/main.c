#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <math.h>

int main()
{
    char s[10];
    long long x;
    printf("Please enter natural non-square numbers to find out continued fraction period\n");

    int fl = 0;
    while (!fl)
    {
        scanf("%s", &s);
        x = 0;
        for (int j = 0; s[j] != '\0'; j++)
        {
                if ('0' > s[j] || s[j] > '9')
                    fl = 1;
                x = x * 10 + (s[j] - '0');
        }
        if (!fl)
        {
            if (sqrt(x) == (long long)sqrt(x))
            {
                printf("Please, enter non-square number\n");
                continue;
            }
            else
            {
                break;
            }
        }
        printf("Please, enter correct numbers\n");
        fl = 0;
    }

    int size_of_period = 0;
    long long a0 = (long long)sqrt(x), a = 0;
    long long b = a0, c = x - a0 * a0;
    printf("[%lld", a0);

    while (c != 1)
    {
        size_of_period++;
        a = (long long)((double)(a0 + b) / (double)c);
        printf(", %lld", a);
        b = a * c - b;
        c = (x - b * b) / c;
    }
    printf(", %lld] %d", (a0 + b), size_of_period + 1);
    return 0;
}
