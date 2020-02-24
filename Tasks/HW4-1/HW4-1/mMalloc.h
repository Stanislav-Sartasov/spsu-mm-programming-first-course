
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

void* myMalloc(size_t size);
void myFree(void* ptr);
void* myRealloc(void* ptr, size_t newSize);