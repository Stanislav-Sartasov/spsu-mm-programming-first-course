#pragma once

typedef struct Node Node;

struct Node
{
    char key[100];
    char value[100];
    Node *next;
    int p;
    int mod;
};

const int LEN, MAX_LEN;

void create(Node *a[]);

int insert(Node *a[], char key[], char v[]);

Node* find(Node *a[], char key[]);

int del(Node *a[], char key[]);
