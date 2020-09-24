#ifndef HEADER_H
#define HEADER_H


#include <stdio.h>
typedef struct p
{
    int key;
    int value;
    struct p* link;
} 
node;

node* createNode(int key, int value);
void nodePushback(int key, int value, node* fNode);

#endif