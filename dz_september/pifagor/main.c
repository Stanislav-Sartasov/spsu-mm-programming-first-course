#include <stdio.h>
#include "functionToGo.h"

int gcd(int x, int y);
int main()
{
    int x, y, z, max;
    //scanf("%d%d%d", &x, &y, &z);
    changeAgainInputText("enter Vx: xeN");
    x = (int)saveinint();
    y = (int)saveinint();
    z = (int)saveinint();
    //printf("%d %d %d", x, y, z);
    if(z<y)
    {
        max = y; y = z; z = max;
    }
    if(z < x)
    {
        max = x; x = z; z = max;
    }

    if(x * x + y * y == z * z)
    {
        printf("Yes\n");
        if(gcd(x,y) == gcd(x,z) == gcd(y,z)==1)
            printf("Simple numbers");
        else
            printf("NOT Simple numbers");
    }
    else
    {
        printf("NO");
    }
    return 0;
}

int gcd(int x, int y)
{
    int d = 1;
    if(x < y)
    {
        d = y; y = x; x = d;
    }
    while(d)
    {
        d = x % y;
        x = y;
        y = d;
    }
    return x;
}
