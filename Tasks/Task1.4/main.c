#include "stdio.h"
#include "math.h"

int is_prime(long int n)
{
    for (int i = 2; i < ((int) sqrt(n - 1)) + 1; i++)
    {
        if (n % i == 0)
            return 0;
    }
    return 1;
}

int main()
{
    int n_start = 2;
    long int curr_mersenne = 3;

    long int end = pow(2, 32) - 1;

    printf("Prime Mersenne numbers:\n");
    while (curr_mersenne <= end)
    {
        if (is_prime(curr_mersenne))
            printf("%ld\n", curr_mersenne);
        n_start++;

        curr_mersenne = pow(2, n_start) - 1;
    }

    return 0;
}
