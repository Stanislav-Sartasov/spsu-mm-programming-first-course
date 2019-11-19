#include <stdio.h>
#include <math.h>
#include "functionToGo.h"

#define pi 3.14159265358979

int minute(double d);
int second(double d);
void getAngle(double a, double b, double c);

int main()
{
    double a, b, c;
    //scanf("%lf%lf%lf",&a,&b,&c);
    changeAgainInputText("try to put \"double\" type of number");
    printf("Input 3 sides of triangle:\n");
    a = saveInDouble();
    b = saveInDouble();
    c = saveInDouble();
    if((a <= 0) || (b <= 0) || (c <= 0))
    {
        printf("impossible");
        return 0;
    }
    //printf("%f %f %f",a,b,c);
    if((a < b + c) && (b < a + c) && (c < a + b) && a > 0 && b > 0 && c > 0)
    {
        printf("possible\n");
        getAngle(a, b, c);
    }
    else
        printf("impossible");
    return 0;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////

void getAngle(double a, double b, double c)
{
    double p = (a + b + c) / 2, s;
    s = sqrt(p * (p - a) * (p - b) * (p - c));

    p = 2.0 * s / (a * b);
    printf("degrees: %d, minutes: %d, seconds: %d\n", (int)trunc(asin(p) * 180 / pi), minute(asin(p) * 180 / pi), second(asin(p) * 180 / pi));
    p = 2.0 * s / (b * c);
    printf("degrees: %d, minutes: %d, seconds: %d\n", (int)trunc(asin(p) * 180 / pi), minute(asin(p) * 180 / pi), second(asin(p) * 180 / pi));
    p = 2.0 * s / (a * c);
    printf("degrees: %d, minutes: %d, seconds: %d\n", (int)trunc(asin(p) * 180 / pi), minute(asin(p) * 180 / pi), second(asin(p) * 180 / pi));
}

int minute(double d)
{
    return (int)trunc((d - trunc(d)) * 60);
}

int second(double d)
{
    return (int)trunc((d * 60 - trunc(d * 60)) * 60);
}
