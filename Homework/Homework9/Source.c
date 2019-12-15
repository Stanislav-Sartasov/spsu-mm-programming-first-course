#include "Header.h"	


void init() {
	malloc_data = (unsigned char*)malloc(MAX_SIZE * sizeof(unsigned char));
	mallocUsed = 0;
}


void* myMalloc(size_t size) {
	void* p = &malloc_data[mallocUsed];
	mallocUsed += size;
	return p;
}


void myFree(char* ptr) {
	ptr = NULL;
}


void* myRealloc(void* ptr, size_t size) {
	ptr = &malloc_data[mallocUsed];
	mallocUsed = size;
}