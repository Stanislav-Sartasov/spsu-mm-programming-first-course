#include "stdio.h"
#include "math.h"

int check_numbers(int x, int y, int z)
{
    if (x * x + y * y == z * z)
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

    if (scanf("%d %d %d", &x, &y, &z) != 3)
    {
        printf("Incorrect values\n\n");
        printf("Input x y z: ");
        setbuf(stdin, NULL);
        scanf("%d %d %d", &x, &y, &z);
    }

    if (x < 1 || y < 1 || z < 1)
    {
        printf("One or more numbers are not natural\n\n");
        printf("Input x y z: ");
        setbuf(stdin, NULL);
        scanf("%d %d %d", &x, &y, &z);
    }

    if (check_numbers(x, y, z) || check_numbers(z, y, x) || check_numbers(z, x, y))
    {
        printf("Numbers are Pifagor triple\n");
        if (check_three_prime(x, y, z))
            printf("Pifagor triple is primitive\n");
        else
            printf("Pifagor triple is not primitive\n");
    }
    else
    {
        printf("Numbers aren't Pifagor triple\n");
    }

    return 0;
}

