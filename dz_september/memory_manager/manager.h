#pragma once

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

extern char *bufer;
extern char *buferstat;
extern unsigned long long bufersize, MEMSIZE;

void init(void);
void initstop(void);
void* myMalloc(size_t size);
void myFree(void *ptr);
void* myRealloc(void* ptr, int size);
