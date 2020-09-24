#include "hashTable.h"

int insert(int key, int value, node** hashTable)
{
    if (hashTable[key %13])
    {
        int flag = 0;

        node* c = hashTable[key %13];
        while (c->link)
        {
            if (c->key == key)
                flag = 1;
            c = c->link;
        }
        if (c->key == key)
            flag = 1;
        if (flag)
        {
            c->link = createNode(key, value);
            return 0;
        }
        else 
            return 1;  
    }
    else
    {
        hashTable[key %13]= createNode(key, value);
        return 0;
    }
}