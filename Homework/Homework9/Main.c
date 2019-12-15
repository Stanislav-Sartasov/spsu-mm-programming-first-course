#include "Header.h"


// The main program starts here.
int main() {
	init();

	printf("Creating new dynamic memory of size 10.\n");
	char* p = (char*)myMalloc(sizeof(char) * 10);

	printf("Assigning the memories value from 0 to 9.");
	p[0] = '0';
	p[1] = '1';
	p[2] = '2';
	p[3] = '3';
	p[4] = '4';
	p[5] = '5';
	p[6] = '6';
	p[7] = '7';
	p[8] = '8';
	p[9] = '9';


	printf("Printing the memories: \n");
	for (int i = 0; i < 10; i++) {
		printf("Memory #%d: %c\n", i + 1, p[i]);
	}


	printf("Free the memory.\n");

	myFree(&p);
	if (!p) {
		printf("The memories is not free.\n");
	}
	else {
		printf("The memories is free.\n");
	}

	getchar(); 
	return 0;
}