#include <stdio.h>
#include <stdlib.h>
#include <math.h>

const char name[] = "Pavel";
const int len_name = 5;
const char surname[] = "Alimov";
const int len_surname = 6;
const char patronymic[] = "Gennadevich";
const int len_patronymic = 11;

void to_binary(long long number, int *array, int down, int up)
{
    up++;
    for (int i = 1; i <= up - down; i++)
    {
        array[up - i] = number % 2;
        number /= 2;
    }
}

void print_array(int *array, int size)
{
    for(int i = 0; i < size; i++)
        printf("%d", array[i]);
	printf(".\n");
}

int find_lower_bound(int a)
{
    int ans = 0;
    int curr_pow_two = 1;
    while (curr_pow_two < a)
    {
        curr_pow_two *= 2;
        ans++;
    }
    return ans - 1;
}

int main()
{
	int res = 1;
	int size = 32;
	int *binary_presentation = (int*) malloc(sizeof(int) * size);
	res = len_name * len_patronymic * len_surname;

	printf("Result of multiply of name, surname and patronymic is %d.\n", res);

	// subtask 1

	long long twos_complement;

	twos_complement = pow(2, 32) - res;
	to_binary(twos_complement, binary_presentation, 0, size - 1);

	printf("First subtask: ");
    print_array(binary_presentation, size);


    // subtask 2


    binary_presentation[0] = 0;
    int bound =  find_lower_bound(res);
    to_binary(127 + bound, binary_presentation, 1, 8);
    int x = pow(2, bound);
    int m = (int) ((res  - x) / (x + .0) * pow(2, 23) +0.5);
    to_binary(m, binary_presentation, 9, size - 1);
    printf("Second subtask: ");
    print_array(binary_presentation, size);

    // subtask 3

    size *= 2;
    binary_presentation = (int*) realloc(binary_presentation, sizeof(int) * size);
    binary_presentation[0] = 1;
    bound =  find_lower_bound(res);
    to_binary(1023 + bound, binary_presentation, 1, 11);
    x = pow(2, bound);
    long long m2 = (long long) ((res  - x) / (x + .0) * pow(2, 52) +0.5);
    to_binary(m2, binary_presentation, 12, size - 1);
    printf("Third subtask: ");
    print_array(binary_presentation, size);

    free(binary_presentation);
	return 0;
}
