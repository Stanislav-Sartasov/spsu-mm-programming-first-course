#include "stdio.h"
#include "string.h"
#include "stdlib.h"
#include "math.h"

void str_rev(char *str, char **new_str)
{
    int n = strlen(str);

    int j = 0;
    for (int i = n-1; i >= 0; i--)
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

    char *rev_binary = (char *) calloc(strlen(binary)+1, sizeof(char));
    str_rev(binary, &rev_binary);

    return rev_binary;
}

void single_precision(int number)
{
    char binary[33];

    for (int i = 0; i < 32; i++)
    {
        binary[i] = '0';
    }
    binary[32] = '\0';

    char* sub_binary = decimal_to_binary(number);

    int exponent = 127 + strlen(sub_binary) - 1;

    char* binary_exp = decimal_to_binary(exponent);

    for (int i = 1; i < strlen(binary_exp) + 1; i++)
    {
        binary[i] = binary_exp[i-1];
    }

    for (int i = 9; i < 9 + strlen(sub_binary) - 1; i++)
    {
        binary[i] = sub_binary[i - 8];
    }

    printf("\n%s\n", binary);

    free(sub_binary);
    free(binary_exp);
}

void double_precision(int n)
{
    char binary[65];

    for (int i = 0; i < 64; i++) {
        binary[i] = '0';
    }
    binary[64] = '\0';

    binary[0] = '1';

    char* sub_binary = decimal_to_binary(n);

    int exponent = 1023 + strlen(sub_binary) - 1;

    char* binary_exp = decimal_to_binary(exponent);

    for (int i = 1; i < strlen(binary_exp) + 1; i++)
    {
        binary[i] = binary_exp[i-1];
    }

    for (int i = 12; i < 12 + strlen(sub_binary) - 1; i++)
    {
        binary[i] = sub_binary[i - 11];
    }

    printf("\n%s\n", binary);

    free(sub_binary);
    free(binary_exp);
}

int main(void)
{
    char* first_name = "ZEMIN";
    char* last_name = "LI";

    int a = strlen(first_name);
    int b = strlen(last_name);

    int c = a * b;

    printf("%d\n", c);


    // a
    long long int target_num = pow(2, 32) - c;

    char* binary_str = decimal_to_32binary(target_num);

    for (int i = 31; i >= 0; i--)
    {
        printf("%c", binary_str[i]);
    }
    printf("\n");

    free(binary_str);

    // b

    single_precision(c);

    // c
    double_precision(c);

    return 0;
}

