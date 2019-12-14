#include <stdio.h>
#include <math.h>
#include <stdlib.h>

#define MSIZE 992*2

struct number
{
    int digit[MSIZE], realSize;
};

void myPower(struct number*, struct number*);
void myPowerLast(struct number*, struct number*, struct number*);
void switcher(struct number**, struct number**);
void myPowerLastC(struct number *, struct number *);

int main()
{
    struct number *a, *b, *c;
    int power = 0;

    a = (struct number*)malloc(sizeof(struct number));
    b = (struct number*)malloc(sizeof(struct number));
    c = (struct number*)malloc(sizeof(struct number));
    for (int i = 0; i < MSIZE; ++i)
    {
        a->digit[i] = 0;
        b->digit[i] = 0;
        c->digit[i] = 0;
    }
    a->realSize = 1; b->realSize = 1; c->realSize = 1;
    a->digit[0] = 3; b->digit[0] = 3; c->digit[0] = 1; power = 1;

    for (;power < 4096;)
    {
        power *= 2;
        myPower(a, b);
        if ((power == 8) || (power == 128) || (power == 256) || (power == 512))
        {
          myPowerLastC(b, c);
        }
        switcher(&a, &b);
    }

    myPowerLast(a, c, b);

    printf("0x");
    for (int i = b->realSize; i >= 0; --i)
    {
        printf("%x", b->digit[i]);
    }

    free(a);
    free(b);
    free(c);
    return 0;
}

void myPowerLastC(struct number *b, struct number *c)
{
    struct number *a = (struct number*)malloc(sizeof(struct number));
    for (int i = 0; i < c->realSize; ++i)
    {
        a->digit[i] = c->digit[i];
        c->digit[i] = 0;
    }
    a->realSize = c->realSize;

    int max = 0;

    for (int i = 0; i < a->realSize; ++i)
    {
        int k = 0;
        for (int j = 0; j < b->realSize; ++j)
        {
            if (i + j > MSIZE)
                break;
            int temp = (a->digit[i] * b->digit[j] + k + c->digit[i + j]) / 16;
            c->digit[i + j] = (a->digit[i] * b->digit[j] + k + c->digit[i + j]) % 16;
            k = temp;
            max = max < i + j ? i + j : max;
        }
        if ((i + b->realSize + 1 < MSIZE) && (k))
        {
            c->digit[i + b->realSize] += k;
            max = max < i + b->realSize ? i + b->realSize : max;
        }
    }
    c->realSize = max + 1;
    free(a);
}

void myPowerLast(struct number *a, struct number *b, struct number *c)
{
    int max = 0;
    for (int i = 0; i < c->realSize; ++i)
        c->digit[i] = 0;

    for (int i = 0; i < a->realSize; ++i)
    {
        int k = 0;
        for (int j = 0; j < b->realSize; ++j)
        {
            if (i + j > MSIZE)
                break;
            int temp = (a->digit[i] * b->digit[j] + k + c->digit[i + j]) / 16;
            c->digit[i + j] = (a->digit[i] * b->digit[j] + k + c->digit[i + j]) % 16;
            k = temp;
            max = max < i + j ? i + j : max;
        }
        if ((i + b->realSize + 1 < MSIZE) && (k))
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

void myPower(struct number *a, struct number *b)
{
    b->realSize = a->realSize;
    int max = 0;

    for (int i = 0; i < b->realSize; ++i)
        b->digit[i] = 0;

    for (int i = 0; i < a->realSize; ++i)
    {
        int k = 0;
        for (int j = 0; j < a->realSize; ++j)
        {
            if (i + j > MSIZE)
                break;
            int temp = (a->digit[i] * a->digit[j] + k + b->digit[i + j]) / 16;
            b->digit[i + j] = (a->digit[i] * a->digit[j] + k + b->digit[i + j]) % 16;
            k = temp;
            max = max < i + j ? i + j : max;
        }
        if ((i + a->realSize + 1 < MSIZE) && (k))
        {
            b->digit[i + a->realSize] += k;
            max = max < i + a->realSize ? i + a->realSize : max;
        }
    }
    b->realSize = max + 1;
}
