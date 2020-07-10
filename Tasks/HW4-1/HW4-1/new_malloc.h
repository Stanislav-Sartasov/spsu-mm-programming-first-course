#pragma once
#include <stdio.h>
#include <stdlib.h>

void* new_malloc(size_t size);
void new_free(void* ptr);
void* new_realloc(void* ptr, size_t newSize);