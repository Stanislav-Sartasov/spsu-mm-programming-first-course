#include "hashTable.h"



int main()
{
    int key, value;
    key = 5;
    value = 4;
    node* nNode = createNode(key, value);
    for (int i = 0; i < 10; i++)
    {
        nodePushback(i, 10 - i, nNode); 
    }
    while (nNode->link)
    {
        printf("%d %d \n", nNode->key, nNode ->value);
        nNode = nNode->link;
    }


    return 0;
}