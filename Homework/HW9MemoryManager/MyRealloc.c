#include "MyMalloc.h"

//void *myMalloc(size_t size);

static size_t ptrLen(void *ptr)
{
    size_t i = 0;
    char *newPtr = (char*)ptr;
    if (!ptr)
        return NULL;
    else
    {
        while (newPtr[i])
            i++;
        return i * sizeof(ptr[i - 1]);    
    }
}

void *myRealloc(void *ptr, size_t size)
{
    void *newPtr = myMalloc(ptrLen(ptr));
    if (!newPtr)
        return NULL;
    memcpy(newPtr, ptr, ptrLen(ptr));
    return newPtr;
}