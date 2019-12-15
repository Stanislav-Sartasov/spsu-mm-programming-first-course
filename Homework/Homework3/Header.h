#pragma once
#include <stdio.h>
#include <math.h>

#define M_PI acos(-1.0)

#ifndef BOOLEAN
#define BOOLEAN
typedef unsigned char bool;
static const bool false = 0;
static const bool true = 1;
#endif


/*
	Checks if a triple can form a triangle.
	@param a:		The first natural numbers.
	@param b:		The second natural numbers.
	@param c:		The third natural numbers.
	@return bool:	true if the triple can form a triangle, false if it's not.
*/
bool isTriangle(float a, float b, float c);


/*
	Prints out 3 angles of the triangle constructed from the triple passed.
	@param a:		The first natural numbers.
	@param b:		The second natural numbers.
	@param c:		The third natural numbers.
*/
void printAngles(float a, float b, float c);