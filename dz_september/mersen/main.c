#include <stdio.h>
#include <math.h>

int main()
{
    int flag = 1;
    long long x;
    for (int i = 1; i<32; ++i)
    {
        flag = 1;
        x = (long long)(pow(2, i)) - 1;
        for (int k = 2; k < sqrt(x); ++k)
        {
            if  (x % k  ==  0)
                {
                    flag = 0; break;
                }
        }

        if ((flag) && (x !=  1))
            printf("%lld\n",x);
    }
    return 0;
}
