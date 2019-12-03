#define _CRT_SECURE_NO_WARNINGS

#include <stdio.h>
#include <math.h>

#define RADIAN 57.295779513


float find_age(float x, float y, float z)
{
	return acos((( y * y + z * z - x * x ) / ( 2 * y * z))) * RADIAN;
}

void ride_age(float age)
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
	float x = 0; float y = 0; float z = 0;
	float eps = 0.00001;


	printf("The program determines whether it is possible to build a triangle on three sides and displays its angles\n");
	printf("Enter three numbers\n");

	while (n == 0)
	{
		int xscan = scanf("%f", &x);
		int yscan = scanf("%f", &y);
		int zscan = scanf("%f%c", &z, &eol);

		if (xscan && yscan && zscan && eol == '\n')
		{
			if (x * y > 0 && y * z > 0 && x * z > 0)
			{
				n = 1;
				if ((x + y - z > eps) && (x + z -y > eps) && (z + y - x > eps))
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