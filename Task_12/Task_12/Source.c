//#define _CRT_SECURE_NO_WARNINGS
//#include <stdio.h>
//#include <stdlib.h>
//
//int main()
//{
//	char* mas = (char*)calloc(sizeof(char), 1);
//	mas[0] = 3;
//	int count = 1;
//
//	printf("The program calculates 3^5000\n");
//	for (int i = 0; i < 4999; i++)
//	{
//	
//
//		for (int j = 0; j < count; j++)
//		{
//			mas[j] = mas[j] * 3;
//		}
//		
//		if (mas[count - 1] > 15)
//		{
//			if (!mas)
//			{
//				printf("no");
//			}
//			else
//			{
//				mas = realloc(mas, (count + 1) * sizeof(char));
//				mas[count] = 0;
//				count = count + 1;
//			}
//		}
//
//		for (int j = 0; j < count - 1 ; j++)
//		{
//			if (mas[j] > 15)
//			{
//				mas[j + 1] = mas[j + 1] + (char)(mas[j] / 16);
//				mas[j] = mas[j] % 16;
//			}
//		}
//	}
//
//
//
//
//	for (int i = (count - 1); i >= 0 ; i--)
//	{
//		switch (mas[i])
//		{
//		case 10:
//			printf("A");
//			break;
//		case 11:
//			printf("B");
//			break;
//		case 12:
//			printf("C");
//			break;
//		case 13:
//			printf("D");
//			break;
//		case 14:
//			printf("E");
//			break;
//		case 15:
//			printf("F");
//			break;
//		default:
//			printf("%d", mas[i]);
//			break;
//		}
//		
//
//	}
//	free(mas);
//	return 0;
//}


#define _CRT_SECURE_NO_WARNINGS 
#include <stdio.h> 


int main()
{
	float arr[10], y, ynew, size;
	double result;
	int k, i, j, facti, factk;
	int x;
	size = sizeof(arr) / sizeof(float);
	for (i = 0; i < size; i++)
		scanf("%f ", &arr[i]);
	scanf("%d", &x);
	scanf("%d ", &k);
	scanf("%f ", &y);
	facti = 1;
	factk = 1;
	result = 0;
	ynew = 1;
	for (i = k; i < size; i++)
	{
		if (i - k == 0)
			for (j = 1; j < k + 1; j++)
				facti = facti * j;
		else
		{
			for (j = 1; j < i + 1; j++)
				facti = facti * j;
			for (j = 1; j < i - k + 1; j++)
			{
				factk = factk * j;
				ynew *= y;
			}
		}
		result += arr[i] * ynew * facti / factk;
	}
	printf("%lf ", result);
	return 0;
}