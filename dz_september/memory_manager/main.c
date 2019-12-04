#include "manager.h"

int main()
{
    unsigned long long memsize = 8;
    int *number, *next;
    unsigned int amount;
    //scanf("%lld", &memsize);
    //printf("%lld", memsize);

    init(memsize);
    printf("how many elements(2)? ");
    scanf("%d", &amount);
    number = (int*)  myMalloc((size_t)(sizeof(int) * amount));
    if (number)
    {
        for (int i = 0; i < (int)amount; ++i)
        {
            printf("put your %d number ", i);
            scanf("%d", &number[i]);
        }

        for(int i = 0; i < (int)amount; ++i)
            printf("%d ", number[i]);
        printf("\n");
        next = (int*)myMalloc(sizeof (int));
        if (!next)
        {
            printf("memory full, can not give space for *next\n");

            myFree(number);
            next = (int*)myMalloc(sizeof (int));
        }
        if (next)
        {
            printf("memory is given for *next, put your number: ");
            scanf("%d",next);
            printf("number is %d", *next);

            myFree(next);
        }



    }
    else
    {
        printf("memory full\n");
    }
    printf("end of test 1\n");

    initstop();
    init(memsize + 4);

    printf("--------------------------------------/n");
    printf("test 2(realloc)/n");
    next = (int*)myMalloc(sizeof (int));
    number = (int*)myMalloc(sizeof (int) * 2);
    printf("\n");
    if (number)
        for(int i = 0; i < 2; ++i)
        {
            printf("put your %d number ", i);
            scanf("%d", &number[i]);
        }

    int *real = myRealloc(number, sizeof(int) * 3);
    if(!real)
        printf("memory full for realloc\n");

    myFree(next);
    real = myRealloc(number, sizeof(int) * 3);

    printf("add number: ");
    scanf("%d", real + 2);


    if (real)
        for (int i = 0; i < 3; ++i)
            printf("%d ", real[i]);

    initstop();

    printf("\n Bye)");
    return 0;
}
