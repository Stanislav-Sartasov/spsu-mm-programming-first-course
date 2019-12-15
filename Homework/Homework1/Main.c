#include "Header.h"
#include <string.h>


int main() {

	char firstName[] = "Ilias";
	char surName[] = "Mzoughi";
	char patronymic[] = "Mzoughi";

	int product = strlen(firstName) * strlen(surName) * strlen(patronymic);
	printf("Product found: %d\n", product);

	char result[BASE];
	decimalToBinary(product, result, BASE);
	twoCompliment(result, BASE);

	printf("\tThe negative 32-bit integer: ");
	printArray(result, BASE);
	printf("\n");

	singleIEEE754(positive, product, result);
	printf("\tThe positive floating-point number: ");
	printArray(result, SINGLE_PRECISION_BASE);
	printf("\n");

	product = strlen(firstName) * strlen(surName) * strlen(patronymic);
	decimalToBinary(product, result, BASE);
	doubleIEEE754(negative, product, result);
	printf("\tThe negative floating-point number: ");
	printArray(result, DOUBLE_PRECISION_BASE);	
	printf("\n");

	return 0;
}