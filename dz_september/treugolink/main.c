#include <stdio.h>
#include <math.h>

#define pi 3.14159265358979

int main()
{
    double a,b,c;
    scanf("%lf%lf%lf",&a,&b,&c);
    //printf("%f %f %f",a,b,c);
    if((a<b+c) && (b<a+c) && (c<a+b) && a>0 && b>0 && c>0)
    {
        double p=(a+b+c)/2, s;
        s=sqrt(p*(p-a)*(p-b)*(p-c)); //printf("%f",s);
        p=2.0*s/(a*b); printf("possible: %f ",asin(p)*180/pi);
        p=2.0*s/(b*c); printf("%f ",asin(p)*180/pi);
        p=2.0*s/(a*c); printf("%f",asin(p)*180/pi);
    }
    else
        printf("impossible");
    return 0;
}
