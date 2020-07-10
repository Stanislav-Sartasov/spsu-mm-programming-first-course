#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>

int digital_root(int n)
{
	int x;
	while (n >= 10)
	{
		x = 0;
		while (n > 0)
		{
			x += n % 10;
			n /= 10;
		}
		n = x;
	}
	return n;
}

int main()
{
	int n = 1000000, sum = 0, i;
	//scanf("%d", &n);
	int *mdrs = (int*)malloc(n * sizeof(int));

	for (i = 2; i < n; i++) mdrs[i] = digital_root(i);

	for (i = 2; i < n; i++)
	{
		for (int j = 2; i * j < n && j <= i; j++)
		{
			if (mdrs[i * j] < mdrs[i] + mdrs[j]) mdrs[i * j] = mdrs[i] + mdrs[j];
		}
	}

	for (i = 2; i < n; i++) sum += mdrs[i];

	printf("%d\n", sum);

	system("pause");
	return 0;
}
