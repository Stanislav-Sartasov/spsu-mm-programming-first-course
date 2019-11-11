#include <stdio.h>
#include <stdlib.h>

#define base 4294967296		//2^32

typedef struct
{
	unsigned int* num;
	int size;
} long_num;

void set_num(long_num* num, int index, unsigned int value)
{
	if (index >= num->size)
	{
		if (num->size)
			num->num = (unsigned int*)realloc(num->num, sizeof(unsigned int) * (index + 1));
		else
			num->num = (unsigned int*)malloc(sizeof(unsigned int) * (index + 1));
		num->size = index + 1;
	}
	num->num[index] = value;
}

unsigned int get_num(long_num* num, int index)
{
	if (index >= num->size)
		return 0;
	return num->num[index];
}

void free_num(long_num* num)
{
	if (num->size)
		free(num->num);
	num->size = 0;	
}

void reset_num(long_num* num, int new_size)
{
	if (num->size == new_size)
		return;
	if (num->size > 0)
		free_num(num);
	num->size = new_size;
	num->num = (unsigned int*)malloc(sizeof(unsigned int) * new_size);
}

void fill_num(long_num* num, int size, unsigned int value)
{
	if (!size)
		return;
	if (size >= num->size)
	{
		if (num->size)
			free(num->num);
		num->num = (unsigned int*)malloc(sizeof(unsigned int) * size);
		num->size = size;
	}
	for (int i = 0; i < size; i++)
		num->num[i] = value;
}

void assign_num(long_num* to, long_num* from)
{
	reset_num(to, from->size);
	for (int i = 0; i < from->size; i++)
		to->num[i] = from->num[i];
}

void null_cut_num(long_num* num)
{
	int i;
	for (i = num->size - 1; (num->num[i] == 0) && (i >= 0); i--);
	if (num->size != i + 1)
	{
		num->num = (unsigned int*)realloc(num->num, sizeof(unsigned int) * (i + 1));
		num->size = i + 1;
	}
}

void print_num_hex(long_num* out)
{
	char null_flag = 1;
	for (int i = out->size - 1; i >= 0; i--)
	{
		unsigned int f = out->num[i];
		int g;
		for (int j = 7; j >= 0; j--)
		{
			g = (f & (15 << (4 * j))) >> (4 * j);
			if (null_flag)
			{
				if (g == 0)
					continue;
				null_flag = 0;
			}
			printf("%x", g);
		}
	}
}

void mtpi(long_num* a, unsigned int b, long_num* out, int shift)
{	
	unsigned long long over = 0;
	reset_num(out, a->size + shift);
	fill_num(out, shift, 0);
	for (int i = 0; i < a->size; i++)
	{
		over = (unsigned long long)a->num[i] * (unsigned long long)b + over / base;
		out->num[i + shift] = (unsigned int)(over % base);
	}
	if (over / base)
		set_num(out, a->size + shift, (unsigned int)(over / base));
}

void sum(long_num* a, long_num* b, long_num* out)
{
	unsigned long long over = 0;
	int max_size;
	if (a->size > b->size)
		max_size = a->size;
	else
		max_size = b->size;
	if (a != out)
		reset_num(out, max_size);
	else if (max_size != a->size)
		out->num = (unsigned int*)realloc(out->num, sizeof(unsigned int) * max_size);		
	for (int i = 0; i < max_size; i++)
	{
		over = (unsigned long long)get_num(a, i) + (unsigned long long)get_num(b, i) + over / base;
		out->num[i] = (unsigned int)(over % base);
	}
	if (max_size != a->size)
		out->size = max_size;
	if (over / base)
		set_num(out, max_size, (unsigned int)(over / base));
}

void mtp(long_num* a, long_num* b, long_num* out)
{
	int max_size = (a->size) + (b->size);
	fill_num(out, max_size, 0);
	long_num interm;
	interm.size = 0;
	for (int i = 0; i < b->size; i++)
	{
		mtpi(a, b->num[i], &interm, i);
		sum(out, &interm, out);
	}
	null_cut_num(out);
	free_num(&interm);
}

int main()
{
	long_num a;
	long_num b;
	long_num c;
	a.size = 0;
	b.size = 0;
	c.size = 0;
	int exp = 5000;
	set_num(&a, 0, 3);
	if (exp & 1)
		set_num(&c, 0, 3);
	else
		set_num(&c, 0, 1);
	for (int i = 2; i <= exp; i <<= 1)
	{
		mtp(&a, &a, &b);
		assign_num(&a, &b);
		if (exp & i)
		{
			mtp(&c, &a, &b);
			assign_num(&c, &b);
		}
	}
	print_num_hex(&c);
}