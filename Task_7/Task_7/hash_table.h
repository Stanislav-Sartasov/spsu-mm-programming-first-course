typedef struct Node Node;
struct Node {
    int key;
    int value;
    struct Node* next;
};


int hash_function(int x, int size);

void hash_initialization(Node** hash_table, int size);

int check(Node** hash_table, int size, int index);

void add(Node** hash_table, int value, int key, int size, int* p);

int find(Node** hash_table, int key, int size);

void rm(Node** hash_table, int key, int size);

void print_table(Node** hash_table, int size);

void copy(Node** hash_table, Node** hash_new, int size, int flag);

void balancing(Node** hash_table, int* size);

