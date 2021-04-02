#pragma once
#include <stdio.h>
#include <stdlib.h>

void* memory_block;

void mem_block_change(unsigned int* mem_control_block, int size, char is_available);

void init();

void* myMalloc(size_t size);

void* myRealloc(void* ptr, size_t new_size);

void myFree(void* ptr);

