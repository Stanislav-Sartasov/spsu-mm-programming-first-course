#include"hash_table.h"
#define STARTN 7
#define ALLOWED 5

void drop(struct table *tab)
{
    for (int i = 0; i < tab->len; ++i)
    {
        while (tab->bins[i] != NULL)
        {
            del(tab->bins[i]->key, tab);
        }
    }
    free(tab->bins);
    tab->len = 0;
    tab->maxlen = 0;
    tab->elemnumber = 0;
    tab->indexMaxlen = 0;
    tab->allowed = 0;
}

void rebalance(struct table *oldTab)
{
    struct table *new = (struct table*)malloc(sizeof (struct table));
    create(new);
    free(new->bins);
    new->bins = (struct list**)malloc(sizeof (struct list*) * (unsigned long long)oldTab->len * 2);
    new->len = oldTab->len * 2;
    new->allowed = oldTab->allowed * 2;
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
    oldTab->indexMaxlen = new->indexMaxlen;
    oldTab->allowed = new->allowed;
}

int hash(int key)
{
    key = (123 * (key + 1)) - 1;
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
    if (h == tab->indexMaxlen)
        tab->maxlen -= 1;
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
        i = 1;
    }
    else
    {
        i = 1;
        while ((ptr->next != NULL) && (ptr->key != key))
        {
            ptr = ptr->next;
            ++i;
        }

        if (ptr->key == key)
        {
            ptr->data = data;
            return 0;
        }
        ptr->next = (struct list*)malloc(sizeof (struct list));
        ptr = ptr->next;
        ptr->next = NULL;
        ++i;
    }

    ptr->data = data;
    ptr->key = key;

    tab->elemnumber += 1;
    if (tab->maxlen < i)
    {
        tab->maxlen = i;
        tab->indexMaxlen = h;
    }


    if ((20 * tab->maxlen > tab->elemnumber) && (tab->elemnumber > tab->allowed))
        rebalance(tab);
    return  0;
}

void create(struct table *tab)
{
    tab->bins = (struct list**)malloc(sizeof (struct list*) * STARTN);
    tab->len = STARTN;
    tab->maxlen = 0;
    tab->elemnumber = 0;
    tab->indexMaxlen = 0;
    tab->allowed = ALLOWED;
    for (int i = 0; i < STARTN; ++i)
        tab->bins[i] = NULL;
}
