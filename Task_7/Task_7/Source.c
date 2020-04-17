#include <malloc.h>
#include "hash_table.h"


int main()
{
    printf("The program solves the \"Hash Table\" task; balancing occurs i"
        "f there are more than 4 elements in one of the hash table lists\n");
    int size = 13;
    int* p = &size;
    Node* hash = (Node*)malloc(sizeof(Node) * size);
    hash_initialization(hash, size);
    for (int i = 0; i < size * 2; i += 2)
    {
        add(hash, i, i, size, p);
    }
    add(hash, 26, 66, size, p);
    add(hash, 28, 27, size, p);
    add(hash, 30, 53, size, p);
    print_table(hash, size);
    printf("\n");
    add(hash, 32, 79, size, p);
    print_table(hash, size);
    printf("\n");
    printf("%d\n", find(hash, 53, size));

    rm(hash, 53, size);
    rm(hash, 27, size);
    rm(hash, 66, size);

    print_table(hash, size);
    printf("%d\n", find(hash, 53, size));
}