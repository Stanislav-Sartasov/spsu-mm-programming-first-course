#pragma once
#include <stdlib.h>
#include <stdio.h>

void* myMalloc(size_t size);

void myFree(void* ptr);

void* myRealloc(void* ptr, size_t size);