#include "stdio.h"
#include "stdlib.h"
#include "math.h"

#define PI 3.1415

int check_triangular(float a, float b, float c)
{
    if ((a + b > c) && (a + c > b) && (b + c > a))
        return 1;
    return 0;
}

float find_angle(float a, float b, float c)
{
    float angle = acos((a * a + b * b - c * c) / (2 * a * b));

    angle = 180 * angle / PI;

    return angle;
}

void angle_min_sec(float angle)
{
    int minutes = ((int)(angle * 60)) % 60;
    int seconds = ((int)(angle * 360)) % 60;

    printf("Grad: %-5d Minutes: %-5d Seconds: %-5d\n", (int) angle, minutes, seconds);
}

void input(float* a, float* b, float* c)
{
    while (1)
    {
        printf("Input a, b, c: ");
        if (scanf("%f %f %f", a, b, c) != 3)
        {
            printf("Incorrect values\n\n");
            fflush(stdin);
            continue;
        }
        break;
    }
}

int main()
{
    setbuf(stdout, NULL);

    float a, b, c;

    input(&a, &b, &c);

    if (!check_triangular(a, b, c))
    {
        printf("Triangular does not exist\n");
        return 0;
    }

    printf("Triangular exists\n");

    float first_angle = find_angle(a, b, c);
    float second_angle = find_angle(b, c, a);
    float third_angle = find_angle(a, c, b);

    angle_min_sec(first_angle);
    angle_min_sec(second_angle);
    angle_min_sec(third_angle);

    return 0;
}

