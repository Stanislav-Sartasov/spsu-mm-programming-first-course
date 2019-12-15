#include <stdio.h>
#include <math.h>


// The main program starts here.
int main() {
	printf("All Mersenne primes on the interval [1; 2 ^ 31 - 1]: \n");

	for (int i = 1; i <= 31; i++) {
		printf("%d\n", (int)pow(2, i) - 1);
	}
	
	return 0;
}