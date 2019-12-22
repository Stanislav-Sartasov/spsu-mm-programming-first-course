#include <stdio.h>

int readNumberStdin(int maxDigits, int* ret) 
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

void getNumber(const char* prompt, int* number) 
{

    do 
	{
        printf("%s", prompt);
    } while(readNumberStdin(8, number));
}

/*
	Checks if the triple passed is a Pythagorean Triple.
	@param a:		The first natural numbers.
	@param b:		The second natural numbers.
	@param c:		The third natural numbers.
	@return bool:	true if the triple is a Pythagorean Triple, false if it's not.
*/
int isPythagoreanTriple(int a, int b, int c) 
{
    return ((1LL * a * a) + (1LL * b * b) == (1LL * c * c));
}

/*
	Calculates the greatest common divisior of two integers.
	@param a:		The first natural number.
	@param b:		The second natural number.
	@return int:	The greatest common divisor.
*/
int gcd(int a, int b) 
{
    if(b == 0)
        return a;
    a %= b;
    return gcd(b, a);
}

int main() 
{
    int x, y, z;
    getNumber("enter number 1: ", &x);
    getNumber("enter number 2: ", &y);
    getNumber("enter number 3: ", &z);
    printf("\n");

    if ((x && y && z) && (isPythagoreanTriple(x, y, z) ||
        isPythagoreanTriple(x, z, y) ||
        isPythagoreanTriple(y, z, x))) 
	{
        printf("(%d, %d, %d) is a Pythagorean triple\n", x, y, z);

        if (gcd(x, gcd(y, z)) == 1)
            printf("(%d, %d, %d) is also a primitive\n", x, y, z);
        else
            printf("(%d, %d, %d) is not a primitive\n", x, y, z);

    } else 
	{
        printf("(%d, %d, %d) is not a Pythagorean triple\n", x, y, z);
    }
}