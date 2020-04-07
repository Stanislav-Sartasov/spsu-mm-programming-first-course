#include <stdio.h>
#include <stdlib.h>
#include <math.h>

long long is_prime(int x)
{
    if (x % 2 == 0)
        return x == 2;
    for (int j = 3; j < round(sqrt(x)) + 1; j++)
    {
        if (x % j == 0)
            return 0;
    }
    return 1;
}

void  exponentiation(long long *x, int *pow_x, int limit, int two, int three, int five)
{
    while (*pow_x + 5 <= limit)
    {
        *x *= five;
        *pow_x += 5;
    }
    while (*pow_x + 3 <= limit)
    {
        *x *= three;
        *pow_x += 3;
    }
    while (*pow_x + 2 <= limit)
    {
        *x *= two;
        *pow_x += 2;
    }
    while (*pow_x + 1 <= limit)
    {
        *x *= 2;
        *pow_x  += 1;
    }
}

int main()
{
    printf("Prime numbers: \n");
    long long n = 1;
    int current_pow_n = 0;
    for (int i = 2; i < 32; i++)
    {
        if (is_prime(i))
        {
            exponentiation(&n, &current_pow_n, i, 4, 8, 32);
            if (is_prime(n - 1))
                printf("%lld\n", n - 1);
        }
    }
    return 0;
}
