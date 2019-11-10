#include <stdio.h>
#include <stdlib.h>

typedef unsigned long long ull;

ull gcd(ull a, ull b)
{
    while (b != 0)
    {
        ull c = b;
        b = a % b;
        a = c;
    }
    return a;
}

void sort_two_numbers(ull *a, ull *b)
{
    if (*a > *b)
    {
        ull t = *a;
        *a = *b;
        *b = t;
    }
}

void input(ull *a, ull *b, ull *c)
{
    int x;
    while (1)
    {
        printf("Enter 3 integer numbers\n");
        x = scanf("%lld %lld %lld", a, b, c);
        if (x == 3)
        {
            char s;
            int fl = 0;
            s = getchar();
            while (s != '\0' && s != '\n')
            {
                if  (s != ' ')
                {
                    fl = 1;
                }
                s = getchar();
            }
            if  (fl == 0 && *a > 0 && *b > 0 && *c > 0)
            {
                break;
            }
        }
        else
        {
            char s;
            s = getchar();
            while (s != '\0' && s != '\n')
            {
                s = getchar();
            }
        }
        printf("Wrong input\n");
    }
}

int main()
{
    ull x, y, z;
    for (;;)
    {
        input(&x, &y, &z);
        sort_two_numbers(&x, &y);
        sort_two_numbers(&y, &z);
        sort_two_numbers(&x, &y);
        if (x + y > z)
        {
            if (x * x + y * y == z * z)
            {
                if (gcd(x, z) == 1)
                    printf("prime pythagoras triple");
                else
                    printf("pythagoras triple. Not prime");
            }
            else
                printf("isn't pythagoras triple");
            return 0;
        }
        printf("Triangle doesn't exist. ");
    }
    return 0;
}
