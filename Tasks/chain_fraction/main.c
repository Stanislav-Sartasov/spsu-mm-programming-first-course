#include <stdio.h>
#include <stdlib.h>
#include <math.h>

const int size_of_precalculation = 500000;

typedef long long ll;

void input(ll *a)
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
            if  (fl == 0 && *a > 0)
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
    ll x;
    input(&x);
    double y = pow(x, 0.5);
    double eps = 10e-8;
    double y_trunc;
    double y_fract = modf(y, &y_trunc);
    while (y - y_trunc < eps)
    {
        printf("The entered number is the square of the integer. ");
        input(&x);
        y = pow(x, 0.5);
        y_fract = modf(y, &y_trunc);
    }
    int arr[size_of_precalculation], denominator;
    double numerator;
    ll t = (int) y_trunc;
    ll q0 = (int) y_trunc;
    printf("fraction have the presentation - %d, ", q0);
    denominator = x - q0 * q0;
    numerator = y + q0;
    arr[0] = (int) (numerator / denominator);
    for (int i = 1; i < size_of_precalculation; i++)
    {
       t = numerator - arr[i - 1] * denominator - y_fract;
       numerator = y + q0 - t;
       denominator = (x - (q0 - t) * (q0 - t)) / denominator;
       arr[i] = (int) (numerator / denominator);
    }
    int *first_q = (int*) malloc(sizeof(int) * 2);
    first_q[0] = arr[0];
    int k = 1, i = 1;
    while (k < size_of_precalculation)
    {
       if (arr[k] != first_q[0])
       {
            first_q = (int*) realloc(first_q,sizeof(int) * (i + 1));
            first_q[i] = arr[k];
            i++;
            k++;
       }
       else
        {
            int compare_q = 1;
            for (int l = k; l < 100; l++)
            {
                if (arr[l] != arr[l % i])
                {
                    compare_q = 0;
                    for (int e = i; e <= l; e++)
                    {
                        first_q = (int*) realloc(first_q,sizeof(int) * (i + 1));
                        first_q[i] = arr[e];
                        i++;
                        k++;
                    }
                    break;
                }
            }
            if (compare_q == 1)
            {
                printf("(");
                for (int l = 0; l < i - 1; l++)
                    printf("%d, ", first_q[l]);
                printf("%d)", first_q[i - 1]);
                free(first_q);
                return 0;
            }
        }
    }
    return 0;
}
