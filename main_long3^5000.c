#include <stdio.h>

#define R 500   // ����� �������� ���� ��� �������� �����
#define pow 5000 // �������


int main()
{
	unsigned int a[R] = { 0 };
	unsigned int abuf[R];
	int i;
	a[0] = 1;
	for (i = 0; i < pow; i++)
	{
		int j;
		for (j = 0; j < R; j++) // ���������� ����� � �����
			abuf[j] = a[j];
		for (j = R - 1; j > 0; j--) // ����� ����� ����� �� ���� ������ (��������� �� 2)
		{
			a[j] <<= 1;
			a[j] |= a[j - 1] >> 31;
		}
		a[0] <<= 1;
		unsigned int transf = 0; // ������� � ����. ������
		for (j = 0; j < R; j++)
		{
			unsigned long long ALU = (unsigned long long int) a[j] + abuf[j] + transf;
			a[j] = (unsigned long int)ALU;
			transf = ALU >> 32;
		}
	}
	for (i = R - 1; i >= 0; i--)
		if (a[i] > 0)
			printf("%X", a[i]);
	printf("\n");
	system("pause");
}
