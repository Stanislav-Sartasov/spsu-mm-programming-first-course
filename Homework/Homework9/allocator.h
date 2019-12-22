#ifndef TASKS_ALLOCATOR_H
#define TASKS_ALLOCATOR_H

#undef min
#undef max

#include <stdlib.h>
#include <stdint.h>

void* myMalloc(size_t size);
void  myFree(void* ptr);
void* myRealloc(void* ptr, size_t size);

// size = size of initial base memory to allocate
void  init(size_t size);

#endif //TASKS_ALLOCATOR_H
