#include <stdio.h>
#include <stdlib.h>

#define memory_size (sizeof(char) * 20971522)

void* mem_start = 0;

void init()
{
	mem_start = malloc(memory_size);
	((size_t*)mem_start)[0] = memory_size;
	((size_t*)mem_start)[1] = 0;
	((char*)mem_start)[8] = 0;
}

void* my_malloc(size_t size)
{
	if (!mem_start)
		init();
	void* point = mem_start;
	size_t pre_size = 0;
	while (((char*)point)[8] || ((size_t*)point)[0] - 9 < size)
	{
		pre_size = ((size_t*)point)[0];
		(char*)point += pre_size;
		if ((char*)point >= (char*)mem_start + memory_size)
			return 0;
	}
	((char*)point)[8] = 1;
	if (((size_t*)point)[0] > size + 18)
	{
		((size_t*)((char*)point + size + 9))[0] = ((size_t*)point)[0] - size - 9;
		((size_t*)((char*)point + size + 9))[1] = size + 9;
		((char*)point + size + 9)[8] = 0;
		((size_t*)point)[0] = size + 9;
	}
	return (void*)((char*)point + 9);
}

void my_free(void* ptr)
{
	(char*)ptr -= 9;
	((char*)ptr)[8] = 0;
	if ((char*)ptr + ((size_t*)ptr)[0] < (char*)mem_start + memory_size)
		if (!((char*)ptr + ((size_t*)ptr)[0])[8])
			((size_t*)ptr)[0] += ((size_t*)((char*)ptr + ((size_t*)ptr)[0]))[0];
	if (((size_t*)ptr)[1])
		if (!((char*)ptr - ((size_t*)ptr)[1])[8])
			((size_t*)((char*)ptr - ((size_t*)ptr)[1]))[0] += ((size_t*)ptr)[0];
}

void* my_realloc(void* ptr, size_t size)
{
	(char*)ptr -= 9;
	void* new_ptr = 0;
	size_t old_size = ((size_t*)ptr)[0];
	size_t pre_size = ((size_t*)ptr)[1];
	my_free((char*)ptr + 9);
	new_ptr = (char*)my_malloc(size);
	if (!new_ptr)
	{
		if (pre_size)
			((size_t*)((char*)ptr - pre_size))[0] = pre_size;
		((size_t*)ptr)[0] = old_size;
		((char*)ptr)[8] = 1;
		return 0;
	}
	(char*)ptr += 9;
	for (int i = 0; i < size && i < old_size - 9; i++)
		((char*)new_ptr)[i] = ((char*)ptr)[i];
	return new_ptr;
}