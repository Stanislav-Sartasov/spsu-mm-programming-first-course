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

int gcd(int a, int b)
{
    int buf;
    while (b > 0)
    {
        buf = a % b;
        a = b;
        b = buf;
    }
    return a;
}

void input(int* x, int* y, int* z)
{
    while (1)
    {
        printf("Input x y z: ");
        if (scanf("%d %d %d", x, y, z) != 3)
        {
            printf("Incorrect values\n\n");
            int c;
            while ((c = getchar()) != '\n' && c != EOF);
            continue;
        }

        if (*x < 1 || *y < 1 || *z < 1)
        {
            printf("One or more numbers are not natural\n\n");
            int c;
            while ((c = getchar()) != '\n' && c != EOF);
            continue;
        }
        break;
    }
}

int main()
{
    setbuf(stdout, NULL);

    int x, y, z;

    input(&x, &y, &z);

    if (check_numbers(x, y, z) || check_numbers(z, y, x) || check_numbers(z, x, y))
    {
        printf("Numbers are Pifagor triple\n");
        if (gcd(gcd(x, y), z) == 1)
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

