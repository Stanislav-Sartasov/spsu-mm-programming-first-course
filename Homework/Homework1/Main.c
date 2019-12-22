#include <stdio.h>
#include <stdint.h>
#include <string.h>

void printBinary32(uint32_t num) 
{
    for(int i = 31; i >= 0; --i)
        printf("%d", (num & (1u << i)) >= 1);
    printf("\n");
}

void printBinary64(uint64_t num) 
{
    for(int i = 63; i >= 0; --i)
        printf("%d", (num & (1ull << i)) >= 1);
    printf("\n");
}

uint32_t twoComplement(uint32_t num) 
{
    return (~num) + 1;
}

int main() 
{

    char firstName[]  = "Ilias";
    char middleName[] = "Mzoughi";
    char lastName[]   = "Mzoughi";

    uint32_t product = strlen(firstName) * strlen(lastName) * strlen(middleName);

    uint32_t a = product;
    float b = product;
    double c = product;

    printf("The negative 32-bit integer: ");
    printBinary32(twoComplement(a));

    printf("The positive floating-point number: ");
    printBinary32((*((uint32_t*)&b)));

    printf("The negative floating-point number: ");
    printBinary64((*((uint64_t*)&c)));
	
}
