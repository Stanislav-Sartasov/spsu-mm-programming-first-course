#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

double angle_value(double a, double b, double c)
{
	return acos((a * a + b * b - c * c) / (2 * a * b));
}

int correct_input(char str[])
{
	char digits[11] = "0123456789";
	int digit_true;
	int point_position = -1;
	
	for (int i = 0; i < strlen(str); i++)
	{
		
		if (str[i] == '.')
		{
			point_position = i;
			break;
		}
	}

	if ((point_position == -1) && (str[0] == '0')) return 1;
	else if ((point_position == 0) || (point_position == strlen(str) - 1)) return 1;
	else if ((str[0] == '0') && (point_position > 1)) return 1;
	else
	{
		for (int l = 0; l < strlen(str); l++)
		{
			digit_true = 0;	
			if (l == point_position) continue;
			for (int n = 0; (n < 10); n++)
			{				
				if (str[l] == digits[n]) digit_true = 1;
			}
			if (digit_true == 0) return 1;
		}
	}
	return 0;
}

int main()
{
	double sides[3];
	char s[21];
	double degrees, minutes, seconds;
	double pi = 3.14159265;

	
	for (int i = 0; i < 3; i++)
	{
		int j = 0;
		while (j != 1)
		{
			printf("Side number %d = ", i + 1);
			(void)scanf("%s", s);
			if (correct_input(s) != 0)  printf("Incorrect input.\n");
			else
			{
				j = 1;
				sides[i] = atof(s);
			}
		}
	}
	
	if ((sides[0] > sides[2] - sides[1]) && (sides[2] > sides[0] - sides[1]) && (sides[0] > sides[1] - sides[2]))
	{
		for (int i = 0; i < 3; i++)
		{
			double value = angle_value(sides[i % 3], sides[(i + 1) % 3], sides[(i + 2) % 3]);
			minutes = modf((value * 180) / pi, &degrees);
			seconds = modf((minutes * 60), &minutes);
			seconds = floor(seconds * 60);
			printf("%f %f %f\n", degrees, minutes, seconds);
		}
	}

	else printf("A triangle with such sides does not exist.");
	return 0;
}