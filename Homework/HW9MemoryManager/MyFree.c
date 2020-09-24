#include "MyMalloc.h"

static memDict blockHead = {0, 0};

void myFree(void *ptr)
{
    memDict *block = (memDict*)(((char*)ptr) - sizeof(size_t));
    block->next = blockHead.next;
    blockHead.next = block;
}