#define _CRT_SECURE_NO_WARNINGS
#include "Header.h"
#include <stdio.h>


int main() {
	int a, b, c;
	printf("Please enter 3 sides of the Pythagorean triangle: ");

	scanf("%d", &a);
	scanf("%d", &b);
	scanf("%d", &c);
	

	if (isPythagoreanTriple(a, b, c)) {
		printf("\tIt's a Pythagorean triple ");
		if (isPrimitive(a, b, c)) {
			printf("and it's primitive!");
		}
		else {
			printf("but it's not primitive!");
		}
	}
	else {
		printf("\tIt's not a Pythagorean triple.");
	}

	return 0;
}