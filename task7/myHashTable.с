#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>
#include <math.h>
#include <string.h>

typedef struct Node Node;

const int LEN = 1e4,
          MAX_LEN = 100;
int P, MOD;

struct Node
{
    char key[100];
    char value[100];
    Node *next;
};

void add(Node *cur, char k[], char v[])
{
	Node *new_node = (Node*) malloc(sizeof(Node));
	strcpy(new_node->key, k);
	strcpy(new_node->value, v);
	new_node->next = NULL;
	cur->next = new_node;
}

void del_node(Node *cur)
{
    if (cur == NULL)
        return;
    cur->next = (cur->next)->next;
    free(cur->next);
}

int get_p()
{
    int p = 10 + rand() % 100;
    int b = 1;
    while (b == 1)
    {
        b = 0;
        for (int i = 2; i <= (int)sqrt(p); i++)
        {
            if (p % i == 0)
            {
                b = 1;
                break;
            }
        }
        p++;
    }
    return p - 1;
}

long long get_mod()
{
    int mod[] = {1e9 + 7, 10003003, 10086773, 10874483, 11621243};
    int k = rand() % 5;
    return mod[k];
}

int hash(char x[])
{
    long long h = 0;
    //P = 1; MOD = 1e6;
    for (int i = 0; x[i] != '\0'; i++)
    {
        h = (h * P + (int)x[i]) % MOD;
    }
    return h % LEN;
}

void rebalance(Node **a[])
{
    P = get_p();
    MOD = get_mod();
    Node *b[LEN];
    for (int i = 0; i < LEN; i++)
    {
        b[i] = (Node*) malloc(sizeof(Node));
        char e[] = {'\0'};
        strcpy(b[i]->key, e);
        strcpy(b[i]->value, e);
        b[i]->next = NULL;
    }
    for (int i = 0; i < LEN; i++)
    {
        Node *cur = a[i];
        cur = cur->next;
        while (cur != NULL)
        {
            insert(&b, cur->key, cur->value);
            cur = cur->next;
        }
    }
    strcpy(a, b);
    for (int i = 0; i < LEN; i++)
        free(b[i]);
}

int insert(Node **a[], char key[], char v[])
{
    int h = hash(key);
    Node *cur = a[h];
    int k = 0;
    while (cur->next != NULL)
    {
        if (!strcmp(cur->key, key))
            return 0;
        cur = cur->next;
        k++;
    }
    if (!strcmp(cur->key, key))
        return 0;
    add(cur, key, v);
    if (k > 20)
        rebalance(a);
    return 1;
}

Node* find(Node **a[], char key[])
{
    int h = hash(key);
    Node *cur = a[h];
    while (cur != NULL)
    {
        if (!strcmp(cur->key, key))
            return cur;
        cur = cur->next;
    }
    return NULL;
}

int del(Node **a[], char key[])
{
    int h = hash(key);
    Node *cur = a[h];
    Node *prev = *a[h];
    while (cur != NULL)
    {
        if (!strcmp(cur->key, key))
        {
            del_node(prev);
            return 1;
        }
        prev = cur;
        cur = cur->next;
    }
    return 0;
}

