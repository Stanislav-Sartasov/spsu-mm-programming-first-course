#include "manager.h"

#define ll long long
#define uc unsigned char
#define max(x, y) x > y ? x : y
#define min(x, y) x < y ? x : y

char* memory_block;
char* memory_status;
ll memory_size = 128; // 32 blocks
ll first_adress;
ll control_block_size;

ll find_size(ll index_block)
{
	char prev_size = memory_status[index_block];
	ll curr_block = index_block + 1;
	char size = memory_status[curr_block];
	ll i = 0;
	while (size == prev_size && size == 255)
	{
		prev_size = memory_status[curr_block];
		curr_block++;
		size = memory_status[curr_block];
		i++;
	}
	return i + prev_size;
}

void my_free(void* ptr)
{
	if (ptr == NULL)
		return;
	ll first_byte = (ll)((ll)ptr - first_adress);
	ll delete_block = first_byte / 4;
	if (delete_block != 0 && (memory_status[delete_block - 1] != 0 && memory_status[delete_block - 1] != 1))
	{
		printf("Invalid pointer in function my_free");
		exit(0);
	}
	ll size_deleted_block = find_size(delete_block);
	ll i = 0;
	for (i; i < size_deleted_block; i++)
		memory_status[delete_block + i] = 0;
	ptr = NULL;
}

ll find_place(size_t size)
{
	ll first_free_block = 0;
	while (first_free_block != control_block_size)
	{
		if (memory_status[first_free_block] == 0)
		{
			ll available_size = 4;
			ll curr_block = first_free_block + 1;
			while (memory_status[curr_block] == 0 && available_size < size && curr_block != control_block_size)
			{
				available_size += 4;
				curr_block++;
			}
			if (curr_block == control_block_size && available_size < size)
				return -1;
			if (available_size < size)
			{
				first_free_block = curr_block;
				continue;
			}
			return first_free_block;
		}
		first_free_block++;
	}
	return -1;
}

void* my_malloc(size_t size)
{
	ll right_size = ((ll)size + 3) / 4 * 4;
	ll remaining_blocks = right_size / 4;
	ll first_free_block = find_place(right_size);
	if (first_free_block == -1)
		return NULL;
	ll curr_block = 0;
	while (remaining_blocks != 0)
	{
		memory_status[first_free_block + curr_block] = min(remaining_blocks, 255);
		remaining_blocks--;
		curr_block++;
	}
	return (void*)&memory_block[first_free_block * 4];
}

void* my_realloc(void* ptr, size_t size)
{
	if (size == 0)
	{
		my_free(ptr);
		return;
	}
	if (ptr == NULL)
		return my_malloc(size);
	size = ((ll)size + 3) / 4 * 4;
	size_t remaining_size_in_blocks = size / 4;
	ll first_byte = (ll)((ll)ptr - first_adress);
	ll relocate_block = first_byte / 4;
	if (relocate_block != 0 && (memory_status[relocate_block - 1] != 0 && memory_status[relocate_block - 1] != 1))
	{
		printf("Invalid pointer in function my_realloc");
		exit(0);
	}
	ll size_relocate_block = find_size(relocate_block);
	int* pointer;
	ll curr_block = 0;
	void* p = my_malloc(size);
	if (p == NULL)
		return NULL;
	ll block_for_remove = (ll)((ll)p - first_adress) / 4;
	curr_block = 0;
	remaining_size_in_blocks = size_relocate_block;
	while (remaining_size_in_blocks != 0)
	{
		for (char j = 0; j < 4; j++)
			memory_block[(block_for_remove + curr_block) * 4 + j] = memory_block[(relocate_block + curr_block) * 4 + j];
		remaining_size_in_blocks--;
		curr_block++;
	}
	my_free((void*)(&memory_block[relocate_block * 4]));
	pointer = &memory_block[(block_for_remove) * 4];
	return (void*)pointer;
}

void init()
{
	memory_size = ((memory_size + 3) / 4) * 4;
	memory_block = (char*)malloc(sizeof(char) * memory_size);
	first_adress = memory_block;
	control_block_size = memory_size / 4;
	memory_status = (uc*)malloc(control_block_size * sizeof(uc));
	for (ll i = 0; i < control_block_size; i++)
		memory_status[i] = 0;
}

void initstop()
{
	free(memory_block);
	free(memory_status);
	control_block_size = 0;
}
