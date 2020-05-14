typedef struct Node Node;
typedef struct Hash Hash;

struct Node
{
    int key;
    int value;
    struct Node* next;
};

struct Hash
{
    int size;
    Node* hash_list;
};

Hash hash_initialization(int size);

void add(Hash* hash_table, int value, int key);

int find(Hash* hash, int key);

void rm(Hash* hash, int key);

void print_table(Hash* hash_table);
