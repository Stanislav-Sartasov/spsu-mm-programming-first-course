#include <stdio.h>
#include <stdint.h>

#define MAX_DIGITS 5005

typedef struct 
{
    // buffer[0] = stores the last digit
    // buffer[1] = stores the second last digit
    // ....
    // buffer[n] = stores the first digit
    // each element of buffer is in range [1, 15]
	uint8_t buffer[MAX_DIGITS];
}
BigInteger;

// increments a digit at index, by inc in BigInteger num.
void incrementDigit(BigInteger* num, int index, int inc)
{
	int carry = inc;
	while (carry) 
	{
		int v = num->buffer[index] + carry;
		num->buffer[index++] = (v % 16);
		carry = (v / 16);
	}
}

// counts the number of digits in a BigInteger
int digitCount(BigInteger const* num) 
{
	for(int i = MAX_DIGITS - 1; i >= 0; --i) 
	{
		if(num->buffer[i] != 0)
			return i + 1;
    }
	return 0;
}

void print(BigInteger const* num) 
{
	char hex[] = "0123456789abcdef";
	int found = 0;
	for(int i = MAX_DIGITS - 1; i >= 0; --i) 
	{
		if (num->buffer[i] == 0 && found == 0)
			continue;
		found = 1;
		printf("%c", hex[num->buffer[i]]);
    }

	if (found == 0)
		printf("0");

	printf("\n");
}

// create a new BitInteger object, with initial value
BigInteger create(unsigned int initial)
{
	BigInteger res;
	int index = 0;
	while(initial) 
	{
		res.buffer[index++] = (initial % 16);
		initial /= 16;
	}
	while (index < MAX_DIGITS)
		res.buffer[index++] = 0;
	return res;
}

BigInteger multiply(BigInteger const* x, BigInteger const* y)
{
	BigInteger res = create(0);
	int carry = 0;

	int cx = digitCount(x);
	int cy = digitCount(y);

	for(int i = 0; i < cy; ++i) 
	{
		int index = i;
		for(int j = 0; j < cx; ++j)
		{
			int v = y->buffer[i] * x->buffer[j] + carry;
			incrementDigit(&res, index++, v % 16);
			carry = (v / 16);
		}
		incrementDigit(&res, index, carry);
		carry = 0;
	}
	return res;
}

// calculate x^y
// x^y = x^(y/2) * x^(y/2) * (y%2)?x: 1
BigInteger exponent(unsigned int x, unsigned int y)
{
	if (y == 0)
		return create(1);

	if (y == 1)
		return create(x);

	BigInteger half = exponent(x, y >> 1u);

	if (y & 1u) 
	{
		BigInteger tmp1 = create(x);
		BigInteger tmp2 = multiply(&half, &half);
		return multiply(&tmp1, &tmp2);
	}
	else
		return multiply(&half, &half);
}

int main() 
{
	const char* description =	"This program print's the solution of 3 ^ 10000"
								" in hexadecimal\n\n";

	printf("%s", description);
	BigInteger res = exponent(3, 10000);
	print(&res);
}