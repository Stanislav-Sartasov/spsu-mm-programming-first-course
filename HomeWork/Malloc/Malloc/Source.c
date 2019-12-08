#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include<malloc.h>
#define endl printf("\n")

int amount_of_space = 10000;
void *memory;
int has_initialized = 0;

typedef struct mem_block
{
	size_t size_of_block;
	struct mem_block *next;
	struct mem_block *previous;
} mem_block;

mem_block *stack = NULL;

void init()
{
	has_initialized = 1;
	memory = malloc(amount_of_space);
	mem_block *block = (mem_block*)memory;
	block->next = NULL;
	block->previous = NULL;
	block->size_of_block = amount_of_space;
	stack = block;
}

mem_block *find_free_memory(size_t size)
{
	mem_block *tmp = stack;
	while (tmp != NULL)
	{
		if (tmp->size_of_block >= size + sizeof(size_t)) return tmp;
		tmp = tmp->next;
	}
	return NULL;
}

void delete_block(mem_block *block)
{
	if (block = stack)
	{
		stack = block->next;
		return;
	}
	else if (block->next == NULL)
	{
		block->previous->next = NULL;
		return;
	}
	block->previous->next = block->next;
	block->next->previous = block->previous; 
}

void *myMalloc(size_t size)
{
	if (has_initialized == 0) init();

	mem_block *block = find_free_memory(size);
	if (block == NULL) return NULL;
	if (size == block->size_of_block)
	{
		delete_block(block);
		return (char*)block + sizeof(size_t);
	}

	block->size_of_block = block->size_of_block - sizeof(size_t) - size;
	mem_block *new = (mem_block*)((char *)block + block->size_of_block);
	new->size_of_block = size + sizeof(size_t);
	return (char *)new + sizeof(size_t);

}

void myFree(void *ptr)
{
	mem_block *block = (mem_block*)((char*)ptr - sizeof(size_t));
	if (stack == NULL)
	{
		block->previous = NULL;
		block->next = NULL;
		stack = block;
		return 0;
	}

	mem_block *tmp = stack;
	while ((tmp->next != NULL) && (tmp->next < block))	tmp = tmp->next;
	
	if ((tmp->previous != NULL) && (tmp->next != NULL))
	{
		tmp->next->previous = block;
		tmp->previous->next = block;
	}
	else if (tmp->previous != NULL)
	{
		tmp->previous->next = block;
	}
	else if (tmp->next != NULL)
	{
		tmp->next->previous = block;
	}
	if ((block->next != NULL) && (block->next == (char *)block + sizeof(block)))
	{
		unite(&block, &(block->next));
	}
	if ((block->previous != NULL) && (block->previous == (char *)block - sizeof(block)))
	{
		unite((&block->previous), &block);
	}


}

int unite(mem_block **first, mem_block **second)
{
	(*first)->next = (*second)->next;
	if ((*second)->next != NULL)
	{
		(*second)->next->previous = (*first);
	}
	(*first)->size_of_block = (*first)->size_of_block + (*second)->size_of_block;
}


void *myRealloc(void *ptr, size_t newSize)
{
	mem_block *block = (char *)ptr - sizeof(size_t);

	if (block->size_of_block - sizeof(size_t) >= newSize) return ptr;

	mem_block *new = myMalloc(newSize);
	if (new == NULL) return NULL;

	memcpy(new, ptr, block->size_of_block - sizeof(size_t));
	myFree(ptr);
	return new;
}

int main()
{
	int *mas1 = myMalloc(10 * sizeof(int));
	for (int i = 0; i < 10; i++)
	{
		mas1[i] = 1;
		printf("%d", mas1[i]);
	}
	endl;
	char *mas2 = myMalloc(10 * sizeof(char));
	for (int i = 0; i < 10; i++)
	{
		mas2[i] = 'a';
		printf("%c", mas2[i]);
	}
	endl;

	endl;

	for (int i = 0; i < 10; i++) printf("%d", mas1[i]);
	endl;
	for (int i = 0; i < 10; i++)printf("%c", mas2[i]);

	endl; endl;
	float *mas3 = myMalloc(5 * sizeof(float));
	for (int i = 0; i < 5; i++)
	{
		mas3[i] = 1.1 + i;
		printf("%f%c", mas3[i], ' ');
	}

	myFree(mas2);

	mas1 = myRealloc(mas1, 15 * sizeof(int));
	mas3 = myRealloc(mas3, 10 * sizeof(float));
	endl;

	for (int i = 10; i < 15; i++) mas1[i] = 1;
	for (int i = 0; i < 15; i++) printf("%d", mas1[i]);
	endl; endl;

	for (int i = 5; i < 10; i++) mas3[i] = 1.1 + i;
	for (int i = 0; i < 10; i++) printf("%f%c", mas3[i], ' ');
	endl;

	system("pause");
}

