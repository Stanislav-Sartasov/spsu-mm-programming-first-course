#include "allocator.h"
#define MEMORY_SIZE 2048
#define ULL_SIZE sizeof(ull)

void* memory;
int flag = 0;


void init()
{
	memory = malloc(MEMORY_SIZE);
	((ull*)memory)[0] = ((ull*)memory)[MEMORY_SIZE / ULL_SIZE - 2] = MEMORY_SIZE;
	((ull*)memory)[1] = ((ull*)memory)[MEMORY_SIZE / ULL_SIZE - 1] = 0;
	flag = 1;
}

void uninit()
{
	free(memory);
	flag = 0;
}

void* findFreeMemory(ull size)
{
	for (ull i = 0; i < MEMORY_SIZE / ULL_SIZE; i += ((ull*)memory)[i] / ULL_SIZE)
	{
		if ((((ull*)memory)[i] - ULL_SIZE * 4 >= size) && (((ull*)memory)[i + 1] == 0))
			return (void*)((ull*)memory + i);
	}
	return NULL;
}

void coalescing(ull* ptr)
{
	ull size = ptr[0] / ULL_SIZE;
	if (ptr > memory)
	{
		if (!ptr[-1])
		{
			long long prev_size = ptr[-2] / ULL_SIZE;
			ptr[size - 2] = ptr[-prev_size] = (size + prev_size) * ULL_SIZE;
			ptr -= prev_size;
			size += prev_size;
		}
	}
	if (ptr + size - 1 < ((ull*)memory + MEMORY_SIZE / ULL_SIZE - 1))
	{
		if (!ptr[size + 1])
		{
			ull next_size = ptr[size] / ULL_SIZE;
			ptr[0] = ptr[size + next_size - 2] = (size + next_size) * ULL_SIZE;
		}
	}

}

void* myMalloc(size_t size)
{
	if (!flag)
		init();
	if (size > MEMORY_SIZE - ULL_SIZE * 4)
		return NULL;
	long long blocks = (size + ULL_SIZE - 1) / ULL_SIZE;
	ull* ptr = (ull*)findFreeMemory(size);
	if (!ptr)
	{
		return NULL;
	}
	else
	{
		if (ptr[0] / ULL_SIZE - 4 - blocks >= 4)
		{
			ptr[blocks + 4] = ptr[ptr[0] / ULL_SIZE - 2] = ptr[0] - (blocks + 4) * ULL_SIZE;
			ptr[blocks + 5] = 0;
			ptr[0] = ptr[blocks + 2] = (blocks + 4) * ULL_SIZE;
			ptr[1] = ptr[blocks + 3] = 1;
			return (void*)(ptr + 2);
		}
		else
		{
			ptr[1] = ptr[ptr[0] / ULL_SIZE - 1] = 1;
			return (void*)(ptr + 2);
		}
	}
}

void myFree(void* ptr)
{
	if (!ptr)
		return;
	ull* p = (ull*)ptr - 2;
	ull size = p[0] / ULL_SIZE;
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

	if (!ptr || *((ull*)ptr - 1) == 0)
		return myMalloc(size);
	ull* p = (ull*)ptr - 2;
	long long block_size = p[0] / ULL_SIZE;
	if ((block_size - 4) * ULL_SIZE >= size)
		return (void*)(p + 2);
	else
	{
		if (p + block_size - 1 < ((ull*)memory + MEMORY_SIZE / ULL_SIZE - 1))
		{
			long long next_size = p[block_size] / ULL_SIZE;
			if (!p[block_size + 1] && (next_size + block_size - 4) * ULL_SIZE > size)
			{
				long long blocks = (size + ULL_SIZE - 1) / ULL_SIZE;
				p[0] = p[block_size + next_size - 2] = (block_size + next_size) * ULL_SIZE;
				p[block_size + next_size - 1] = 1;
				block_size += next_size;
				if (p[0] - ULL_SIZE * 4 - size >= ULL_SIZE * 4)
				{
					p[blocks + 4] = p[block_size - 2] = (block_size - blocks - 4) * ULL_SIZE;
					p[blocks + 5] = p[block_size - 1] = 0;
					p[0] = p[blocks + 2] = (blocks + 4) * ULL_SIZE;
					p[1] = p[blocks + 3] = 1;
				}
				return (void*)(p + 2);
			}
		}
		ull* new_ptr = (ull*)myMalloc(size);
		if (new_ptr)
		{
			memcpy(new_ptr, ptr, p[0] - ULL_SIZE * 4);
			myFree(ptr);
			return (void*)(new_ptr);
		}
		return (void*)(p + 2);
	}
}