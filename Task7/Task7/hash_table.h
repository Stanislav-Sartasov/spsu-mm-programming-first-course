
struct Cell
{
	int value;  
	/* ќтмечаем удаленные элементы(просто удалить элемент мы не можем)
	0 - элемент отсутсвует
	1 - элемент удален	
	2 - элемент находитс€ в €чейке */
	int remote;
};

struct Hash_table
{
	int size;
	int full;
	struct Cell* hash_list;
};


struct Hash_table initialization(int size);

void add_value(struct Hash_table* table, int value);

void delete_value(struct Hash_table* table, int value);

void find_value(struct Hash_table* table, int value);

void output(struct Hash_table* table);
