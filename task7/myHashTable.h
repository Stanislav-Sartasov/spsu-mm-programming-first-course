#pragma once

typedef struct Node Node;

struct Node
{
    char key[100];
    char value[100];
    Node *next;
};

const int LEN, MAX_LEN;
int P, MOD;

void add(Node *cur, char k[], char v[]);

void del_node(Node *cur);

int get_p();

long long get_mod();

int hash(char x[]);

void rebalance(Node **a[]);

int insert(Node **a[], char key[], char v[]);

Node* find(Node **a[], char key[]);

int del(Node **a[], char key[]);
