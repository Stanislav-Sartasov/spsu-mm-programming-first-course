#pragma once

void *managed_memory_start;
void *last_valid_address;

typedef struct
{
	int is_available;
	int size;
} mem_control_block;

void* myMalloc(size_t size);

void myFree(void* ptr);

void* myRealloc(void* ptr, size_t size);

void init();
