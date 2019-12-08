#pragma once
static int amount_of_space = 10000;
static void *memory;
static int has_initialized = 0;

typedef struct mem_block
{
	size_t size_of_block;
	struct mem_block *next;
	struct mem_block *previous;
} mem_block;

 static mem_block *stack = NULL;


void init();
mem_block *find_free_memory(size_t size);
void delete_block(mem_block *block);
void *myMalloc(size_t size);
void myFree(void *ptr);
int unite(mem_block **first, mem_block **second);
void *myRealloc(void *ptr, size_t newSize);