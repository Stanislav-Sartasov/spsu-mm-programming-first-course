#include <stdio.h>
#include <string.h>
#include <stdlib.h>

long long gcd(long long a, long long b)
{
    while (b)
    {
        long long c = a % b;
        a = b;
        b = c;
    }
    return a;
}

int main()
{
    char s[10];
    long long x, y, z;
    long long a[3];
    printf("Please enter natural numbers x, y, z (separated by space without commas) to find out if they are a Pythagorean triple\n");

    int fl = 0;
    while (!fl)
    {
        for (int i = 0; i < 3; i++)
        {
            scanf("%s", &s);
            a[i] = 0;
            for (int j = 0; s[j] != '\0'; j++)
            {
                if ('0' > s[j] || s[j] > '9')
                    fl = 1;
                a[i] = a[i] * 10 + (s[j] - '0');
            }
        }
        if (!fl && a[0] && a[1] && a[2])
            break;
        printf("Please, enter correct numbers\n");
        fl = 0;
    }

    x = a[0];
    y = a[1];
    z = a[2];

    if (x * x + y * y == z * z || x * x + z * z == y * y || y * y + z * z == x * x)
    {
        long long d = gcd(gcd(x, y), z);
        if (d == 1)
        {
            printf("This is a primitive Pythagorean triple");
            return 0;
        }
        printf("This is a Pythagorean triple");
        return 0;
    }

    printf("This is not a Pythagorean triple");
    return 0;
}
