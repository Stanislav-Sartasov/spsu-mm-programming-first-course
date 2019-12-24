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

// finds if pools[poolID] has any free continues numOfBlocks blocks, of size (2 ^ poolID)
int poolBlockAvailable(int poolID, int numOfBlocks)
{
    if (pools[poolID].tree.root == NULL || !btreeQuery(&pools[poolID].tree, numOfBlocks))
        return 0;
    return 1;
}

// returns a memory location to (2 ^ poolID * numOfBlocks) bytes of free memory
void* poolBlockRemove(int poolID, int numOfBlocks)
{

    if (pools[poolID].tree.root == NULL) {
        fprintf(stderr, "[pool]: fatal error, root = NULL\n");
        exit(-1);
    }

    // find a free node, whose has at least numOfBlocks blocks
    BNode* freeNode = btreeQuery(&pools[poolID].tree, numOfBlocks);

    // This should not happen, this function should only be called when pool has free block's
    if (freeNode == NULL || freeNode->nBlock < numOfBlocks)
	{
        fprintf(stderr, "[pool]: fatal error\n");
        exit(-1);
    }

    btreeDelete(&pools[poolID].tree, freeNode);

    // no extra blocks in free node
    if (freeNode->nBlock == numOfBlocks)
        return (void *)(freeNode->segMin);

    // we have some extra blocks in free node, re-add those back to tree
    uintptr_t segMin = freeNode->segMin + (numOfBlocks * (1 << poolID));
    uintptr_t segMax = freeNode->segMax;
    uintptr_t nBlock = freeNode->nBlock - numOfBlocks;

    btreeInsert(&pools[poolID].tree, segMin, segMax, nBlock);

    return (void *)(freeNode->segMin);
}

// add's a (2 ^ poolID * numOfBlocks) bytes of memory to pool
// poolID = pool index
// numOfBlocks = number of blocks to add
// blk = memory location of starting block
void poolBlockAdd(int poolID, int numOfBlocks, void* blk)
{
    // we need to insert this block back to pool, and join any left and right
    // continuous blocks

    // The node to insert
    uintptr_t segMin = (uintptr_t)(blk);
    uintptr_t segMax = segMin + (numOfBlocks * (1 << poolID)) - 1;
    uintptr_t nBlock = numOfBlocks;

    {
        // find, if we have a right neighbouring block in tree, if yes combine it
        BNode* nei = btreeFindNode(&pools[poolID].tree, segMax + 1, 0);
        if (nei)
		{
            btreeDelete(&pools[poolID].tree, nei);
            segMax = nei->segMax;
            nBlock += nei->nBlock;
        }
    }

    {
        // find, if we have a left neighbouring block in tree, if yes combine it
        BNode* nei = btreeFindNode(&pools[poolID].tree, 0, segMin - 1);
        if (nei)
		{
            btreeDelete(&pools[poolID].tree, nei);
            segMin = nei->segMin;
            nBlock += nei->nBlock;
        }
    }

    btreeInsert(&pools[poolID].tree, segMin, segMax, nBlock);
}

// returns a memory block of size (2 ^ poolID * numOfBlocks)
// poolID = pool id
// numOfBlocks = number of blocks to allocate
void* poolAllocate(int poolID, int numOfBlocks)
{
    if (!(poolID >= MIN_POOL_ID && poolID <= MAX_POOL_ID))
        return NULL;

    // check if pool[poolID], is having free continuos numOfBlocks blocks
    if (poolBlockAvailable(poolID, numOfBlocks))
	{
        return poolBlockRemove(poolID, numOfBlocks);
    }

    // ask right side neighboring pools
    for (int i = poolID + 1; i <= MAX_POOL_ID; ++i)
	{
        int d = (1 << (i - poolID));
        int r = (numOfBlocks / d) + ((numOfBlocks % d)? 1: 0);
        int t = r * (1 << (i - poolID));

        if (poolBlockAvailable(i, r))
		{
            void* blk = poolBlockRemove(i, r);
            poolBlockAdd(poolID, t, blk);
            return poolBlockRemove(poolID, numOfBlocks);
        }
    }

    // ask left side neighboring pools
    for (int i=poolID-1; i >= MIN_POOL_ID; --i)
	{
        int r = numOfBlocks * (1 << (poolID - i));
        int t = numOfBlocks;

        if (poolBlockAvailable(i, r))
		{
            void* blk = poolBlockRemove(i, r);
            poolBlockAdd(poolID, t, blk);
            return poolBlockRemove(poolID, numOfBlocks);
        }
    }

    return NULL;
}

// deallocate's previously allocated memory block
void poolDeallocate(int poolID, int numOfBlocks, void* addr)
{
    if (!(poolID >= MIN_POOL_ID && poolID <= MAX_POOL_ID)) 
	{
        fprintf(stderr, "[POOL]: fatal error, deleting block from wrong pool\n");
        exit(-1);
    }

    poolBlockAdd(poolID, numOfBlocks, addr);
}

void poolInit(int poolID, int numOfBlocks, void* addr)
{
    for (int i=0; i <= MAX_POOL_ID; ++i)
	{
        pools[i].tree.root = NULL;
    }
    poolBlockAdd(poolID, numOfBlocks, addr);
}


