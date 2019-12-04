#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>
#include<math.h>
#include"IDK.h"



int main()
{
	init(n, ptr);

	struct hash a, b, c, d, e, f, g;
	a.key = 1; a.value = 20;
	b.key = 1; b.value = 11;
	c.key = 2; c.value = 92;
	d.key = 2; d.value = 14;
	e.key = -14; e.value = 9;
	f.key = 387654; f.value = 41;
	g.key = 1; g.value = 31;

	add(a);
	add(b);

	find(a);

	printf("\n");

	add(c);
	add(d);
	add(e);
	add(f);
	add(g);

	find(a);

	delite(a);

	printf("\n");

	find(a);

	myFree();

	system("pause");
	return 0;
}