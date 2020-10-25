#pragma once
#include <stdio.h>
#include <stdlib.h>

void* memory_block;

#pragma pack(push, 1)

typedef struct mem_control_block
{
	int size;
	char is_available;
}mem_control_block;

#pragma pack(pop)

void init();

void* myMalloc(size_t size);

void* myRealloc(void* ptr, size_t new_size);

void myFree(void* ptr);