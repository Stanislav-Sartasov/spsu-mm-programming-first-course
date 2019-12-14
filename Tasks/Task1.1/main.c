#include "stdio.h"
#include "string.h"
#include "stdlib.h"
#include "math.h"

void str_rev(char *str, char **new_str)
{
    int n = strlen(str);

    int j = 0;
    for (int i = n - 1; i >= 0; i--)
    {
        (*new_str)[j] = str[i];
        j++;
    }
}

char* decimal_to_32binary(long long int number)
{
    char* binary = calloc(32, sizeof(char));

    int i = 0;
    while (number > 0)
    {
        int rem = number % 2;
        binary[i] = rem + '0';
        number = number / 2;
        i++;
    }

    return binary;
}

char* decimal_to_binary(int number)
{
    char binary[32];

    for (int i = 0; i < 31; i++)
    {
        binary[i] = '\0';
    }


    int i = 0;
    while (number > 0)
    {
        int rem = number % 2;

        binary[i] = rem + '0';
        number = number / 2;
        i++;
    }

    char *rev_binary = (char *) calloc(strlen(binary) + 1, sizeof(char));
    str_rev(binary, &rev_binary);

    return rev_binary;
}

void convert_number(int n, int mode)
{
    int res_len;
    int exp_bias;
    int mantissa_bias;
    char sign;

    // 0 - single precision; 1 - double precision
    if (mode == 0)
    {
        res_len = 32;
        exp_bias = 127;
        mantissa_bias = 9;
        sign = '0';
    }
    else if (mode == 1)
    {
        res_len = 64;
        exp_bias = 1023;
        mantissa_bias = 12;
        sign = '1';
    }

    char binary[res_len + 1];

    for (int i = 0; i < res_len; i++) {
        binary[i] = '0';
    }
    binary[res_len] = '\0';

    binary[0] = sign;

    char* sub_binary = decimal_to_binary(n);

    int exponent = exp_bias + strlen(sub_binary) - 1;

    char* binary_exp = decimal_to_binary(exponent);

    for (int i = 1; i < strlen(binary_exp) + 1; i++)
    {
        binary[i] = binary_exp[i - 1];
    }

    for (int i = mantissa_bias; i < mantissa_bias + strlen(sub_binary) - 1; i++)
    {
        binary[i] = sub_binary[i - mantissa_bias + 1];
    }

    printf("\n%s\n", binary);

    free(sub_binary);
    free(binary_exp);
}

int main(void)
{
    setbuf(stdout, NULL);
    char* first_name = "ZEMIN";
    char* last_name = "LI";

    int a = strlen(first_name);
    int b = strlen(last_name);

    int c = a * b;

    printf("Found product: %d\n\n", c);

    // a
    long long int target_num = pow(2, 32) - c;

    char* binary_str = decimal_to_32binary(target_num);

    printf("Negative 32-bit:\n");
    for (int i = 31; i >= 0; i--)
    {
        printf("%c", binary_str[i]);
    }
    printf("\n\n");

    free(binary_str);

    // b
    printf("IEEE 754 (single precision):");
    convert_number(c, 0);

    printf("\n");

    // c
    printf("IEEE 754 (double precision):");
    convert_number(c, 1);

    return 0;
}
