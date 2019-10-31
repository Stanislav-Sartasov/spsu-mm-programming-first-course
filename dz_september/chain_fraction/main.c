#include <stdio.h>
#include <math.h>
#include "../mylib/functionToGo.h"

int main()
{
    long long q, q0;
    double b, r, a = savein();

    r = reminder(a, sqrt(a));
    q = wholep(a, sqrt(a));
    printf("%lld\n",q);
    a=sqrt(a);
    b=r;
    q0=q;
    do
    {
        r = reminder(a, b);
        q = wholep(a, b);
        printf("%lld\n",q);
        a=b;
        b=r;
    }
    while(2*q0 != q);

    return 0;
}
