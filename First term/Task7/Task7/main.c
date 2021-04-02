#include "hash_table.h"

int main()
{
	printf("hash table built by double hashing method\n");

	struct Hash_table table = initialization(5);
	add_value(&table, 100);
	add_value(&table, 151);
	add_value(&table, 12);
	add_value(&table, 122);
	find_value(&table, 12);
	find_value(&table, 122);
	find_value(&table, 129);
	delete_value(&table, 100);
	delete_value(&table, 101);
	delete_value(&table, 122);
	find_value(&table, 100);
	output(&table);
	return 0;
}
