#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include<malloc.h>
#define amount_of_space 10000
#define endl printf("\n")
int stack[amount_of_space];
int free_space = amount_of_space, has_initialized = 0;
int amount_of_blocks = 1;
struct mem_block *blocks;

struct mem_block
{
	int loccation;
	int size;
	int available;
};

void init()
{
	for (int i = 0; i < amount_of_space; i++) stack[i] = i;
	
	blocks = (struct mem_block*)malloc(1 * sizeof(struct mem_block));
	blocks[0].available = amount_of_space;
	blocks[0].loccation = 0;
	has_initialized = 1;
}

void* myMalloc(size_t size)
{
	if (size <= 0)
	{
		printf("%s", "Size can't be less than 0");
		return NULL;
	}
	if (has_initialized == 0) init();

	void *bp;
	int i;
	i = allocate(size);

	if (i == -1) return NULL;
	bp = &stack[blocks[i].loccation];
	return bp;
}

int allocate(size_t size)
{
	if (size > free_space)
	{
		printf("%s", "You have no free memory");
		return -1;
	}

	for (int i = 0; i < amount_of_blocks; i++)
	{
		if (blocks[i].available == size)
		{
			blocks[i].available = 0;
			blocks[i].size = size;
			if (i != 0) blocks[i].loccation = blocks[i - 1].loccation + blocks[i - 1].size;
			else blocks[i].loccation = 0;
			free_space -= size;
			return i;
		}
	}

	for (int i = 0; i < amount_of_blocks; i++)
	{
		if (blocks[i].available > size)
		{
			free_space -= size;
			split_blocks(i, size);
			return i;
		}
	}

}

int split_blocks(int ptr, size_t size)
{
	amount_of_blocks++;
	blocks = (struct mem_block*)realloc(blocks, amount_of_blocks * sizeof(struct mem_block));
	for (int i = amount_of_blocks - 1; i >= ptr; i--)
	{
		blocks[i].size = blocks[i - 1].size;
		blocks[i].loccation = blocks[i - 1].loccation;
		blocks[i].available = blocks[i - 1].available;
	}
	blocks[ptr + 1].available = blocks[ptr].available - size;
	blocks[ptr].available = 0;
	blocks[ptr].size = size;
	if (ptr != 0) blocks[ptr].loccation = blocks[ptr - 1].loccation + blocks[ptr - 1].size;
	else blocks[ptr].loccation = 0;
	return 0;
}

void myFree(void* bp)
{
	int ptr;
	for (int i = 0; i < amount_of_blocks; i++)
	{
		if (bp == &stack[blocks[i].loccation])
		{
			ptr = i;
			break;
		}
	}
	blocks[ptr].available = blocks[ptr].size;
	blocks[ptr].size = 0;
	combine_blocks(ptr);
}

