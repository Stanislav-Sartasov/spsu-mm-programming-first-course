#include "MyMalloc.h"

int main ()
{
    char *a = "Hello world";
    a = myRealloc(a, 20);
    a[15] = '!';
    printf("%s", a);
    return 0;
}
