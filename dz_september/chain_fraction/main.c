#include <stdio.h>
#include <math.h>
#include "../mylib/functionToGo.h"

int main()
{
    long long q, q0, denum =0, number = 0;
    //double a = saveIn();
    long long a = saveInInt();

    q = (long long)trunc(sqrt(a));

    if ((long long)a - q * q == 0)
    {
        printf("%lld", q);
        return 0;
    }

    printf("%lld ", q);
    q0 = (long long)(q + q) % (long long)(a - q * q) - q;
    denum = (long long)a - q * q;

    number = (-q0 + q) / denum;
    printf("%lld ", number);

    for (;2 * q != number;)
    {
        denum = (long long)(a - q0 * q0) / (long long)(denum);
        number = (-q0 + q) / denum;
        printf("%lld ", number);
        q0 = (long long)(q - q0) % (long long)(denum) - q;
    }
    return 0;
}