int combine_blocks(int ptr)
{
	if (blocks[ptr - 1].available > 0 && blocks[ptr + 1].available > 0)
	{
		
		struct mem_block *tmp_blocks = (struct mem_block*)malloc((amount_of_blocks - 2) * sizeof(struct mem_block));
		blocks[ptr - 1].available += blocks[ptr].available + blocks[ptr + 1].available;
		free_space += blocks[ptr - 1].available;

		for (int i = 0; i <= ptr - 1; i++)
		{
			tmp_blocks[i].available = blocks[i].available;
			tmp_blocks[i].loccation = blocks[i].loccation;
			tmp_blocks[i].size = blocks[i].size;
		}

		for (int i = ptr + 2; i < amount_of_blocks; i++)
		{
			tmp_blocks[i - 2].available = blocks[i].available;
			tmp_blocks[i - 2].loccation = blocks[i].loccation;
			tmp_blocks[i - 2].size = blocks[i].size;
		}

		amount_of_blocks -= 2;
		blocks = (struct mem_block*)realloc(blocks, amount_of_blocks * sizeof(struct mem_block));

		for (int i = 0; i <amount_of_blocks; i++)
		{
			blocks[i].available = tmp_blocks[i].available;
			blocks[i].loccation = tmp_blocks[i].loccation;
			blocks[i].size = tmp_blocks[i].size;
		}

		free(tmp_blocks);
	}
	
	else if (blocks[ptr - 1].available > 0)
	{
		struct mem_block *tmp_blocks = (struct mem_block*)malloc((amount_of_blocks - 1) * sizeof(struct mem_block));
		blocks[ptr - 1].available += blocks[ptr].available;
		free_space += blocks[ptr - 1].available;

		for (int i = 0; i <= ptr - 1; i++)
		{
			tmp_blocks[i].available = blocks[i].available;
			tmp_blocks[i].loccation = blocks[i].loccation;
			tmp_blocks[i].size = blocks[i].size;
		}

		for (int i = ptr + 1; i < amount_of_blocks; i++)
		{
			tmp_blocks[i - 1].available = blocks[i].available;
			tmp_blocks[i - 1].loccation = blocks[i].loccation;
			tmp_blocks[i - 1].size = blocks[i].size;
		}

		amount_of_blocks--;
		blocks = (struct mem_block*)realloc(blocks, amount_of_blocks * sizeof(struct mem_block));

		for (int i = 0; i < amount_of_blocks; i++)
		{
			blocks[i].available = tmp_blocks[i].available;
			blocks[i].loccation = tmp_blocks[i].loccation;
			blocks[i].size = tmp_blocks[i].size;
		}

		free(tmp_blocks);
	}

	else if (blocks[ptr + 1].available > 0)
	{
		struct mem_block *tmp_blocks = (struct mem_block*)malloc((amount_of_blocks - 1) * sizeof(struct mem_block));
		blocks[ptr].available += blocks[ptr + 1].available;
		free_space += blocks[ptr].available;

		for (int i = 0; i <= ptr; i++)
		{
			tmp_blocks[i].available = blocks[i].available;
			tmp_blocks[i].loccation = blocks[i].loccation;
			tmp_blocks[i].size = blocks[i].size;
		}

		for (int i = ptr + 1; i < amount_of_blocks; i++)
		{
			tmp_blocks[i - 1].available = blocks[i].available;
			tmp_blocks[i - 1].loccation = blocks[i].loccation;
			tmp_blocks[i - 1].size = blocks[i].size;
		}

		amount_of_blocks--;
		blocks = (struct mem_block*)realloc(blocks, amount_of_blocks * sizeof(struct mem_block));

		for (int i = 0; i < amount_of_blocks; i++)
		{
			blocks[i].available = tmp_blocks[i].available;
			blocks[i].loccation = tmp_blocks[i].loccation;
			blocks[i].size = tmp_blocks[i].size;
		}

		free(tmp_blocks);
	}

	else free_space += blocks[ptr].available;
	return 0;
}

void* myRealloc(void* bp, size_t size) 
{
	myFree(bp);
	bp = myMalloc(size);
	return bp;
}

int main()
{

	int *mas1 = myMalloc(10);
	for (int i = 0; i < 10; i++) printf("%d%c", mas1[i], ' ');
	endl;

	int *mas2 = myMalloc(10);
	for (int i = 0; i < 10; i++) printf("%d%c", mas2[i],' ');
	endl;

	int *mas3 = myMalloc(20);
	for (int i = 0; i < 10; i++) printf("%d%c", mas3[i], ' ');
	endl;

	myFree(mas3);
	//myFree(mas1);
	//myFree(mas2);

	mas2 = myRealloc(mas2, 12);
	for (int i = 0; i < 12; i++) printf("%d%c", mas2[i], ' ');
	endl;

	int *mas4 = myMalloc(10);
	for (int i = 0; i < 10; i++) printf("%d%c", mas4[i], ' ');
	endl;

	int *mas5 = myMalloc(58);
	for (int i = 0; i < 10; i++) printf("%d%c", mas5[i], ' ');
	endl;

	myFree(mas2);
	
	mas5 = myRealloc(mas5,11);
	for (int i = 0; i < 10; i++) printf("%d%c", mas5[i], ' ');

	system("pause");
	return 0;
}