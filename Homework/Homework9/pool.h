#ifndef TASKS_POOL_H
#define TASKS_POOL_H

#undef min
#undef max

#define MIN_POOL_ID   (6)
#define MAX_POOL_ID   (16)

// initialized pool with (2 ^ p * c) bytes of memory
void  poolInit(int p, int c, void* addr);

// return a continues block of memory of size (2 ^ p * c)
void* poolAllocate(int p, int c);

// add a continues block of memory of size (2 ^ p * c) to pool
void  poolDeallocate(int p, int c, void* addr);

#endif //TASKS_POOL_H
