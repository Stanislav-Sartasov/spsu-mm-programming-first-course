#include <stdio.h>

#define MAX  (999999)

int mdrs[MAX + 1];

/*
	Calculates the digital root.
	@param x:	The number that needs to be calculated.
*/
int digitalRoot(int x) 
{
    return ((x - 1) % 9) + 1;
}

int main() 
{
    const char description[] = "This programs calculate's the sum of all MDRS(n)"
                               " for n in interval [2, 999999]\n\n";

    printf("%s", description);

    for(int i = 2; i <= MAX; ++i)
        mdrs[i] = digitalRoot(i);

    for (int i = 2; i <= MAX; ++i) 
	{
        for (int j = 2; i * j <= MAX && j <= i; ++j) 
		{
            if (mdrs[i * j] < mdrs[i] + mdrs[j])
                mdrs[i * j] = mdrs[i] + mdrs[j];
        }
    }

    unsigned long long res = 0;
    for (int i = 2; i <= MAX; ++i)
        res += mdrs[i];

    printf("solution: %lld\n", res);

    return 0;
}
