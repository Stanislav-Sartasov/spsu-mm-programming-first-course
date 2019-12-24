#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <string.h>
#include "memory_manager.h"


int main() 
{
	init();
	char *c1 = myMalloc(5 * sizeof(char));
	char *c2 = myMalloc(10 * sizeof(char));
	char *c3 = myMalloc(5 * sizeof(char));
	char *c4 = myMalloc(10 * sizeof(char));
	char *c5 = myMalloc(5 * sizeof(char));
	strcpy(c1, "1111");
	strcpy(c2, "222222222");
	strcpy(c3, "3333");
	strcpy(c4, "444444444");
	strcpy(c5, "5555");
	myFree(c2);
	myFree(c4);
	char *c6 = myMalloc(15 * sizeof(char)); //cant because no space availabe (
	myFree(c3);
	c6 = myMalloc(15 * sizeof(char)); // can because free blocks merging together when c3 released

	return 0;
}