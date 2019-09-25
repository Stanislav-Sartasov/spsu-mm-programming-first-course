#include <stdio.h>

void in_int(char* message, int* variable_int)
{
	for (;;)
	{
		char c;
		printf("%s", message);
		do
		{								//handling spaces at the beginning of a line
			c = getchar();
		} while (c == ' ' || c == '\t');

		if (c == '\n')
			continue;		//handling empty input

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
				continue;
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
			continue;
		}
		*variable_int *= minus;
		return;
	}
}

void in_double(char* message, double* variable_double)
{
	for (;;)
	{
		char c;
		printf("%s", message);

		do
		{
			c = getchar();
		} while (c == ' ' || c == '\t');

		if (c == '\n')
			continue;

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
				continue;
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
			continue;
		}

		*variable_double *= minus;
		return;
	}
}