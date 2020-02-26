#pragma once
#include <stdio.h>
#include <stdlib.h>

typedef struct mem_block
{
	size_t size_of_block;
	struct mem_block* next;
	struct mem_block* previous;
} mem_block;

static mem_block* stack = NULL;

void* newMalloc(size_t size);
void newFree(void* ptr);
void* newRealloc(void* ptr, size_t newSize);