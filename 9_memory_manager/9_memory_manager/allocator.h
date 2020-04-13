#pragma once
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

void* myMalloc(size_t size);
void myFree(void* ptr);
void* myRealloc(void* ptr, size_t size);
void init();