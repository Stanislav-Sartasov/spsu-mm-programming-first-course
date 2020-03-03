#define _CRT_SECURE_NO_WARNINGS
#include "Header.h"
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>

int main()
{
	hash_table* map = init(1);
	add(&map, 15, 10);
	add(&map, 5, 10);
	add(&map, 10, 20);
	add(&map, 11, 20);
	add(&map, 12, 20);
	add(&map, 13, 20);
	add(&map, 14, 20);
	add(&map, 16, 20);
	add(&map, 17, 20);
	add(&map, 18, 20);
	add(&map, 19, 20);
	add(&map, 20, 20);
	add(&map, 21, 20);
	add(&map, 22, 20);


	find(&map, 10);
	printf("\n");
	find(&map, 20);
	delete(map, 10);
	printf("\n");
	find(&map, 10);
	delete(map, 10);
	find(&map, 10);

	delete_hash_table(&map);

	return 0;
}