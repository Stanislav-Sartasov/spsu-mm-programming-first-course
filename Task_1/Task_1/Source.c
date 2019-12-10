#include <stdio.h>
#include <string.h>
#include <math.h>

void perevod(long long x, int size, int sign_bit)
{
	int i = 0;
	int j = 0;
	int *mas = (int*)calloc(size , sizeof(int));
	
	if ( sign_bit != -1 )
	{
		printf("%d", sign_bit);
	}

	while (x != 0)
	{
		if ( x % 2 == 0 )
		{
			mas[i] = 0;
			x = x / 2;
		}
		else
		{
			mas[i] = 1;
			x = x / 2;
		}
		i++;
	}
	for ( i = i - 1 ; i >= 0 ; i-- )
	{
		printf("%d", mas[i]);
	}

	free(mas);
	printf("\n");
}



int main()
{
	char surmame[] = "shemyakin";
	char name[] = "andrey";
	char patronymic[] = "aleksandrovich";

	long p = strlen(surmame) * strlen(name) * strlen(patronymic);
	printf_s("composition is %d\n", p);
	
	long long negative = pow(2, 32) - p;
	printf_s("negative 32-bit: \n");
	perevod(negative, sizeof(int) * 8, -1);
	

	printf_s("positive floating point number of unit precision according to the IEEE 754 standarda: \n");
	float fp = (float)p;
	int xf = &fp;
	perevod( *(int*)xf, (sizeof(float) * 8 - 1), 0);

	printf_s("negative double precision floating point number according to IEEE 754 standard: \n");
	double dp = (double)p;
	long long  xd = &dp;
	perevod( *(long long*)xd, (sizeof(double) * 8 - 1), 1);
	return 0;
}
