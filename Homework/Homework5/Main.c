#include <stdio.h>
#include <stdint.h>
#include <math.h>

#define MAX     (100005)

typedef uint32_t u32;

int readNumberStdin(int maxDigits, u32* ret) 
{
    int digits = 0;
    int c = 0;
    (*ret) = 0;

    while ((c = getchar()) != '\n') 
	{
        if (!(c >= '0' && c <= '9') || (++digits > maxDigits)) 
		{
            while (getchar() != '\n');
            return 1;
        }
        (*ret) = (*ret) * 10 + (c - '0');
    }

    return !digits;
}

int isPerfectSquare(u32 n) 
{
    u32 r = floor(sqrt(n));
    return (r * r) == n;
}

int main()
{

    const char description[] = "This program finds the terms and period of continue"
                               " fraction of sqrt(n)\n\n";

    printf("%s", description);

    u32 n;
    do 
	{
        printf("enter a non-perfect square number(max digits=8): ");
    } while(readNumberStdin(8, &n) || (n == 0) || isPerfectSquare(n));

    u32 r = floor(sqrt(n));
    u32 a = r;
    u32 p = 0;
    u32 q = 1;
    u32 i = 0;

    printf("\nsequence: [%d; ", r);
    do 
	{    
        p = a * q - p;
        q = (n - p * p) / q;
        a = (r + p) / q;
        ++i;
        printf("%d, ", a);
    } while(q != 1);
    printf("...]\nperiod: %d\n", i);
}



