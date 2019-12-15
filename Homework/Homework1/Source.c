#include "Header.h"


void decimalToBinary(int m_num, char* m_result, int m_base){
	int num = m_num;


	// Convert into binary.
	for (int i = 0; i < m_base; i++)
	{
		if (num > 0) {
			m_result[i] = num % 2;
			num = num / 2;
		}
		else {
			m_result[i] = 0;
		}
	}


	// Reverse the array.
	reverseArray(m_result, 0, m_base - 1);
}


void reverseArray(char arr[], int start, int end)
{
	while (start < end)
	{
		char temp = arr[start];
		arr[start] = arr[end];
		arr[end] = temp;
		start++;
		end--;
	}
}


void printArray(char* m_input, int m_length) {
	for (int i = 0; i < m_length; i++) {
		printf("%d", m_input[i]);
	}
}


void oneCompliment(char* m_num) {
	for (int i = 0; i < BASE; i++) {
		m_num[i] = (m_num[i] + 1) % 2;
	}
}


// todo
void plusOne(char* m_num) {
	if (*m_num == 0) {
		*m_num = 1;
	}
	else {
		*m_num = 0;
		plusOne(m_num - 1);
	}
}


void twoCompliment(char* m_num) {
	oneCompliment(m_num);

	plusOne(&m_num[BASE - 1]);
}


void singleIEEE754(sign m_sign, float m_num, char* m_result) {
	char temp[64];
	char biInteger[BASE];
	char biFloat[SINGLE_NORMALISED_MANTISA_LENGTH];
	char biasedExponent[SINGLE_PRECISION_BIASED_EXPONENT_LENGTH];

	decimalToBinary((int)m_num, biInteger, SINGLE_PRECISION_BASE);
	for (int i = 0; i < log2((int)m_num); i++) {
		biInteger[i] = biInteger[i + BASE - (int)log2((int)m_num) - 1];
	}


	// Normalised mantisa
	char normalisedMantisa[SINGLE_NORMALISED_MANTISA_LENGTH];
	floatPointToBinary(m_num - floor(m_num), biFloat, SINGLE_NORMALISED_MANTISA_LENGTH);
	for (int i = 0; i < SINGLE_NORMALISED_MANTISA_LENGTH; i++) {
		normalisedMantisa[i] = biInteger[i+1];
	}
	for (int i = log2((int)m_num); i < SINGLE_NORMALISED_MANTISA_LENGTH; i++) {
		normalisedMantisa[i] = biFloat[i - (int)log2((int)m_num)];
	}


	// Biased exponent
	int i_biasedExponent = SINGLE_PRECISION_BIASED_EXPONENT + log2((int)m_num);
	decimalToBinary(i_biasedExponent, temp, SINGLE_PRECISION_BASE);
	for (int i = 0; i < floor(log2((int)i_biasedExponent)); i++) {
		temp[i] = temp[(i + BASE - (int)(log2(floor(i_biasedExponent))) - 1)];
	}
	for (int i = 0; i < SINGLE_PRECISION_BIASED_EXPONENT_LENGTH; i++) {
		biasedExponent[i] = temp[i];
	}


	// Result
	m_result[0] = m_sign;
	for (int i = 1; i < SINGLE_PRECISION_BIASED_EXPONENT_LENGTH + 1; i++) {
		m_result[i] = biasedExponent[i - 1];
	}
	for (int i = SINGLE_PRECISION_BIASED_EXPONENT_LENGTH + 1; i < DOUBLE_PRECISION_BASE; i++) {
		m_result[i] = normalisedMantisa[i - SINGLE_PRECISION_BIASED_EXPONENT_LENGTH - 1];
		if (m_result[i] != 1 && m_result[i] != 0) {
			m_result[i] = 0;
		}
	}
}


void floatPointToBinary(float m_num, char* m_result, int m_length) {
	float num = m_num;


	for (int i = 0; i < m_length; i++) {
		if (num != 1.0 && num != 0.0) {
			num *= 2;
			m_result[i] = (char)num;
			num -= (char)num;
		}
		else {
			m_result[i] = 0;
		}
	}
}


void doubleIEEE754(sign m_sign, float m_num, char* m_result) {
	char biInteger[64];
	char biFloat[DOUBLE_NORMALISED_MANTISA_LENGTH];
	char biasedExponent[DOUBLE_PRECISION_BIASED_EXPONENT_LENGTH];


	// Normalised mantisa
	decimalToBinary((int)m_num, biInteger, DOUBLE_NORMALISED_MANTISA_LENGTH);
	for (int i = 0; i < log2((int)m_num); i++) {
		biInteger[i] = biInteger[i + DOUBLE_NORMALISED_MANTISA_LENGTH - (int)log2((int)m_num) - 1];
	}

	char normalisedMantisa[DOUBLE_NORMALISED_MANTISA_LENGTH];
	floatPointToBinary(m_num - (int)m_num, biFloat, DOUBLE_NORMALISED_MANTISA_LENGTH);

	for (int i = 0; i < DOUBLE_NORMALISED_MANTISA_LENGTH - 1; i++) {
		normalisedMantisa[i] = biInteger[i + 1];
	}
	normalisedMantisa[DOUBLE_NORMALISED_MANTISA_LENGTH - 1] = 0;
	for (int j = log2((int)m_num); j < DOUBLE_NORMALISED_MANTISA_LENGTH; j++) {
		normalisedMantisa[j] = biFloat[j - (int)log2((int)m_num)];
	}


	// Biased exponent
	int i_biasedExponent = DOUBLE_PRECISION_BIASED_EXPONENT + log2((int)m_num);
	char temp[DOUBLE_NORMALISED_MANTISA_LENGTH];
	decimalToBinary(i_biasedExponent, temp, DOUBLE_NORMALISED_MANTISA_LENGTH);
	for (int i = 0; i < log2((int)i_biasedExponent); i++) {
		temp[i] = temp[i + BASE - (int)log2((int)i_biasedExponent) - 1];
	}
	for (int i = 0; i < DOUBLE_PRECISION_BIASED_EXPONENT_LENGTH; i++) {
		biasedExponent[i] = temp[i];
	}


	// Result
	m_result[0] = m_sign;
	for (int i = 1; i < DOUBLE_PRECISION_BIASED_EXPONENT_LENGTH + 1; i++) {
		m_result[i] = biasedExponent[i - 1];
	}
	for (int i = DOUBLE_PRECISION_BIASED_EXPONENT_LENGTH + 1; i < DOUBLE_PRECISION_BASE; i++) {
		m_result[i] = normalisedMantisa[i - DOUBLE_PRECISION_BIASED_EXPONENT_LENGTH - 1];
	}
}