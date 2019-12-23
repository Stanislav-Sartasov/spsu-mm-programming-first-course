#pragma once
#include <stdio.h>
#include <stdlib.h>
#include <string.h>


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
    int elemnumber, maxlen, indexMaxlen, allowed;
};

void create(struct table *tab);
void drop(struct table *tab);
int add(int key, int data, struct table *tab);
struct list * find(int key, struct table *tab);
void del(int key, struct table *tab);
