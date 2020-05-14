#pragma once
#include <stdio.h>
#include "hash_table.h"
#include <malloc.h>
#define LIMIT 4

void balancing(Hash* hash);
void copy(Node** hash_table, Node** hash_new, int size, int flag);

void hash_list_initialization(Node** list, int size)
{
    for (int i = 0; i < size; i++)
    {
        list[i] = NULL;
    }
}

int hash_function(int x, int size)
{
    return x % size;
}

Hash hash_initialization(int size)
{
    Hash hash;
    hash.size = size;
    hash.hash_list = (Node*)malloc(sizeof(Node) * size);
    hash_list_initialization((Node**)hash.hash_list, hash.size);
    return hash;
}

void add_for_balancing(Node** hash_table, int value, int key, int size)
{
    Node* new_node = (Node*)malloc(sizeof(Node));
    if (new_node)
    {
        new_node->key = key;
        new_node->value = value;
        new_node->next = hash_table[hash_function(key, size)];
        hash_table[hash_function(key, size)] = new_node;
    }
}

int check(Node** hash_table, int index)
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

void add(Hash* hash, int value, int key)
{
    Node* new_node = (Node*)malloc(sizeof(Node));
    Node** hash_table = (Node**)hash->hash_list;
    if (new_node)
    {
        new_node->key = key;
        new_node->value = value;
        new_node->next = hash_table[hash_function(key, hash->size)];
        hash_table[hash_function(key, hash->size)] = new_node;
    }
    if (check(hash_table, hash_function(key, hash->size)))
    {
        balancing(hash);
        hash->size = hash->size * 2;

    }
}


void print_table(Hash* hash)
{
    Node** hash_table = (Node**)hash->hash_list;
    for (int i = 0; i < hash->size; i++)
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

int find(Hash* hash, int key)
{
    Node** hash_table = (Node**)hash->hash_list;
    Node* node = hash_table[hash_function(key, hash->size)];
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

void rm(Hash* hash, int key)
{
    Node** hash_table = (Node**)hash->hash_list;
    Node* node;
    Node* last_node;
    node = hash_table[hash_function(key, hash->size)];
    if (node->key == key)
    {
        hash_table[hash_function(key, hash->size)] = node->next;
        printf("removal successful\n");
    }
    else
    {
        last_node = hash_table[hash_function(key, hash->size)];
        node = hash_table[hash_function(key, hash->size)]->next;
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



void balancing(Hash* hash)
{
    int new_size = hash->size * 2;
    Hash new_hash = hash_initialization(new_size);

    Node** hash_table = (Node**)hash->hash_list;
    Node** hash_table_new = (Node**)new_hash.hash_list;
    copy(hash_table, hash_table_new, new_size, 1);
    hash_list_initialization(hash_table, new_size);
    copy(hash_table_new, hash_table, new_size, 0);
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
            add_for_balancing(hash_new, new_node->value, new_node->key, size);
            new_node = new_node->next;
        }
        free(new_node);
    }
}