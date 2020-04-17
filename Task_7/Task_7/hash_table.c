#pragma once
#include <stdio.h>
#include "hash_table.h"
#define LIMIT 4


int hash_function(int x, int size)
{
    return x % size;
}

void hash_initialization(Node** hash_table, int size)
{
    for (int i = 0; i < size; i++)
    {
        hash_table[i] = NULL;
    }
}

int check(Node** hash_table, int size, int index)
{
    int i = 0;
    Node* node = hash_table[index];
    while (node)
    {
        i++;
        node = node->next;
    }
    free(node);
    return i > LIMIT ? 1 : 0;
}

void add(Node** hash_table, int value, int key, int size, int* p)
{
    Node* new_node = (Node*)malloc(sizeof(Node));
    if (new_node)
    {
        new_node->key = key;
        new_node->value = value;
        new_node->next = hash_table[hash_function(key, size)];
        hash_table[hash_function(key, size)] = new_node;
    }
    if (check(hash_table, size, hash_function(key, size)))
    {
        balancing(hash_table, p);
    }

}

int find(Node** hash_table, int key, int size)
{
    Node* node = hash_table[hash_function(key, size)];
    while (node)
    {
        if (node->key == key)
        {
            return node->value;
        }
        node = node->next;
    }
    return -1;
}

void rm(Node** hash_table, int key, int size)
{
    Node* node, * last_node;
    node = hash_table[hash_function(key, size)];
    if (node->key == key)
    {
        hash_table[hash_function(key, size)] = node->next;
        printf("removal successful\n");
    }
    else
    {
        last_node = hash_table[hash_function(key, size)];
        node = hash_table[hash_function(key, size)]->next;
        while (node)
        {
            if (node->key == key)
            {
                last_node->next = node->next;
                free(node);
                printf("removal successful\n");
                return;
            }
            else
            {
                last_node = node;
                node = node->next;
            }
        }
    }

}

void print_table(Node** hash_table, int size)
{
    for (int i = 0; i < size; i++)
    {
        printf("key %d :  ", i);
        Node* node = hash_table[i];
        while (node)
        {
            printf("%d (key %d), ", node->value, node->key);
            node = node->next;
        }
        free(node);
        printf("\n");
    }
}

void copy(Node** hash_table, Node** hash_new, int size, int flag)
{
    int x = size;
    if (flag)
    {
        x = size / 2;
    }
    for (int i = 0; i < x; i++)
    {
        Node* new_node = hash_table[i];
        while (new_node)
        {
            add(hash_new, new_node->value, new_node->key, size, NULL);
            new_node = new_node->next;
        }
        free(new_node);
    }
}

void balancing(Node** hash_table, int* size)
{
    *size *= 2;
    int size_new = *size;
    Node* hash_new = (Node*)malloc(sizeof(Node) * size_new);
    hash_initialization(hash_new, size_new);
    copy(hash_table, hash_new, size_new, 1);
    *hash_table = malloc(sizeof(Node) * size_new);
    hash_initialization(hash_table, size_new);
    copy(hash_new, hash_table, size_new, 0);
    free(hash_new);
}



