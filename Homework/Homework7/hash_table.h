#ifndef TASKS_HASH_TABLE_H
#define TASKS_HASH_TABLE_H

#include <stdlib.h>

typedef struct 
{
    int     key;
    int     value;
    int     isFree;
    size_t  next;
} DataNode;

typedef struct 
{
    size_t     size;
    size_t     numKeys;
    size_t     freeNodeIndex;
    size_t*    buckets;
    DataNode*  data;
} HashTable;

HashTable* hashTableCreate();
void       hashTableDelete(HashTable* table);


int  hashTableInsertKey(HashTable* t, int key, int value);
void hashTableDeleteKey(HashTable* t, int key);
int  hashTableSearchKey(HashTable* table, int key, int defaultValue);

#endif //TASKS_HASH_TABLE_H
