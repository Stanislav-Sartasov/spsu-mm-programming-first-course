#include "stdio.h"
#include "math.h"

int check_numbers(int x, int y, int z)
{
    if (x*x + y*y == z*z)
    {
        return 1;
    }
    return 0;
}

int min3(int a, int b, int c)
{
    if (a <= b && a <= c)
    {
        return a;
    }
    else if (b <= a && b <= c)
    {
        return b;
    }
    else
    {
        return c;
    }

}

int check_three_prime(int x, int y, int z)
{
    for (int i = 2; i < min3(x, y, z); i++)
    {
        if (x % i == 0 && y % i == 0 && z % i == 0)
        {
            return 0;
        }
    }
    return 1;
}


int main()
{
    setbuf(stdout, NULL);

    int x, y, z;

    printf("Input x y z: ");
    scanf("%d %d %d", &x, &y, &z);

    printf("%d %d %d\n", x, y, z);

    if (check_numbers(x, y, z))
    {
        printf("True\n");
        if (check_three_prime(x, y, z))
            printf("Numbers are prime");
        else
            printf("Numbers are not prime");
    }
    else
    {
        printf("False\n");
    }

    return 0;
}

