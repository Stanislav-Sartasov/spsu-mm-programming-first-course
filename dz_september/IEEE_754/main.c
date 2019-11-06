#include <stdio.h>
#include<string.h>

void showTwo(int a);
void showIEEEx64(int a);

int main()
{
    char name[80] = "Makar", surname[80] = "Pelogeiko", fathern[80] = "Andreevich";
    int length = strlen(name) * strlen(surname) * strlen(fathern);

    showTwo(length);
    showIEEEx64(length);
    return 0;
}

void showIEEEx64(int a)
{
    long long b = 18014398509481983 - a + 1;
    int c[54], e[11], f = 53, exp = 0, expk = 10;
    for (register int i = 0; i < 11; ++i)
        e[i] = 0;
    for (register int i = 0; i < 54; ++i)
        c[i] = 0;
    while (b && (f + 1))
    {
        c[f--] = b % 2;
        b /= 2;
    }
    ++f;
    exp = 127 + 54 - f;
    while (exp && (expk + 1))
    {
        e[expk--] = exp % 2;
        exp /= 2;
    }

    printf("IEE 754x64: %d-", c[0]);
    for (register int i = 0; i < 11; ++i)
        printf("%d",e[i]);
    printf("-");
    for (register int i = 2; i < 54; ++i)
        printf("%d", c[i]);
    printf("\n");
}

void showTwo(int a)
{
   long long b = 4294967295 - a + 1;
    int c[32], i = 31, exp = 0, e[8];
    for (register int i = 0; i < 8; ++i)
        e[i] = 0;
    for (register int i = 0; i < 32; ++i)
        c[i] = 0;
    while (b && (i + 1))
    {
        c[i--] = b % 2;
        b /= 2;
    }
    ++i;
    printf("inverted code+1: ");
    for (register int k = 0; k < 32; ++k)
        printf("%d",c[k]);

    //IEEE 754 format 32
    printf("\n");
    i = 31; b = a;
    for (register int i = 0; i < 32; ++i)
        c[i] = 0;
    while (a && ( i + 1))
    {
        c[i--] = a % 2;
        a /= 2;
    }

    for (register int k = 0; k < 32; ++k)
    {
        if (c[k] == 1)
        {
            i = k + 1; exp = 127 + 32 - i;
            printf("IEEE 754: 0-");
            int p = 7;
            while (exp && (p + 1))
            {
                e[p--] = exp % 2;
                exp /= 2;
            }
            for (int h = 0; h < 8; ++h)
                printf("%d",e[h]);

            printf("-");
            for (register int h = i; h < 32; ++h)
                printf("%d",c[h]);
            for (register int h = 32 - i; h < 24; ++h)
                printf("0");
            break;
        }
        if (k == 31)
            printf("IEEE 754: 0-01111111-00000000000000000000000000000000");
    }
    printf("\n");

}
