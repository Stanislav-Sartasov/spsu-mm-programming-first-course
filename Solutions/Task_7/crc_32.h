#pragma once

unsigned crc_calc(unsigned char* in, unsigned len);
void crc_set(unsigned poly, unsigned init, char revert, unsigned xor_out);
char crc_set_flag;