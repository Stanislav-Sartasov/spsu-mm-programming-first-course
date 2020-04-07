#include <stdio.h>
#include <stdlib.h>

typedef unsigned long long ull;

void input(ull *a)
{
    int x;
    while (1)
    {
        printf("Enter 1 integer number\n");
        x = scanf("%lld", a);
        if (x == 1)
        {
            char s;
            int fl = 0;
            s = getchar();
            while (s != '\0' && s != '\n')
            {
                if  (s != ' ')
                {
                    fl = 1;
                }
                s = getchar();
            }
            if  (fl == 0 && *a >= 0)
            {
                break;
            }
        }
        else
        {
            char s;
            s = getchar();
            while (s != '\0' && s != '\n')
            {
                s = getchar();
            }
        }
        printf("Wrong input\n");
    }
}

int main()
{
    ull n;
    unsigned char pence[8] = {1, 2, 5, 10, 20, 50, 100, 200};
    input(&n);
    ull *a = (ull*) malloc(sizeof(ull) * (n + 2));
    for (int i = 0; i < n + 2; i++)
        a[i] = 0;
    a[0] = 1;
    for (int j = 0; j < 8; j++)
        for (int i = pence[j]; i < n + 1; i++)
            a[i] += a[i - pence[j]];
    printf("answer - %lld", a[n]);
    free(a);
    return 0;
}
