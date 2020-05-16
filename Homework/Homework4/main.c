#include <stdio.h>
#include <math.h>

int mersenNumber()
{
    int n, b, flag;
    for (n = 1; n < 32; n++)
    {
        flag = 1;
        b = pow(2, n) - 1;
        for (int j = 2; j < (int)sqrt(b); j++)
        {
            if (b % j == 0)
                flag = 0;
        }
        if (flag)
            printf("%d\n", b);
    }
    return 0;
}

int main()
{
    printf("This programm print's all the Mersenne primes in interval [1, 2^31 - 1]\n");
    mersenNumber();
    return 0;
}