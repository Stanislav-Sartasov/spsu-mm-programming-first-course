#define _CRT_SECURE_NO_WARNINGS

#include <stdio.h> 
#include <stdlib.h>

int dv(int a1, int a2)
{
	if (a2 == 0)
		return a1;
	return dv(a2, a1 % a2);
}

void ins(int *p, int *w, int *q)
{
	char sres[15]; // inputed string
	int res[3]; // number array
	short check = 0;

	while (check == 0)
	{
		for (int k = 0; k <= 2; k++)
		{ 
			scanf("%s", &sres);
			res[k] = 0;
			for (int l = 0; sres[l] != '\0'; l++)
			{
				if (!((sres[l] >= '0') && (sres[l] <= '9')))
					check = 1;
				res[k] = res[k] * 10 + (sres[l] - '0');
			} 
		}
		for (int k = 0; k <= 2; k++)
		{
			if (res[k] <= 0)
				check = 1;
		}
		if (check == 0)
			break;
		else
		{
			printf("Incorrect input, please try again.\n");
			check = 0;
		}
	}
	*p = res[0];
	*w = res[1];
	*q = res[2];
}

int main()
{
	int x = 0;
	int y = 0; 
	int z = 0;

	printf("The program recognizes pythagorean triplets and tests them for primitiveness.\n");
	printf("Input three natural numbers: ");

	ins(&x, &y, &z);

	int max = x;
	x = max(max(x, y), z);

	if (x != max)
		if (y == x)
			y = max;
		else
			z = max;

	if (x * x != y * y + z * z)
		printf("The numbers don't make up the pythagorean triplet.");
	else
		if (dv(x, y) == dv(z, y) == dv(x, z) == 1)
			printf("The numbers make up the primitive pythagorean triplet.");
		else
			printf("The numbers make up the non-primitive pythagorean triplet.");
	return 0;
}
