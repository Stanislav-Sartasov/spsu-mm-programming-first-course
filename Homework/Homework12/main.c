#include "gmp.h"
#include <stdio.h>
#include <string.h>

// Please change these constants to test the other numbers:
#define BASE 3
#define EXPONENT 5000


// The main program starts here.
int main()
{
	struct MP_INT* a = (struct MP_INT*)malloc(sizeof(struct MP_INT*));
	mpz_init_set_ui(a, BASE);
	mpz_pow_ui(a, a, EXPONENT); 

	char* s = mpz_get_str(0, 16, a);
	printf("Digits: %s\n", s);


	free(s);

	return 0;
} 

