#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "../mylib/functionToGo.h"
#include "hash-table.h"

int main()
{

    int key, val;
    struct table mytab;
    create(&mytab);
    do
    {
        printf("put your key(int): ");
        key = (int)saveInInt();
        printf("(value == 0 to exit)put your value(int): ");
        val = (int)saveInInt();
        if (val)
            add(key, val, &mytab);
    }
    while (val != 0);

    printf("\n---------------------\n");
///////////////////////////////
//    struct list *ptr1;;
//    for (int i = 0; i < mytab.len; ++i)
//    {
//        ptr1 = mytab.bins[i];
//        while (ptr1 != NULL)
//        {
//            printf("k: %d, v: %d, i: %d\n", ptr1->key, ptr1->data, i);
//            ptr1 = ptr1->next;
//        }
//    }
///////////////////////////////////
    do
    {
        printf("(key == 0 to exit)put your key(int) to delete the element: ");
        key = (int)saveInInt();
        if (key)
        {
            del(key, &mytab);
            printf("done\n");
        }
    }
    while (key != 0);

    printf("\n---------------------\n");

    struct list *ptr;
    do
    {
        printf("(key == 0 to exit)put your key(int) to find the value: ");
        key = (int)saveInInt();
        if (key)
            ptr = find(key, &mytab);
        else
            break;
        if (ptr)
            printf(" data: %d\n", ptr->data);
        else
             printf("no such element\n");
    }
    while (key != 0);
    printf("\n---------------------\n");

    drop(&mytab);
    printf("hash-table deleted\n");

    return 0;
}
