#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "../mylib/functionToGo.h"
#define StartN 7


struct list
{
    int key;
    int data;
    struct list *next;
};

struct table
{
    struct list **bins;
    int len;
    int elemnumber, maxlen;
};

void create(struct table *tab);
int hash(int key);
int add(int key, int data, struct table *tab);
void rebalance(struct table *oldTab);
struct list * find(int key, struct table *tab);
void del(int key, struct table *tab);

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

    struct list *ptr1 = mytab.bins[0];

    for (int i = 0; i < mytab.len; ++i)
    {
        ptr1 = mytab.bins[i];
        while (ptr1 != NULL)
        {
            printf("k: %d, v: %d, i: %d\n", ptr1->key, ptr1->data, i);
            ptr1 = ptr1->next;
        }
    }

    do
    {
        printf("(key == 0 to exit)put your key(int) to delete the element: ");
        key = (int)saveInInt();
        del(key, &mytab);
        printf("done\n");
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

    return 0;
}

void rebalance(struct table *oldTab)
{
    struct list *ptr1;
    printf("balansing\n");
    for (int i = 0; i < oldTab->len; ++i)
    {
        ptr1 = oldTab->bins[i];
        while (ptr1 != NULL)
        {
            printf("k: %d, v: %d, i: %d\n", ptr1->key, ptr1->data, i);
            ptr1 = ptr1->next;
        }
    }
    struct table *new = (struct table*)malloc(sizeof (struct table));
    create(new);
    free(new->bins);
    new->bins = (struct list**)malloc(sizeof (struct list*) * (unsigned long long)oldTab->len * 2);
    new->len = oldTab->len * 2;
    for (int i = 0; i < new->len; ++i)
        new->bins[i] = NULL;

    for (int i = 0; i < oldTab->len; ++i)
    {
        while (oldTab->bins[i] != NULL)
        {
            add(oldTab->bins[i]->key, oldTab->bins[i]->data, new);
            del(oldTab->bins[i]->key, oldTab);
        }
    }
    free(oldTab->bins);
    oldTab->len = new->len;
    oldTab->bins = new->bins;
    oldTab->maxlen = new->maxlen;
    oldTab->elemnumber = new->elemnumber;
}

int hash(int key)
{
    //key = (123 * (key + 1)) - 1;
    return key > 0 ? key : -key;
}

void del(int key, struct table *tab)
{
    struct list *ptr = find(key, tab);
    if (!ptr)
        return;
    int h = hash(key) % tab->len;
    struct list *ptr1 = tab->bins[h];
    if (ptr1 == ptr)
    {
        tab->bins[h] = ptr->next;
    }
    else
    {
        while (ptr1->next != ptr)
        {
            if (ptr1->key == key)
            {
                break;
            }
            ptr1 = ptr1->next;
        }
        ptr1->next = ptr->next;
    }
    free(ptr);
    tab->elemnumber -= 1;
}

struct list * find(int key, struct table *tab)
{
    int h = hash(key) % tab->len;
    struct list *ptr = tab->bins[h];
    if (ptr == NULL)
        return  NULL;
    while (ptr->next != NULL)
    {
        if (ptr->key == key)
        {
            break;
        }
        ptr = ptr->next;
    }
    if (ptr->key == key)
    {
        return ptr;
    }
    else
    {
        return NULL;
    }
}

int add(int key, int data, struct table *tab)
{
    int h = hash(key) % tab->len;
    int i = 0;
    struct list *ptr = tab->bins[h];
    if (tab->bins[h] == NULL)
    {
        tab->bins[h] = (struct list*)malloc(sizeof (struct list));
        tab->bins[h]->next = NULL;
        ptr = tab->bins[h];
    }
    else
    {
        while ((ptr->next != NULL) || (ptr->key == key))
        {
            ptr = ptr->next;
            ++i;
        }

        if (ptr->key == key)
        {
            ptr->data = data;
        }
        else
        {

        ptr->next = (struct list*)malloc(sizeof (struct list));
        ptr = ptr->next;
        ptr->next = NULL;
        }
    }


    tab->elemnumber += 1;
    if (tab->maxlen < i)
        tab->maxlen = i;

    ptr->data = data;
    ptr->key = key;

    if ((20 * tab->maxlen > tab->elemnumber) && (tab->elemnumber > 25))
        rebalance(tab);
    return  0;
}

void create(struct table *tab)
{
    tab->bins = (struct list**)malloc(sizeof (struct list*) * StartN);
    tab->len = StartN;
    tab->maxlen = 0;
    tab->elemnumber = 0;
    for (int i = 0; i < StartN; ++i)
        tab->bins[i] = NULL;
}
