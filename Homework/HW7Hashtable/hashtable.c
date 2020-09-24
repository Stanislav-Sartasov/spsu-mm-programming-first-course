#include "hashTable.h"

node* buildTable (int items)
{
    node* hashTable = malloc(items * sizeof(node));
    return hashTable;
}