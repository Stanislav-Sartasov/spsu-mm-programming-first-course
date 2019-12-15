#pragma once

#ifdef _MSC_VER
#define _CRT_SECURE_NO_WARNINGS
#endif

#ifndef __cplusplus
typedef unsigned char sign;
static const sign positive = 0;
static const sign negative = 1;
#endif

#define BASE 32
#define SINGLE_NORMALISED_MANTISA_LENGTH 23
#define SINGLE_PRECISION_BIASED_EXPONENT 127
#define SINGLE_PRECISION_BIASED_EXPONENT_LENGTH 8
#define SINGLE_PRECISION_BASE 32

#define DOUBLE_NORMALISED_MANTISA_LENGTH 52
#define DOUBLE_PRECISION_BIASED_EXPONENT 1023
#define DOUBLE_PRECISION_BIASED_EXPONENT_LENGTH 11
#define DOUBLE_PRECISION_BASE 64


#include <stdio.h>
#include <math.h>


/*
	Convert a decimal number into binary with the BASE defined above.
	@param m_num - the number that needs to be converted.
	@param m_result - the converted number will be stored here.
	@param m_base - the number of binary digits.
	@return	char* - the array contains the INVERTED converted number.
*/
void decimalToBinary(int m_num, char* m_result, int m_base);


/*
	Inverts all the bits in the binary representation.
	@param m_num The binary number needs to be converted.
*/
void oneCompliment(char* m_num);


/*
	Converts to signed number representation
	@param m_num The binary number needs to be converted.
*/
void twoCompliment(char* m_num);


/*
	Reverses the array from the beginning and ending positons passed.
	@param arr The array that needs to be reversed.
	@param start The beginning position.
	@param end The ending position.
*/
void reverseArray(char arr[], int start, int end);


/*
	Prints out a number of elements in a char array.
	@param m_input The array that needs to be printed.
	@param m_length The number of elements that needs to be printed.
*/
void printArray(char* m_input, int m_length);


/*
	Converts a number to single-precision IEEE754 standard.
	@param m_sign The sign of the number, could be negative or positive.
	@param m_num The number that needs to be converted.
	@param m_result The converted number will be stored in it.
*/
void singleIEEE754(sign m_sign, float m_num, char* m_result);


/*
	Converts a number to double-precision IEEE754 standard.
	@param m_sign The sign of the number, could be negative or positive.
	@param m_num The number that needs to be converted.
	@param m_result The converted number will be stored in it.
*/
void doubleIEEE754(sign m_sign, float m_num, char* m_result);


/*
	Converts a float point (which is smaller than 1) to binary presentation.
	@param m_num The number that needs to be converted.
	@param m_result The converted number will be stored in it.
	@param m_length The max length of the converted number.
*/
void floatPointToBinary(float m_num, char* m_result, int m_length);