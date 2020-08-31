#include <stdio.h>
#include <stdlib.h>
#include "alloc.h"

int main()
{
	// initialization
	int* ptr_arr[10]; 
	init(); 
	printf("Initial state:\n");
	print_blocks();

	printf("Simple malloc test:\n");
	for (int i = 0; i < 10; i++) // 1 malloc test
	{
		ptr_arr[i] = (int*)myMalloc(64 + i * 4);
		for (int j = 0; j < (64 + i * 4); j++)
			ptr_arr[i][j] = j;
	}
	print_blocks();

	printf("Free test:\n");
	for (int i = 0; i < 5; i++) // 1 free test
		myFree(ptr_arr[i * 2]);
	print_blocks();

	printf("Block merge test:\n");
	myFree(ptr_arr[5]); // 2 free test
	print_blocks();

	printf("Free space search:\n");
	ptr_arr[5] = (int*)myMalloc(200); // 2 malloc test
	print_blocks();

	printf("Clear all:\n");
	myFree(ptr_arr[5]); // 3 free test
	for (int i = 0; i < 5; i++)
		myFree(ptr_arr[i * 2 + 1]);
	print_blocks();

	// realloc tests

	printf("Forward reallocation: before\n");
	ptr_arr[0] = (int*)myMalloc(100);
	ptr_arr[1] = (int*)myMalloc(96);
	ptr_arr[2] = (int*)myMalloc(64);
	ptr_arr[3] = (int*)myMalloc(72);

	myFree(ptr_arr[2]); // 1 test preparation
	print_blocks();

	printf("Forward reallocation: after\n");
	myRealloc(ptr_arr[1], 128); // 1 realloc test
	print_blocks();

	printf("Backward reallocation: before\n");
	myFree(ptr_arr[0]); // 2 test preparation
	print_blocks();

	printf("Backward reallocation: after\n");
	myRealloc(ptr_arr[1], 200); // 2 realloc test
	print_blocks();

	printf("Full reallocation test:\n");
	myRealloc(ptr_arr[0], 500); // 3 realloc test
	print_blocks();

	return 0;
}