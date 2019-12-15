#include "Header.h"


int main() {
	printf("Enter 3 numbers: ");
	
	float a, b, c;
	scanf_s("%f", &a);
	scanf_s("%f", &b);
	scanf_s("%f", &c);


	if (isTriangle(a, b, c)) {
		printf("It's a triangle!\n");
		printAngles(a, b, c);
	}
	else {
		printf("Not triangle!");
	}

	return 0;
}