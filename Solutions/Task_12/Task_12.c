#include <stdio.h>
#include <stdlib.h>

#define base 4294967296
#define size 248

void mtpi(unsigned int* a, unsigned int b, unsigned int* out, int shift)
{
	for (int i = 0; i < shift; i++)
		out[i] = 0;
	unsigned long long over = 0;
	for (int i = 0; (i + shift) < size; i++)
	{
		over = (unsigned long long)a[i] * (unsigned long long)b + over / base;
		out[i + shift] = over % base;
	}
}

void sum(unsigned int* a, unsigned int* b, unsigned int* out)
{
	unsigned long long over = 0;
	for (int i = 0; i < size; i++)
	{
		over = (unsigned long long)a[i] + (unsigned long long)b[i] + over / base;
		out[i] = over % base;
	}
}

void mtp(unsigned int* a, unsigned int* b, unsigned int* out)
{
	for (int i = 0; i < size; i++)
		out[i] = 0;
	unsigned int* interm[size] = { 0 };
	for (int i = 0; i < size; i++)
	{
		mtpi(a, b[i], interm, i);
		sum(out, interm, out);
	}
}

int main()
{
	unsigned int* a[size] = { 0 };
	unsigned int* b[size] = { 0 };
	unsigned int* c[size] = { 0 };
	c[0] = 1;
	a[0] = 3;
	for (int i = 2; i <= 5000; i <<= 1)
	{
		mtp(a, a, b);
		for (int j = 0; j < size; j++)
			a[j] = b[j];
		if (5000 & i)
		{
			mtp(c, a, b);
			for (int j = 0; j < size; j++)
				c[j] = b[j];
		}
	}
	for (int i = size - 1; i >= 0; i--)
	{
		unsigned int f = c[i];
		for (int j = 7; j >= 0; j--)
			printf("%x", (f & (15 << (4 * j))) >> (4 * j));
	}
}