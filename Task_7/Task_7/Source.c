#include "hash_table.h"

int main()
{
    printf("The program solves the \"Hash Table\" task; balancing occurs i"
        "f there are more than 4 elements in one of the hash table lists\n");

    Hash hash = hash_initialization(13);
    for (int i = 0; i < hash.size * 2; i += 2)
    {
        add(&hash, i, i);
    }
    add(&hash, 26, 66);
    add(&hash, 28, 27);
    add(&hash, 30, 53);
    print_table(&hash);
    printf("\n");
    add(&hash, 32, 79);
    print_table(&hash);
    printf("\n");
    printf("%d\n", find(&hash, 53));
    rm(&hash, 53);
    rm(&hash, 27);
    rm(&hash, 66);
    print_table(&hash);
    printf("%d\n", find(&hash, 53));
}
