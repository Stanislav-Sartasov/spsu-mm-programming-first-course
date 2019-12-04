#pragma once

#include <stdio.h>
#include <stdlib.h>

extern char *bufer;
extern char *buferstat;
extern unsigned long long bufersize;

void init(unsigned long long x);
void initstop(void);
void* myMalloc(size_t size);
void myFree(void *ptr);
void* myRealloc(void* ptr, int size);

unsigned long long getBuferStat(unsigned long long i);
void pushBuferStat(unsigned long long i, short n);
