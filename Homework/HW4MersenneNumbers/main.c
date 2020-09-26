#include <stdio.h>
#include <math.h>

int mersenneNumber()
{
    int numb;
    for (int n = 1; n < 32; n++)
    {
        int prime = 0;
        numb = pow(2, n) - 1;
        for (int j = 2; j < sqrt(b); j++)
            if (b % j == 0)
                prime = 1;
        if (prime == 0)
            printf("%d\n", numb);
    }
    return 0;
}

int main()
{
    printf("This prog. prints all the Mersenne primes in interval [1, 2^31 - 1]\n");
    mersenneNumber();
    return 0;
}

