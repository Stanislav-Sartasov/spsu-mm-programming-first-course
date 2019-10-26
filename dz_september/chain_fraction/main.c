#include <stdio.h>
#include <math.h>
#include "../mylib/functionToGo.h"

int main()
{
    long long q;
    double b, r, a = savein();

    r = reminder(a, sqrt(a));
    q = wholep(a, sqrt(a));
    printf("%lld\n",q);
    a=sqrt(a);
    b=r;
    for(;b>0.0002;)
    {
        r = reminder(a, b);
        q = wholep(a, b);
        printf("%lld\n",q);
        a=b;
        b=r;
    }

    return 0;
}
