
static int **hashTable;
static const int n = 10;
static int ptr = 1;

static struct hash
{
	int key;
	int value;
};


void init(int a, int b);
int hash_func(int key);
void add(struct hash a);
int rebalance(unsigned int choice);
void find(struct hash a);
void delite(struct hash a);
void myFree();