#pragma once

#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <string.h>
#include <stdlib.h>

void init();

void uninit();

void* myMalloc(size_t size);

void myFree(void* ptr);

void* myRealloc(void* ptr, size_t size);
