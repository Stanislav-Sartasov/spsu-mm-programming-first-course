#define _CRT_SECURE_NO_WARNINGS

#include <stdio.h>
#include <math.h>

#define RADIAN 57.295779513


double find_age(double x, double y, double z)
{
	return acos((( y * y + z * z - x * x ) / ( 2 * y * z))) * RADIAN;
}

void ride_age(double age)
{	
	int d = (int)age;
	printf("degree : %d\n", d);

	int m = (int)((age - d) * 60);
	printf("minute : %d\n", m);
	
	int s = (int)(((( age - d ) * 60) - m ) * 60);
	printf("second : %d\n", s);
}


int main()
{
	int n = 0;
	char eol = 0;
	double x = 0; double y = 0; double z = 0;
	double age1; double age2; double age3;


	while (n == 0)
	{
		int xscan = scanf("%lf", &x);
		int yscan = scanf("%lf", &y);
		int zscan = scanf("%lf%c", &z, &eol);

		if (xscan && yscan && zscan && eol == '\n')
		{
			if (x * y > 0 && y * z > 0 && x * z > 0)
			{
				n = 1;
				if ((x + y - z > 0.00001) && (x + z - y > 0.00001) && (z + y - x > 0.00001))
				{
					printf("You can build a triangle\n");

					printf("One age : \n");
					ride_age(find_age(x, y, z));

					printf("Second age : \n");
					ride_age(find_age(y, x, z));

					printf("Third age : \n");
					ride_age(find_age(z, y, x));


				}
				else
				{
					printf("You can't build a triangle\n");
				}
			}
			else
			{
				printf("Input error. Try again\n");
			}
		}
		else
		{
			printf("Input error. Try again\n");
			scanf("%*[^\n]");
		}
	}

	return 0;

}