#include "Header.h"


bool isTriangle(float a, float b, float c) {
	if (a <= 0 || b <= 0 || c <= 0) {
		return false;
	}else{
		if ((a + b > c) &&
			(b + c > a) &&
			(c + a > b)) {
			return true;
		}
		else {
			return false;
		}
	}
}


void printAngles(float a, float b, float c) {
	float cos[3];
	cos[0] = (b * b + c * c - a * a) / (2.0 * b * c);
	cos[1] = (c * c + a * a - b * b) / (2.0 * c * a);
	cos[2] = (a * a + b * b - c * c) / (2.0 * a * b);

	double radians[3];
	float degrees[3];
	float minutes[3];
	float seconds[3];

	for (int i = 0; i < 3; i++) {
		radians[i] = acos(cos[i]);
		degrees[i] = radians[i] * 180 / M_PI;
		minutes[i] = (degrees[i] - (int)degrees[i]) * 60;
		seconds[i] = (minutes[i] - (int)minutes[i]) * 60;
	}

	
	for (int i = 0; i < 3; i++) {
		printf("\t%d degree, %d minute, %d second\n", (int)degrees[i], (int)minutes[i], (int)seconds[i]);
	}
}