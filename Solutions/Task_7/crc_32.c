#include <stdio.h>
#include <stdlib.h>

unsigned crc_table[256];
unsigned crc_init;
char crc_revert;
unsigned crc_xor_out;
char crc_set_flag = 0;

void crc_set(unsigned poly, unsigned init, char revert, unsigned xor_out)
{
	crc_init = init;
	crc_revert = revert;
	crc_xor_out = xor_out;
	crc_set_flag = 1;
	unsigned crc;
	if (revert)
	{
		unsigned r_poly = 0;
		for (int i = 1;; i <<= 1)
		{
			r_poly = poly & i ? (r_poly << 1) | 1 : r_poly << 1;
			if (i == 0x80000000)
				break;
		}
		for (int i = 0; i < 256; i++)
		{
			crc = i;
			for (int j = 0; j < 8; j++)
				crc = crc & 1 ? (crc >> 1) ^ r_poly : crc >> 1;
			crc_table[i] = crc;
		}
	}
	else
	{
		for (int i = 0; i < 256; i++)
		{
			crc = i << 24;
			for (int j = 0; j < 8; j++)
				crc = crc & (1 << 31) ? (crc << 1) ^ poly : crc << 1;
			crc_table[i] = crc;
		}
	}
}

unsigned crc_calc(unsigned char* input, unsigned len)
{
	unsigned crc = crc_init;
	for (unsigned i = 0; i < len; i++)
		if (crc_revert)
			crc = crc_table[(crc ^ input[i]) & 255] ^ (crc >> 8);
		else
			crc = crc_table[(crc >> 24) ^ input[i]] ^ (crc << 8);
	return crc ^ crc_xor_out;
}