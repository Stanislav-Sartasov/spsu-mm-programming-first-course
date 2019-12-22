#include "pool.h"
#include "btree.h"

#include <stdio.h>

typedef struct 
{
    BTree tree;
} Pool;

/*
 * we have a series of pools, each holding multiple chunks of size of power of 2.
 *
 * pool[MIN_POOL_ID]    : holds free memory blocks of size 2 ^ (MIN_POOL_ID)
 * pool[MIN_POOL_ID + 1]: holds free memory blocks of size 2 ^ (MIN_POOL_ID + 1)
 * ...
 * pool[MAX_POOL_ID]    : holds free memory blocks of size 2 ^ (MAX_POOL_ID)
 *
 * */
Pool pools[MAX_POOL_ID + 1];

// finds if pools[p] has any free continues c blocks, of size (2 ^ p)
static int poolBlockAvailable(int p, int c)
{
    if (pools[p].tree.root == NULL || !btreeQuery(&pools[p].tree, c))
        return 0;
    return 1;
}

// returns a memory location to (2 ^ p * c) bytes of free memory
static void* poolBlockRemove(int p, int c)
{

    if (pools[p].tree.root == NULL) {
        fprintf(stderr, "[pool]: fatal error, root = NULL\n");
        exit(-1);
    }

    // find a free node, whose has at least c blocks
    BNode* freeNode = btreeQuery(&pools[p].tree, c);

    // This should not happen, this function should only be called when pool has free block's
    if (freeNode == NULL || freeNode->nBlock < c)
	{
        fprintf(stderr, "[pool]: fatal error\n");
        exit(-1);
    }

    btreeDelete(&pools[p].tree, freeNode);

    // no extra blocks in free node
    if (freeNode->nBlock == c)
        return (void *)(freeNode->segMin);

    // we have some extra blocks in free node, re-add those back to tree
    uintptr_t segMin = freeNode->segMin + (c * (1 << p));
    uintptr_t segMax = freeNode->segMax;
    uintptr_t nBlock = freeNode->nBlock - c;

    btreeInsert(&pools[p].tree, segMin, segMax, nBlock);

    return (void *)(freeNode->segMin);
}

// add's a (2 ^ p * c) bytes of memory to pool
// p = pool index
// c = number of blocks to add
// blk = memory location of starting block
static void poolBlockAdd(int p, int c, void* blk)
{
    // we need to insert this block back to pool, and join any left and right
    // continuous blocks

    // The node to insert
    uintptr_t segMin = (uintptr_t)(blk);
    uintptr_t segMax = segMin + (c * (1 << p)) - 1;
    uintptr_t nBlock = c;

    {
        // find, if we have a right neighbouring block in tree, if yes combine it
        BNode* nei = btreeFindNode(&pools[p].tree, segMax + 1, 0);
        if (nei)
		{
            btreeDelete(&pools[p].tree, nei);
            segMax = nei->segMax;
            nBlock += nei->nBlock;
        }
    }

    {
        // find, if we have a left neighbouring block in tree, if yes combine it
        BNode* nei = btreeFindNode(&pools[p].tree, 0, segMin - 1);
        if (nei)
		{
            btreeDelete(&pools[p].tree, nei);
            segMin = nei->segMin;
            nBlock += nei->nBlock;
        }
    }

    btreeInsert(&pools[p].tree, segMin, segMax, nBlock);
}

// returns a memory block of size (2 ^ p * c)
// p = pool id
// c = number of blocks to allocate
void* poolAllocate(int p, int c)
{
    if (!(p >= MIN_POOL_ID && p <= MAX_POOL_ID))
        return NULL;

    // check if pool[p], is having free continuos c blocks
    if (poolBlockAvailable(p, c))
	{
        return poolBlockRemove(p, c);
    }

    // ask right side neighboring pools
    for (int i = p + 1; i <= MAX_POOL_ID; ++i)
	{
        int d = (1 << (i - p));
        int r = (c / d) + ((c % d)? 1: 0);
        int t = r * (1 << (i - p));

        if (poolBlockAvailable(i, r))
		{
            void* blk = poolBlockRemove(i, r);
            poolBlockAdd(p, t, blk);
            return poolBlockRemove(p, c);
        }
    }

    // ask left side neighboring pools
    for (int i=p-1; i >= MIN_POOL_ID; --i)
	{
        int r = c * (1 << (p - i));
        int t = c;

        if (poolBlockAvailable(i, r))
		{
            void* blk = poolBlockRemove(i, r);
            poolBlockAdd(p, t, blk);
            return poolBlockRemove(p, c);
        }
    }

    return NULL;
}

// deallocate's previously allocated memory block
void poolDeallocate(int p, int c, void* addr)
{
    if (!(p >= MIN_POOL_ID && p <= MAX_POOL_ID)) 
	{
        fprintf(stderr, "[POOL]: fatal error, deleting block from wrong pool\n");
        exit(-1);
    }

    poolBlockAdd(p, c, addr);
}

void poolInit(int p, int c, void* addr)
{
    for (int i=0; i <= MAX_POOL_ID; ++i)
	{
        pools[i].tree.root = NULL;
    }
    poolBlockAdd(p, c, addr);
}


