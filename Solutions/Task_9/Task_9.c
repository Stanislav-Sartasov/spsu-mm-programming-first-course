#include <stdio.h>
#include <stdlib.h>

#include "my_malloc.h"

int main()
{
	int** arr = (int**)my_malloc(sizeof(int*));
	int** code;
	arr[0] = (int*)my_malloc(sizeof(int) * 100);
	arr[0][0] = 0;
	int i = 1;
	while (arr[i - 1])
	{
		code = (int**)my_realloc(arr, sizeof(int*) * ++i);
		if (code)
			arr = code;
		else
		{
			i--;
			break;
		}
		arr[i - 1] = (int*)my_malloc(sizeof(int) * 100);
		arr[i - 1][0] = i - 1;
	}

	char flag = 1;
	for (i; i > 0; i--)
	{
		flag *= (arr[i - 1][0] == i - 1);
	}
	if (flag)
		printf("it's ok");
	else
		printf("not ok");
}