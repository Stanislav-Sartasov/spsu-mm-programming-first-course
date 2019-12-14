#include <stdio.h>
#include <math.h>
#include "../mylib/functionToGo.h"

int main()
{
    long long q, q0, denum = 0, number = 0, a, period = 0;

    printf("This programm give's elements of continious fraction for entred sqrt(x), x > 0\n");
    do
    {
        printf("enter yor number(not full square of number): ");
        a = saveInInt();
    }
    while ((sqrt(a) - trunc(sqrt(a)) == 0) || (a <= 0));

    q = (long long)trunc(sqrt(a));

    printf("%lld ", q);
    q0 = (long long)(q + q) % (long long)(a - q * q) - q;
    denum = (long long)a - q * q;

    ++period;
    number = (- q0 + q) / denum;
    printf("%lld ", number);

    for (;2 * q != number;)
    {
        denum = (long long)(a - q0 * q0) / (long long)(denum);
        number = (- q0 + q) / denum;
        printf("%lld ", number);
        ++period;
        q0 = (long long)(q - q0) % (long long)(denum) - q;
    }
    printf("\nperiod is: %lld", period);
    return 0;
}
