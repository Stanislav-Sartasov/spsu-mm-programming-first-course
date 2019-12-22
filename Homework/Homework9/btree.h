#ifndef TASKS_BTREE_H
#define TASKS_BTREE_H

#undef min
#undef max

#include <stdint.h>
#include <stdlib.h>

struct bnodeDt
{
    struct bnodeDt* parent;
    struct bnodeDt* left;
    struct bnodeDt* right;

    // holds integer pointer value of start of memory block
    // This node will be stored in this memory location.
    uintptr_t segMin;

    // holds integer pointer value of end of memory block
    uintptr_t segMax;

    // each btree is associated with a size P, which is a power of 2.
    // nBlock holds, number of memory blocks of size P, in [segMax - segMin + 1]
    size_t    nBlock;

    // nBlockMax stores the maximum nBlock of subtree, starting from this node.
    size_t    nBlockMax;
};

struct btreeDt
{
    struct bnodeDt* root;
};

typedef struct bnodeDt BNode;
typedef struct btreeDt BTree;

void btreeInsert(BTree* tree, uintptr_t segMin, uintptr_t segMax, size_t nBlock);
void btreeDelete(BTree* tree, BNode* z);

// finds a node, with value equal to segMin and segMax
// if segMin == 0: only find node with same segMax
// if segMax == 0: only find node with same segMin
BNode* btreeFindNode(BTree* tree, uintptr_t segMin, uintptr_t segMax);

// finds a node, whose node->nBlock >= nBlock
BNode* btreeQuery(BTree* tree, size_t nBlock);

#endif //TASKS_BTREE_H
