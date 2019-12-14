#ifndef MANAGER_H_INCLUDED
#define MANAGER_H_INCLUDED

#include <stdio.h>
#include <stdlib.h>
#include <string.h>


void init();
void initstop();
void* my_malloc(size_t size);
void my_free(void *ptr);
void* my_realloc(void* ptr, size_t size);

#endif // MANAGER_H_INCLUDED
