#include "allocator.h"
#define MEMORY_SIZE 1024

void* memory;
int flag = 0;

void init()
{
	memory = malloc(MEMORY_SIZE);
	((size_t*)memory)[0] = ((size_t*)memory)[MEMORY_SIZE / 4 - 2] = MEMORY_SIZE;
	((size_t*)memory)[1] = ((size_t*)memory)[MEMORY_SIZE / 4 - 1] = 0;
	flag = 1;
}

void uninit()
{
	free(memory);
	flag = 0;
}

void* findFreeMemory(size_t size)
{
	for (int i = 0; i < MEMORY_SIZE / 4; i += ((size_t*)memory)[i] / 4)
	{
		if ((((size_t*)memory)[i] - 16 >= size) && (((size_t*)memory)[i + 1] == 0))
			return (void*)((size_t*)memory + i);
	}
	return NULL;
}

void coalescing(size_t* ptr)
{
	int size = ptr[0] / 4;
	if (ptr > memory)
	{
		if (!ptr[-1])
		{
			int prev_size = ptr[-2] / 4;
			ptr[size - 2] = ptr[-prev_size] = (size + prev_size) * 4;
			ptr -= prev_size;
			size += prev_size;
		}
	}
	if (ptr + size - 1 < ((size_t*)memory + MEMORY_SIZE / 4 - 1))
	{
		if (!ptr[size + 1])
		{
			int next_size = ptr[size] / 4;
			ptr[0] = ptr[size + next_size - 2] = (size + next_size) * 4;
		}
	}

}

void* myMalloc(size_t size)
{
	if (!flag)
		init();
	if (size > MEMORY_SIZE - 16)
		return NULL;
	int blocks = (size + 3) / 4;
	size_t* ptr = (size_t*)findFreeMemory(size);
	if (!ptr)
	{
		return NULL;
	}
	else
	{
		if (ptr[0] / 4 - 4 - blocks >= 4)
		{
			ptr[blocks + 4] = ptr[ptr[0] / 4 - 2] = ptr[0] - (blocks + 4) * 4;
			ptr[blocks + 5] = 0;
			ptr[0] = ptr[blocks + 2] = (blocks + 4) * 4;
			ptr[1] = ptr[blocks + 3] = 1;
			return (void*)(ptr + 2);
		}
		else
		{
			ptr[1] = ptr[ptr[0] / 4 - 1] = 1;
			return (void*)(ptr + 2);
		}
	}
}

void myFree(void* ptr)
{
	if (!ptr)
		return;
	size_t* p = (size_t*)ptr - 2;
	size_t size = p[0] / 4;
	p[1] = p[size - 1] = 0;
	coalescing(p);
}

void* myRealloc(void* ptr, size_t size)
{
	if (size == 0)
	{
		myFree(ptr);
		return NULL;
	}

	if (!ptr || *((size_t*)ptr - 1) == 0)
		return myMalloc(size);
	size_t* p = (size_t*)ptr - 2;
	size_t block_size = p[0] / 4;
	if ((block_size - 4) * 4 >= size)
		return (void*)(p + 2);
	else
	{
		if (p + block_size - 1 < ((size_t*)memory + MEMORY_SIZE / 4 - 1))
		{
			int next_size = p[block_size] / 4;
			if (!p[block_size + 1] && (next_size + block_size - 4) * 4 > size)
			{
				int blocks = (size + 3) / 4;
				p[0] = p[block_size + next_size - 2] = (block_size + next_size) * 4;
				p[block_size + next_size - 1] = 1;
				block_size += next_size;
				if (p[0] - 16 - size >= 16)
				{
					p[blocks + 4] = p[block_size - 2] = (block_size - blocks - 4) * 4;
					p[blocks + 5] = p[block_size - 1] = 0;
					p[0] = p[blocks + 2] = block_size * 4 + 16;
					p[1] = p[blocks + 3] = 1;
				}
				return (void*)(p + 2);
			}
		}
		size_t* new_ptr = (size_t*)myMalloc(size);
		if (new_ptr)
		{
			memcpy(new_ptr, ptr, p[0] - 16);
			myFree(ptr);
			return (void*)(new_ptr);
		}
		return (void*)(p + 2);
	}
}

