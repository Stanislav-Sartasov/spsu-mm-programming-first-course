#include <stdio.h>
#include <math.h>

int main()
{
	int i, d = 1;

	printf("1\n");
	for (i = 1; i < 31; i++)
	{
		d = (d << 1) | 1;
		int x;
		for (x = 2; x <= (float)sqrtf(d) && d % x != 0; x++);
		if (x > sqrtf(d))
			printf("%d\n", d);
	}
}