#include "Header.h"
#include <math.h>


/*
	Checks if the triple passed is a Pythagorean Triple.
	@param a:		The first natural numbers.
	@param b:		The second natural numbers.
	@param c:		The third natural numbers.
	@return bool:	true if the triple is a Pythagorean Triple, false if it's not.
*/
bool isPythagoreanTriple(int a, int b, int c) {
	if (a <= 0 || b <= 0 || c <= 0) {
		return false;
	}
	else {
		if ((a * a + b * b == c * c) ||
			(b * b + c * c == a * a) ||
			(c * c + a * a == b * b)) {
			return true;
		}else {
			return false;
		}
	}
}


/*
	Calculates the greatest common divisior of two integers.
	@param a:		The first natural number.
	@param b:		The second natural number.
	@return int:	The greatest common divisor.
*/
int gcd(int a, int b){
	if (a == 0 || b == 0) {
		return a + b;
	}

	while (a != b) {
		if (a > b) {
			a -= b;
		}
		else {
			b -= a;
		}
	}
	return a;
}


/*
	Check if a Pythagorean Triple is primitive.
	@param a:		The first natural numbers.
	@param b:		The second natural numbers.
	@param c:		The third natural numbers.
	@return bool:	true if the Pythagorean Triple is primitive, false if it's not.
*/
bool isPrimitive(int a, int b, int c) {
	// Assumes that the triple passed is a Pythagorean triple.
	if (gcd(a, gcd(b, c)) == 1) {
		return true;
	}
	else {
		return false;
	}
}