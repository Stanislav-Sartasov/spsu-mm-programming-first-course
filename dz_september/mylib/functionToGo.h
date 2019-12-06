#pragma once
#include <stdio.h>

extern char stringOfAgainInput[80];

double saveIn(void);
long long saveInInt(void);
void changeAgainInputText(const char *str);
long long gcd(long long, long long);
double reminder(double, double); // only for numbers >0, divident - wholep(divident, divisor)
long long wholep(double, double); // only for numbers >0, whole part divident / divisor
