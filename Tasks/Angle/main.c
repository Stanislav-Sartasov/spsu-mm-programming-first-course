#include <stdio.h>
#include <stdlib.h>
#include <math.h>

void input(double *a, double *b, double *c)
{
   int x;
   while (1)
   {
       printf("Enter 3 real numbers \n");
       x = scanf("%lf %lf %lf", a, b, c);
       if (x == 3)
       {
           char s;
           int fl = 0;
           s = getchar();
           while (s != '\0' && s != '\n')
           {
               if  (s != ' ')
               {
                   fl = 1;
               }
               s = getchar();
           }
           if  (fl == 0 && *a > 0 && *b > 0 && *c > 0)
           {
               break;
           }
       }
       else
       {
           char s;
           s = getchar();
           while (s != '\0' && s != '\n')
           {
               s = getchar();
           }
       }
       printf("Wrong input\n");
   }
}

void sort_two_numbers(double *a, double *b)
{
    if (*a > *b)
    {
        double t = *a;
        *a = *b;
        *b = t;
    }
}


double calculate_angle(double x, double y, double z)
{
    double pi = 3.14159265359;
    return acos((x * x + y * y - z * z) / (2 * y * x)) * 180 / pi;
}

void print_angle(int number_of_angle, double angle)
{
    double degrees, minute, seconds;
    minute = modf(angle, &degrees) * 60;
    seconds = round(modf(minute, &minute) * 60);
    if ((int) seconds == 60)
    {
        seconds = 0;
        minute++;
    }
    if (minute == 60)
    {
        minute = 0;
        degrees++;
    }
    printf("%d angle: %d degrees %d minutes %d seconds\n", number_of_angle, (int) degrees, (int) minute, (int) seconds);
}

int main()
{
    double x, y, z;
    for (;;)
    {
        input(&x, &y, &z);
        sort_two_numbers(&x, &y);
        sort_two_numbers(&y, &z);
        sort_two_numbers(&x, &y);
        if (x + y > z)
        {
            print_angle(1, calculate_angle(x, y, z));
            print_angle(2, calculate_angle(y, z, x));
            print_angle(3, calculate_angle(z, x, y));
            return 0;
        }
        printf("Triangle doesn't exist. ");
    }
    return 0;
}
