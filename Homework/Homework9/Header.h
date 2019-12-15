#pragma once
#include <stdio.h>

#define MAX_SIZE 500

unsigned char* malloc_data;
size_t mallocUsed;

void init();


void* myMalloc(size_t size);


void myFree(void* ptr);


void* myRealloc(void* ptr, size_t size);