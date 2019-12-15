#include <stdio.h>
#include <math.h>


// The main program starts here.
int main() {
	int num;
	
	printf("Enter a number that is not a square of an integer: ");

	while (1) {
		scanf_s("%d", &num);
		if (num <= 0 || floor(sqrt(num)) == sqrt(num)) {
			printf("Incorrect input. Try again: ");
		}
		else {
			break;
		}
	}

	
	int sequence = 0;
	double xi = sqrt(num);
	int ai = floor(xi);

	int a0 = floor(xi);
	printf("[%d; ", a0);

	while(1){
		xi = 1 / (xi - ai);
		ai = floor(xi);
		printf("%d, ", ai);
		sequence++;

		if (ai == 2 * a0) {
			printf("...]\nWith the sequence %d", sequence);
			break;
		}
	}


	return 0;
}