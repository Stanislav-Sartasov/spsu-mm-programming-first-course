#include "hashTable.h"


node* createNode (int key, int value)
{

    node* nNode = malloc(sizeof(node));

    nNode->key = key;
    nNode->value = value;
    nNode->link = NULL;
    return nNode;
}
