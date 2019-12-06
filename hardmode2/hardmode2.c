#include <stdio.h>
#include <math.h>
#include <string.h>


// Сначала находим N(1,0,k), N(0,1,k) полным перебором для k<8. Затем считаем биномиальные коэф-ы для 
// С из 1984 по k, 2<k<8. Тогда N(1,0,n)=сумма по всем 2<k<8 (C из n по k) * (N(1, 0, k) - N(1, 0, k - 1)),
// очевидно, что в <3 цветов граф не красится. Аналогично N(0, 1, n). Введем две дополнительные функции p1, p2.
// p1 - кол-во способов покрасить элемент А в n цветов, с уже зафиксированными двумя цветами. p2 - покрасить B.
// Тогда ответ в задаче - (C из 100 по 25) * N(1, 0, 1984) * (p1)^24 * (p2)^75.
// Также можно доказать соотношение между N(1, 0, n) и p1. N(1, 0, n) = n * (n - 1) * p1 <=>
// <=> n * (n - 1) * (p1)^25 * (p2)^75

const base = 1e9;
const mod = 1e8;

int factorial(int n)
{
	if (n == 0 || n == 1) return 1;
	return n * factorial(n - 1);
}

void brute_force(int n, int p_a[], int p_b[]) // Раскрасить элемент А в <=n цветов(n <= 7)
{
	int answer_a = 0;
	int answer_b = 0;
	for (int a = 0; a < n; a++)
	{
		for (int b = 0; b < n; b++)
		{
			for (int c = 0; c < n; c++)
			{
				for (int d = 0; d < n; d++)
				{
					for (int e = 0; e < n; e++)
					{
						for (int f = 0; f < n; f++)
						{
							for (int g = 0; g < n; g++)
							{
								if (a != b && a != c && a != d && b != e && b != f && c != d && c != g
									&& d != e && e != f && f != g) answer_a += 1;
								if (a != b && a != c && a != d && b != f && c != d && c != g
									&& d != e && e != f && f != g) answer_b += 1;
							}

						}
					}
				}
			}
		}
	}
	p_a[n - 1] = answer_a;
	p_b[n - 1] = answer_b;
}

void sum(long long* first, long long* second, int size)
{
	for (int i = size - 1; i >= 0; i--)
	{
		first[i] += second[i];
	}
	for (int i = size - 1; i >= 0; i--)
	{
		if (first[i] >= base)
		{
			first[i] -= base;
			first[i - 1] += 1;
		}
	}
}


void multiplication(long long* q, int n, int size)
{

	int rest = 0;
	int p = 0;
	int j = size - 1;
	while (j >= 0)
	{
		q[j] = q[j] * n + p;

		rest = q[j] % base;
		p = (q[j] - rest) / base;
		q[j] = rest;

		j -= 1;
	}

}

void division(long long* q, int n, int size)
{
	for (int i = 0; i < size; i++)
	{
		if (i != size - 1)
		{
			q[i + 1] += ((q[i] % n) * base);

		}
		q[i] /= n;
	}
}



void bin_coef(long long q[][3], int n, int k)
{
	int fact = factorial(k);
	long long array[3] = { 0, 0, 1 };
	for (int i = n; i > n - k; i--)
	{
		multiplication(array, i, 3);
	}
	division(array, fact, 3);
	for (int u = 0; u < 3; u++)
	{
		q[k - 3][u] = array[u];
	}
}

void part(long long sample[][3], long long* arr, int* amounts, int number)
{

	long long copy[5];
	for (int j = 0; j < 3; j++)
	{
		copy[j + 2] = sample[number - 3][j];
	}
	copy[0] = 0;
	copy[1] = 0;
	multiplication(copy, amounts[number - 1] - amounts[number - 2], 5);
	sum(arr, copy, 5);

}

void quick(long long* arr, int i)
{
	arr[i] = (arr[i - 1] * arr[i - 1]) % mod;
}


int mod_exp(long long x) // x^25 % mod
{
	long long degrees[5];
	degrees[0] = x % mod;
	for (int i = 1; i < 5; i++)
	{
		quick(degrees, i);
	}
	long long answer = (((degrees[0] * degrees[3]) % mod) * degrees[4]) % mod;
	return answer;
}

long long binom(int n, int k) // Упростим числитель и знаменатель, сведем к делению "длинного" на "короткое".
{
	int numerator[25];
	int denominator[25];
	char flag_n[25];
	char flag_d[25];
	memset(flag_n, -1, sizeof(char) * 25);
	memset(flag_d, -1, sizeof(char) * 25);
	for (int i = 100; i > 75; i--)
	{
		numerator[100 - i] = i;
		denominator[100 - i] = 100 - i + 1;
	}
	for (int i = 0; i < 25; i++)
	{
		for (int j = 24; j > 0; j--)
		{
			if (denominator[j] != 1 && numerator[i] % denominator[j] == 0 && flag_n[i] == -1 && flag_d[j] == -1)
			{
				numerator[i] /= denominator[j];
				denominator[j] = 1;
				flag_n[i] = 1;
				flag_d[j] = 1;

				break;
			}
		}
	}

	int d = 1;
	long long total[5] = { 0, 0, 0, 0, 1 };
	for (int i = 0; i < 25; i++)
	{
		multiplication(total, numerator[i], 5);
		d *= denominator[i];
	}
	division(total, d, 5);

	return (total[4] % mod);
}

int main()
{
	int amount_a[7] = { 0, 0, 0, 0, 0, 0, 0 };
	int amount_b[7] = { 0, 0, 0, 0, 0, 0, 0 };
	for (int i = 1; i < 8; i++)
	{
		brute_force(i, amount_a, amount_b);
	}

	long long coef[5][3];
	for (int i = 0; i < 5; i++)
	{
		memset(coef[i], 0, sizeof(long long) * 3);
	}
	for (int z = 3; z <= 7; z++) // "С из 1984 по k", k от 3 до 7.
	{
		bin_coef(coef, 1984, z);
	}

	long long list_a[5];
	long long list_b[5];
	memset(list_a, 0, sizeof(long long) * 5);
	memset(list_b, 0, sizeof(long long) * 5);
	for (int i = 3; i < 8; i++) // N(1, 0, n), N(0, 1, n)
	{
		part(coef, list_a, amount_a, i);
		part(coef, list_b, amount_b, i);
	}

	division(list_a, 3934272, 5); // (N(1, 0, n) / (1984 * 1984)) = p1
	division(list_b, 3934272, 5); // p2

	long long a = list_a[4] % mod;
	long long b = list_b[4] % mod;
	long long res;
	a = mod_exp(a);
	b = mod_exp(a);
	b = (((b * b) % mod) * b) % mod; // b^75 % mod
	res = (a * b) % mod;
	res = (res * binom(100, 25)) % mod;
	res = (((1984 * res) % mod) * 1983) % mod;
	printf("%lli", res);


	return 0;
}
