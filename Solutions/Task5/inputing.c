#include <stdio.h>

char in_int_back(char* message, int* variable_int)	//return 0 if it's ok, else return -1
{
	char c;
	printf("%s", message);
	do
	{								//handling spaces at the beginning of a line
		c = getchar();
	} while (c == ' ' || c == '\t');

	if (c == '\n')
		return -1;		//handling empty input

	char minus;						//handling negative input
	if (c == '-')
	{
		c = getchar();
		minus = -1;
	}
	else
		minus = 1;

	while (c == ' ' || c == '\t')
		c = getchar();

	*variable_int = 0;

	while (c >= '0' && c <= '9')
	{
		if (*variable_int * 10 + c - '0' < 0)		//handling int overflow
		{
			while (getchar() != '\n');
			printf("invalid input, input is too big\n");
			return -1;
		}
		*variable_int = *variable_int * 10 + c - '0';
		c = getchar();
	}

	while (c == ' ' || c == '\t')		//post processing and validation of input
		c = getchar();

	if (c != '\n')
	{
		while (getchar() != '\n');
		printf("invalid input\n");
		return -1;
	}
	*variable_int *= minus;
	return 0;
}

void in_int(char* message, int* variable_int)	//function for int input
{
	while (in_int_back(message, variable_int));
}

char in_double_back(char* message, double* variable_double)	//return 0 if it's ok, else return -1
{
	char c;
	printf("%s", message);

	do
	{
		c = getchar();
	} while (c == ' ' || c == '\t');

	if (c == '\n')
		return -1;

	char minus;
	if (c == '-')
	{
		c = getchar();
		minus = -1;
	}
	else
		minus = 1;

	while (c == ' ' || c == '\t')
		c = getchar();

	*variable_double = 0.0;
	char whole_part_flag = (c >= '0' && c <= '9') ? 1 : 0;

	while (c >= '0' && c <= '9')
	{
		*variable_double = *variable_double * 10 + c - '0';
		c = getchar();
	}

	while (c == ' ' || c == '\t')
		c = getchar();

	if ((c == '.') && whole_part_flag)
	{
		c = getchar();

		while (c == ' ' || c == '\t')
			c = getchar();

		if (c < '0' || c > '9')
		{
			if (c != '\n')
				while (getchar() != '\n');
			printf("invalid input\n");
			return -1;
		}
		for (int i = 10; c >= '0' && c <= '9'; i *= 10)
		{
			*variable_double = *variable_double + ((double)c - (double)'0') / (double)i;
			c = getchar();
		}
	}

	while (c == ' ' || c == '\t')
		c = getchar();

	if (c != '\n')
	{
		while (getchar() != '\n');
		printf("invalid input\n");
		return -1;
	}

	*variable_double *= minus;
	return 0;
}

void in_double(char* message, double* variable_double)	//function for double input
{
	while (in_double_back(message, variable_double));
}