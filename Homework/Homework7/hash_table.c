#include "hash_table.h"

#include <stdio.h>

#define HASH_TABLE_END(t)       (t->size)
#define SUCCESS                 (0)
#define ERROR                   (1)
#define BUCKET_EMPTY(t, i)      (t->buckets[i] == t->size)
#define BUCKET_HEAD(t, i)       (t->buckets[i])

static HashTable* HashTableCreateImpl(size_t size);
static size_t     hashCode(HashTable* table, int key);
static size_t     getFreeNodeIndex(HashTable* t);
static void       addNode(HashTable* t, int key, int value);
static int        rebalance(HashTable* t);
static void       freeNode(HashTable* t, size_t bucketIndex, size_t nodeIndex,
                           size_t prevNodeIndex);

HashTable* hashTableCreate() 
{
    return HashTableCreateImpl(1);
}

static HashTable* HashTableCreateImpl(size_t size) 
{

    size_t* buckets = malloc(size * sizeof(size_t));
    DataNode* data  = malloc(size * sizeof(DataNode));

    if (!buckets || !data)
        return NULL;

    HashTable* table = malloc(sizeof(HashTable));

    if (!table) {
        free(buckets);
        free(data);
        return NULL;
    }

    table->size = size;
    table->numKeys = 0;
    table->freeNodeIndex = 0;
    table->buckets = buckets;
    table->data = data;

    for(int i=0; i<size; ++i) {
        data[i].next = (i + 1);
        data[i].isFree = 1;
        buckets[i] = HASH_TABLE_END(table);
    }

    return table;
}

void hashTableDelete(HashTable* table) 
{
    if (!table)
        return;

    free(table->buckets);
    free(table->data);
    free(table);
}

static size_t hashCode(HashTable* table, int key) 
{
    return key % table->size;
}

static size_t getFreeNodeIndex(HashTable* t) 
{
    size_t freeIndex = HASH_TABLE_END(t);
    if (t->freeNodeIndex != HASH_TABLE_END(t)) 
	{
        freeIndex = t->freeNodeIndex;
        t->freeNodeIndex = t->data[freeIndex].next;
    }
    return freeIndex;
}

static void addNode(HashTable* t, int key, int value) 
{

    size_t bucketIndex = hashCode(t, key);
    size_t freeNodeIndex = getFreeNodeIndex(t);

    t->data[freeNodeIndex].key    = key;
    t->data[freeNodeIndex].value  = value;
    t->data[freeNodeIndex].next   = HASH_TABLE_END(t);
    t->data[freeNodeIndex].isFree = 0;

    if (!BUCKET_EMPTY(t, bucketIndex))
        t->data[freeNodeIndex].next = BUCKET_HEAD(t, bucketIndex);

    BUCKET_HEAD(t, bucketIndex) = freeNodeIndex;
    ++t->numKeys;
}

static void freeNode(HashTable* t, size_t bucketIndex, size_t nodeIndex,
        size_t prevNodeIndex) 
{

    if (prevNodeIndex == HASH_TABLE_END(t)) 
	{
        // deleting beginning node
        t->buckets[bucketIndex] = t->data[nodeIndex].next;
        t->data[nodeIndex].next = HASH_TABLE_END(t);
    } else 
	{
        // delete node in middle
        t->data[prevNodeIndex].next = t->data[nodeIndex].next;
        t->data[nodeIndex].next = HASH_TABLE_END(t);
    }

    // add node to free list
    t->data[nodeIndex].isFree = 1;
    if (t->freeNodeIndex != HASH_TABLE_END(t)) 
	{
        t->data[nodeIndex].next   = t->freeNodeIndex;
        t->freeNodeIndex = nodeIndex;
    } else 
	{
        t->freeNodeIndex = nodeIndex;
    }

    --t->numKeys;
}

static int rebalance(HashTable* t) 
{

    size_t newSize = t->size;
    if (t->size == t->numKeys) 
	{
        newSize = newSize << 1u;
    } else if (t->numKeys < (t->size >> 2u))
	{
        newSize = newSize >> 1u;
    } else
	{
        return SUCCESS;
    }

    HashTable* tmpTable = HashTableCreateImpl(newSize);
    if (tmpTable == NULL)
        return ERROR;

    for (size_t i=0; i < t->size; ++i) 
	{
        if (!t->data[i].isFree)
            addNode(tmpTable, t->data[i].key, t->data[i].value);
    }

    free(t->buckets);
    free(t->data);

    *t = *tmpTable;

    tmpTable->buckets = NULL;
    tmpTable->data = NULL;
    hashTableDelete(tmpTable);

    return SUCCESS;
}

int hashTableInsertKey(HashTable* t, int key, int value) 
{
    // rebalance prior, if inserting key causes rebalance
    if(rebalance(t) == ERROR)
        return ERROR;

    addNode(t, key, value);
    return SUCCESS;
}

void hashTableDeleteKey(HashTable* t, int key) 
{

    size_t bucketIndex = hashCode(t, key);

    size_t prevNodeIndex = HASH_TABLE_END(t);
    size_t nodeIndex = BUCKET_HEAD(t, bucketIndex);
    while (nodeIndex != HASH_TABLE_END(t)) 
	{
        if (t->data[nodeIndex].key == key) 
		{
            freeNode(t, bucketIndex, nodeIndex, prevNodeIndex);
            break;
        }
        prevNodeIndex = nodeIndex;
        nodeIndex = t->data[nodeIndex].next;
    }

    rebalance(t);
}

int hashTableSearchKey(HashTable* t, int key, int defaultValue) 
{

    size_t bucketIndex = hashCode(t, key);

    if (!t->buckets || BUCKET_EMPTY(t, bucketIndex))
        return defaultValue;

    size_t nodeIndex = BUCKET_HEAD(t, bucketIndex);
    while (nodeIndex != HASH_TABLE_END(t)) 
	{
        if (t->data[nodeIndex].key == key)
            return t->data[nodeIndex].value;
        nodeIndex = t->data[nodeIndex].next;
    }

    return defaultValue;
}
