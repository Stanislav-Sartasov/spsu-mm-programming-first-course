#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define MAX_MERORY_SIZE			10 * 1024 * 1024	// (100MB)
#define MAX_ENTRY_COUNT			MAX_MERORY_SIZE / 4

struct MemoryEntry
{
	char*			postion;
	unsigned int	size;
};

char* g_memory_buffer;
struct MemoryEntry* g_malloc_memory_list;
unsigned int g_malloc_memory_entry_count;
struct MemoryEntry* g_free_memory_list;
unsigned int g_free_memory_entry_count;

void init()
{
	g_memory_buffer = (char*)malloc(MAX_MERORY_SIZE);

	g_malloc_memory_list = (struct MemoryEntry*)malloc(MAX_ENTRY_COUNT * sizeof(struct MemoryEntry));
	g_malloc_memory_entry_count = 0;

	g_free_memory_list = (struct MemoryEntry*)malloc(MAX_ENTRY_COUNT * sizeof(struct MemoryEntry));
	g_free_memory_list[0].postion = g_memory_buffer;
	g_free_memory_list[0].size = MAX_MERORY_SIZE;
	g_free_memory_entry_count = 1;
}

void end()
{
	if (g_memory_buffer)
	{
		free(g_memory_buffer);
	}
	if (g_malloc_memory_list)
	{
		free(g_malloc_memory_list);
	}
	if (g_free_memory_list)
	{
		free(g_free_memory_list);
	}
}

void* myMalloc(size_t size)
{
	unsigned int i;
	char* return_pos;

	for (i = 0 ; i < g_free_memory_entry_count ; i = i + 1)
	{
		if (g_free_memory_list[i].size >= (unsigned int)size)
		{
			// get freed memory region
			return_pos = g_free_memory_list[i].postion;
			g_free_memory_list[i].postion = return_pos + size;
			g_free_memory_list[i].size -= size;

			// register to malloc memory list
			g_malloc_memory_list[g_malloc_memory_entry_count].postion = return_pos;
			g_malloc_memory_list[g_malloc_memory_entry_count].size = size;
			g_malloc_memory_entry_count = g_malloc_memory_entry_count + 1;

			return return_pos;
		}
	}

	return NULL;
}

void myFree(void* ptr)
{
	unsigned int i;

	for (i = 0; i < g_malloc_memory_entry_count ; i++)
	{
		if (g_malloc_memory_list[i].postion == (char*)ptr)
		{
			// register to free memory list
			g_free_memory_list[g_free_memory_entry_count].postion = (char*)ptr;
			g_free_memory_list[g_free_memory_entry_count].size = g_malloc_memory_list[i].size;
			g_free_memory_entry_count = g_free_memory_entry_count + 1;

			// clean malloced memory region
			g_malloc_memory_list[i].postion = NULL;
			g_malloc_memory_list[i].size = 0;

			//decrease malloced memory entry count
			if (i + 1 < g_malloc_memory_entry_count)
			{
				memmove(&g_malloc_memory_list[i], &g_malloc_memory_list[i+1], sizeof(struct MemoryEntry) * (g_malloc_memory_entry_count - (i + 1)));
			}
			g_malloc_memory_entry_count = g_malloc_memory_entry_count - 1;

			break;
		}
	}
}

void* myRealloc(void* ptr, size_t size)
{
	char* new_mem;
	unsigned int i;

	// find old size
	for (i = 0; i < g_malloc_memory_entry_count ; i++)
	{
		if (g_malloc_memory_list[i].postion == (char*)ptr)
		{
			new_mem = (char*)myMalloc(size);
			memmove(new_mem, ptr, g_malloc_memory_list[i].size);
			myFree(ptr);

			return new_mem;
		}
	}

	return NULL;
}

void printf_memory_state()
{
	unsigned int i;

	printf("current allocated memory list\n");
	for (i = 0 ; i < g_malloc_memory_entry_count ; i++)
	{
		printf("%x - %d\n", (int)g_malloc_memory_list[i].postion, g_malloc_memory_list[i].size);
	}

	printf("current deallocated memory list\n");
	for (i = 0 ; i < g_free_memory_entry_count ; i++)
	{
		printf("%x - %d\n", (int)g_free_memory_list[i].postion, g_free_memory_list[i].size);
	}
	printf("\n");
}

int main()
{
	int i;
	void* ptr[10];

	init();

	printf("after initialization\n");
	printf_memory_state();

	for (i = 0 ; i < 10 ; i++)
	{
		ptr[i] = myMalloc(1000);
	}

	printf("after allocation\n");
	printf_memory_state();

	for (i = 0 ; i < 10 ; i++)
	{
		myFree(ptr[i]);
	}

	printf("after free\n");
	printf_memory_state();

	end();
	return 0;
}