#include "btree.h"

#include <stdint.h>
#include <string.h>
#include <stdio.h>

static size_t max_t(size_t a, size_t b)
{
    return (a >= b) ? a : b;
}

static void updateNodeNBlockMax(BNode* node)
{
    node->nBlockMax = node->nBlock;
    if (node->left)
        node->nBlockMax = max_t(node->nBlockMax, node->left->nBlockMax);
    if (node->right)
        node->nBlockMax = max_t(node->nBlockMax, node->right->nBlockMax);
}

static BNode* subtreeMinimum(BNode* node) 
{
    while (node->left != NULL)
        node = node->left;
    return node;
}

static void insertImpl(BNode* cur, BNode* node) 
{
    if (node->segMin < cur->segMin) {
        // node goes to left subtree of cur
        if (cur->left == NULL) {
            cur->left = node;
            node->parent = cur;
        } else {
            insertImpl(cur->left, node);
        }
    } else if (node->segMin > cur->segMin) 
	{
        // node goes to right subtree
        if (cur->right == NULL) {
            cur->right = node;
            node->parent = cur;
        } else 
		{
            insertImpl(cur->right, node);
        }
    } else {
        // this should never happen, because we dont have overlapping segments
        fprintf(stderr, "btreeInsert fatal error\n");
        exit(-1);
    }

    // calculate the maximum segment size subtree of cur node
    updateNodeNBlockMax(cur);
}

void btreeInsert(BTree* tree, uintptr_t segMin, uintptr_t segMax, size_t nBlock)
{
    if ((segMax - segMin + 1) < sizeof(BNode))
	{
        fprintf(stderr, "insert: size < sizeof(BNode)");
        while (1);
    }

    BNode* node = (BNode*) (segMin);

    node->parent = NULL;
    node->left   = NULL;
    node->right  = NULL;
    node->segMin = segMin;
    node->segMax = segMax;
    node->nBlock = nBlock;
    node->nBlockMax = nBlock;

    if (tree->root == NULL)
	{
        tree->root = node;
        return;
    }

    insertImpl(tree->root, node);
}

static void replace(BTree* tree, BNode* u, BNode* v) 
{
    if (u->parent == NULL)
        tree->root = v;
    else if (u == u->parent->left)
        u->parent->left = v;
    else
        u->parent->right = v;

    if (v != NULL)
        v->parent = u->parent;

}

static void updateTreeNBlockMax(BNode* node)
{
    if (node->left)
        updateTreeNBlockMax(node->left);

    if (node->right)
        updateTreeNBlockMax(node->right);

    updateNodeNBlockMax(node);
}

void btreeDelete(BTree* tree, BNode* z)
{
    if (z->left == NULL)
	{
        replace(tree, z, z->right);
    }
    else if(z->right == NULL)
	{
        replace(tree, z, z->left);
    }
    else
	{
        BNode* y = subtreeMinimum(z->right);
        if (y->parent != z)
		{
            replace(tree, y, y->right);
            y->right = z->right;
            y->right->parent = y;
        }
        replace(tree, z, y);
        y->left = z->left;
        y->left->parent = y;
    }

    if (tree->root)
        updateTreeNBlockMax(tree->root);
}


static int findNodeAreEqual(BNode* cur, uintptr_t segMin, uintptr_t segMax)
{

    if (segMin != 0 && segMax != 0)
        return (cur->segMin == segMin) && (cur->segMax == segMax);
    if (segMin != 0)
        return cur->segMin == segMin;
    if (segMax != 0)
        return cur->segMax == segMax;
    return 0;
}

static BNode* btreeFindNodeImpl(BNode* cur, uintptr_t segMin, uintptr_t segMax)
{
    if (findNodeAreEqual(cur, segMin, segMax))
        return cur;

    uintptr_t cmpValue = segMin ? segMin : segMax;

    if (cmpValue < cur->segMin && cur->left)
        return btreeFindNodeImpl(cur->left, segMin, segMax);
    if (cmpValue > cur->segMin && cur->right)
        return btreeFindNodeImpl(cur->right, segMin, segMax);

    return NULL;
}

BNode* btreeFindNode(BTree* tree, uintptr_t segMin, uintptr_t segMax)
{
    if (tree->root == NULL)
        return  NULL;
    return btreeFindNodeImpl(tree->root, segMin, segMax);
}

BNode* btreeQueryImpl(BNode* node, size_t nBlock)
{

    if(node->nBlockMax < nBlock)
        return NULL;

    BNode* res = NULL;

    if (node->left && node->right && node->left->nBlockMax >= nBlock
        && node->right->nBlockMax >= nBlock)
	{

        if (node->left->nBlockMax <= node->right->nBlockMax)
            res = btreeQueryImpl(node->left, nBlock);
        else
            res = btreeQueryImpl(node->right, nBlock);
    }

    else if (node->left && node->left->nBlockMax >= nBlock)
        res = btreeQueryImpl(node->left, nBlock);
    else if (node->right && node->right->nBlockMax >= nBlock)
        res = btreeQueryImpl(node->right, nBlock);

    if (node->nBlock >= nBlock)
	{
        if (!res)
            res = node;
        else 
		{
            if (node->nBlock <= res->nBlock)
                res = node;
        }
    }

    // This should never happen
    if (res == NULL) {
        fprintf(stderr, "find fatal error\n");
        exit(-1);
    }

    return res;
}

BNode* btreeQuery(BTree* tree, size_t nBlock)
{
    if (tree->root == NULL)
        return NULL;
    return btreeQueryImpl(tree->root, nBlock);
}
