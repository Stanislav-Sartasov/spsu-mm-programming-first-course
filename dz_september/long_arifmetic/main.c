#include <stdio.h>
#include <math.h>
#include <stdlib.h>

struct number
{
    int *digit;
    int realSize;
};

void myPower(struct number*, struct number*, unsigned long long);
void myPowerLast(struct number*, struct number*, struct number*, unsigned long long);
void switcher(struct number**, struct number**);
void myPowerLastC(struct number *, struct number *, unsigned long long);
int powerGo(int, int);

int main()
{
    unsigned long long MSIZE = (unsigned long long)(1 + (log(3) / log(16)) * 5000);
    struct number *a, *b, *c;
    int power = 0;

    int n = 5000, n1 = n, step = 1;
    while (n > 0)
    {
        n = n / 2;
        ++step;
    }
    step = step - 2;
    n = pow(2, step);

    a = (struct number*)malloc(sizeof(struct number));
    b = (struct number*)malloc(sizeof(struct number));
    c = (struct number*)malloc(sizeof(struct number));
    a->digit = (int*)malloc(sizeof (int) * MSIZE);
    b->digit = (int*)malloc(sizeof (int) * MSIZE);
    c->digit = (int*)malloc(sizeof (int) * MSIZE);
    for (int i = 0; (unsigned long long)i < MSIZE; ++i)
    {
        a->digit[i] = 0;
        b->digit[i] = 0;
        c->digit[i] = 0;
    }
    a->realSize = 1; b->realSize = 1; c->realSize = 1;
    a->digit[0] = 3; b->digit[0] = 3; c->digit[0] = 1; power = 1;

    for (;power < n;)
    {
        power *= 2;
        myPower(a, b, MSIZE);
        //if ((power == 8) || (power == 128) || (power == 256) || (power == 512))
        if (powerGo(power, n1 - n))
        {
          myPowerLastC(b, c, MSIZE);
        }
        switcher(&a, &b);
    }

    myPowerLast(a, c, b, MSIZE);

    printf("0x");
    for (int i = b->realSize - 1; i >= 0; --i)
    {
        printf("%x", b->digit[i]);
    }


    free(a->digit);
    free(b->digit);
    free(c->digit);
    free(a);
    free(b);
    free(c);
    return 0;
}

int powerGo(int power, int number)
{
    int t = 0;
    int step = 1;
    while (number > 0)
    {
       t = number % 2;
       number = number / 2;
       if (step * t == power)
           return 1;
       step *= 2;
    }
    return 0;
}

void myPowerLastC(struct number *b, struct number *c, unsigned long long MSIZE)
{
    struct number *a = (struct number*)malloc(sizeof(struct number));
    a->digit = (int*)malloc(sizeof (int) * MSIZE);
    for (int i = 0; i < c->realSize; ++i)
    {
        a->digit[i] = c->digit[i];
        c->digit[i] = 0;
    }
    a->realSize = c->realSize;

    myPowerLast(b, a, c, MSIZE);

    free(a->digit);
    free(a);
}

void myPowerLast(struct number *a, struct number *b, struct number *c, unsigned long long MSIZE)
{
    int max = 0;
    for (int i = 0; i < c->realSize; ++i)
        c->digit[i] = 0;

    for (int i = 0; i < a->realSize; ++i)
    {
        int k = 0;
        for (int j = 0; j < b->realSize; ++j)
        {
            if ((unsigned long long)(i + j) > MSIZE)
                break;
            int temp = (a->digit[i] * b->digit[j] + k + c->digit[i + j]) / 16;
            c->digit[i + j] = (a->digit[i] * b->digit[j] + k + c->digit[i + j]) % 16;
            k = temp;
            max = max < i + j ? i + j : max;
        }
        if (((unsigned long long)(i + b->realSize + 1) < MSIZE) && (k))
        {
            c->digit[i + b->realSize] += k;
            max = max < i + b->realSize ? i + b->realSize : max;
        }
    }
    c->realSize = max + 1;
}

void switcher(struct number **a, struct number **b)
{
    struct number *temp = *a;
    *a = *b;
    *b = temp;
}

void myPower(struct number *a, struct number *b, unsigned long long MSIZE)
{
    b->realSize = a->realSize;
    myPowerLast(a, a, b, MSIZE);
}
