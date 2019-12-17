#include <stdio.h>
#include <stdlib.h>
#include <string.h>
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
};

void create(struct table *tab);
int hash(int key);
int add(int key, int data, struct table *tab);
struct list * find(int key, struct table *tab);

int main()
{

    struct table mytab;
    create(&mytab);
    add(2, 4, &mytab);
    struct list *ptr = find(2, &mytab);
    printf("%d", ptr->data);

    return 0;
}

int hash(int key)
{
    key = 5 * key / 2;
    return key;
}

struct list * find(int key, struct table *tab)
{
    int h = hash(key) % tab->len;
    struct list *ptr = tab->bins[h];
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
        return 0;
    }
}

int add(int key, int data, struct table *tab)
{
    int h = hash(key) % tab->len;
    struct list *ptr = tab->bins[h];
    if (tab->bins[h] == NULL)
    {
        tab->bins[h] = (struct list*)malloc(sizeof (struct list));
        tab->bins[h]->next = NULL;
        ptr = tab->bins[h];
    }
    else
    {
        while (ptr->next != NULL)
        {
            ptr = ptr->next;
        }

        ptr->next = (struct list*)malloc(sizeof (struct list));
        ptr = ptr->next;
        ptr->next = NULL;
    }

    ptr->data = data;
    ptr->key = key;
    return  0;
}

void create(struct table *tab)
{
    tab->bins = (struct list**)malloc(sizeof (struct list*) * StartN);

    for (int i = 0; i < StartN; ++i)
        tab->bins[i] = NULL;
}
