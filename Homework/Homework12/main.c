#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define CS_BASE 16				// система счисления, в которой выполняется расчёт и вывод на экран

typedef struct
{
	unsigned int Length;		// размер массива
	unsigned int Count;			// количество чисел в массиве
	char Sign;					// знак числа
	unsigned __int8* Value;		// массив чисел, представленный в виде символов
} BigInt;


// инициализация экземпляра структуры BigInt
BigInt BigIntInit()
{
	BigInt bigNumber;
	bigNumber.Length = 100;
	bigNumber.Count = 1;
	bigNumber.Value = (unsigned __int8*)malloc(bigNumber.Length);
	memset(bigNumber.Value, 0, (size_t)bigNumber.Length);
	bigNumber.Sign = '+';
	return bigNumber;
}
// инициализация посредством значения
BigInt BigIntInitValue(int value)
{
	BigInt bigNumber = BigIntInit();
	if (value < 0)
	{
		bigNumber.Sign = '-';
		value *= -1;
	}
	unsigned int index = 0;
	unsigned int k = 0;

	while (value > 0)
	{
		k = value / CS_BASE;
		bigNumber.Value[index] = value - k * CS_BASE;
		value = k;
		index++;
	}
	bigNumber.Count = index;
	return bigNumber;
}
// присвоение значения
int BigIntSetValue(BigInt* bigNumber, int value)
{
	if (value < 0)
	{
		bigNumber->Sign = '-';
		value *= -1;
	}
	else {
		bigNumber->Sign = '+';
	}
	memset(bigNumber->Value, 0, (size_t)bigNumber->Count);
	unsigned int index = 0;
	unsigned int k = 0;

	while (value > 0)
	{
		k = value / CS_BASE;
		bigNumber->Value[index] = value - k * CS_BASE;
		value = k;
		index++;
	}
	bigNumber->Count = index;
}

// очищает данные (нужно использовать, чтобы предотвратить утечки памяти, т.к. массив выделяется динамически)
void BigIntClear(BigInt* bigNumber)
{
	free(bigNumber->Value);
	bigNumber->Count = 0;
	bigNumber->Length = 0;
	bigNumber->Sign = NULL;
}
// вывод в консоль в десятичном формате
void BigIntPrint(BigInt* bigNumber)
{
	if (bigNumber->Sign == '-')
		printf("%c", bigNumber->Sign);
	for (int index = bigNumber->Count - 1; index >= 0; --index)
	{
		unsigned __int8 value = bigNumber->Value[index];
		switch (value)
		{
		case 0: case 1: case 2: case 3: case 4: case 5: case 6: case 7: case 8: case 9:
			printf("%d", value);
			break;
		case 10:
			printf("A");
			break;
		case 11:
			printf("B");
			break;
		case 12:
			printf("C");
			break;
		case 13:
			printf("D");
			break;
		case 14:
			printf("E");
			break;
		case 15:
			printf("F");
			break;
		default:
			printf("error"); 
			break;
		}

	}
}

// изменение размера числа
int BigIntResize(BigInt* bigNumber, unsigned int newSize)
{
	if (newSize < bigNumber->Count + 10)
		return 0;
	unsigned __int8* array = (unsigned __int8*)malloc(newSize);
	memset(array, 0, (size_t)newSize);
	memcpy(array, bigNumber->Value, (size_t)bigNumber->Count);
	free(bigNumber->Value);
	bigNumber->Value = array;
	bigNumber->Length = newSize;
	return 1;
}
// сравнение значений чисел по модулю
int BigIntCompare(BigInt* firstBigN, BigInt* secondBigN)
{
	if (firstBigN->Count > secondBigN->Count)
		return 1;
	else if (secondBigN->Count > firstBigN->Count)
		return 2;
	for (int index = firstBigN->Count - 1; index >= 0; --index)
	{
		if (firstBigN->Value[index] > secondBigN->Value[index])
			return 1;
		else if (secondBigN->Value[index] > firstBigN->Value[index])
			return 2;
	}
	return 0;
}
// меняет числа местами
int BigIntSwap(BigInt* firstBigN, BigInt* secondBigN)
{
	if (firstBigN == NULL || secondBigN == NULL)
		return 0;
	BigInt* bigInt = secondBigN;
	secondBigN = firstBigN;
	firstBigN = bigInt;
	return 1;
}
// обнуление числа
void BigIntZero(BigInt* bigNumber)
{
	memset(bigNumber->Value, 0, (size_t)bigNumber->Count);
	bigNumber->Count = 1;
	bigNumber->Sign = '+';
}
// копирование числа
BigInt BigIntInitCopy(BigInt* bigNumber)
{
	BigInt newNumber;
	newNumber.Sign = bigNumber->Sign;
	newNumber.Length = bigNumber->Length;
	newNumber.Count = bigNumber->Count;
	newNumber.Value = (unsigned __int8*)malloc(newNumber.Length);
	memcpy(newNumber.Value, bigNumber->Value, (size_t)bigNumber->Length);
	return newNumber;
}
// копирование значения из одного числа в другое
void BigIntCopy(BigInt* copyTo, BigInt* copyFrom)
{
	if (copyTo->Length < copyFrom->Count)
		BigIntResize(copyTo, copyFrom->Length);
	memcpy(copyTo, copyFrom, copyFrom->Count);
	copyTo->Count = copyFrom->Count;
	memset(copyTo->Value[copyTo->Count], 0, copyTo->Length - copyTo->Count);
	copyTo->Sign = copyFrom->Sign;
}

// умножение числа на порядок
void BigIntShift(BigInt* bigNumber, unsigned int times)
{
	if (times == 0) return;
	for (unsigned int index = bigNumber->Count + times - 1; index >= times; --index)
		bigNumber->Value[index] = bigNumber->Value[index - 1];
	for (unsigned int index = 0; index < times; ++index)
		bigNumber->Value[index] = 0;
	bigNumber->Count += times;
}

// вспомогательная функция сложения больших чисел
void BigIntPlusBigUInt(BigInt* firstBigN, BigInt* secondBigN)
{
	unsigned int count = firstBigN->Count > secondBigN->Count ? firstBigN->Count : secondBigN->Count;
	if (count >= firstBigN->Length)
		BigIntResize(firstBigN, secondBigN->Length);
	unsigned __int8 buffer = 0;
	unsigned int index = 0;
	while (index < count || buffer > 0)
	{
		if (index >= firstBigN->Length - 10)
		{
			firstBigN->Count = index + 1;
			BigIntResize(firstBigN, firstBigN->Length + 50);
		}
		buffer += firstBigN->Value[index] + secondBigN->Value[index];
		firstBigN->Value[index] = buffer % CS_BASE;
		buffer /= CS_BASE;
		index++;
	}
	firstBigN->Count = index;
}
// вспомогательная функция вычитания больших чисел
void BigIntMinusBigUInt(BigInt* firstBigN, BigInt* secondBigN)
{
	__int8 buffer = 0;
	unsigned int index = 0;
	while (index < firstBigN->Count)
	{
		buffer = firstBigN->Value[index] - secondBigN->Value[index];
		if (buffer < 0)
		{
			buffer += CS_BASE;
			firstBigN->Value[index + 1]--;
		}
		firstBigN->Value[index] = buffer;
		index++;
	}
	for (index = firstBigN->Count - 1; index >= 0; --index)
		if (firstBigN->Value[index] != 0)
		{
			firstBigN->Count = index + 1;
			break;
		}
}
// сложение больших чисел
void BigIntPlusBigInt(BigInt* firstBigN, BigInt* secondBigN)
{
	if (firstBigN->Sign == secondBigN->Sign)
	{
		BigIntPlusBigUInt(firstBigN, secondBigN);
	}
	else {
		int comp = BigIntCompare(firstBigN, secondBigN);
		if (comp == 0)
			BigIntZero(firstBigN);
		else if (comp == 1)
			BigIntMinusBigUInt(firstBigN, secondBigN);
		else {
			BigInt temp = BigIntInitCopy(secondBigN);
			BigIntMinusBigUInt(&temp, firstBigN);
			(*firstBigN) = BigIntInitCopy(&temp);
			BigIntClear(&temp);
		}
	}
}
// вычитание больших чисел
void BigIntMinusBigInt(BigInt* firstBigN, BigInt* secondBigN)
{
	char sign = secondBigN->Sign;
	if (sign == '+')
		secondBigN->Sign = '-';
	else
		secondBigN->Sign = '+';
	BigIntPlusBigInt(firstBigN, secondBigN);
	secondBigN->Sign = sign;
}

// вспомогательная функция умножения на число от 0 до 9
void BigIntMultInt(BigInt* firstBigN, unsigned __int8 number)
{
	unsigned __int8 buffer = 0;
	unsigned int index = 0;
	unsigned int count = firstBigN->Count;
	while (index < count || buffer > 0)
	{
		if (index >= firstBigN->Length - 10)
		{
			firstBigN->Count = index + 1;
			BigIntResize(firstBigN, firstBigN->Length + 50);
		}
			
		buffer += firstBigN->Value[index] * number;
		firstBigN->Value[index] = buffer % CS_BASE;
		buffer /= CS_BASE;
		index++;
	}
	firstBigN->Count = index;
}
// умножение двух больших чисел
void BigIntMultBigInt(BigInt* firstBigN, BigInt* secondBigN)
{
	if (firstBigN->Sign != secondBigN->Sign)
		firstBigN->Sign = '-';
	else
		firstBigN->Sign = '+';

	BigInt buffer = BigIntInitCopy(firstBigN);
	BigIntZero(firstBigN);

	for (unsigned int index = 0; index < secondBigN->Count; ++index)
	{
		BigInt temp = BigIntInitCopy(&buffer);
		BigIntMultInt(&temp, secondBigN->Value[index]);
		BigIntPlusBigInt(firstBigN, &temp);
		BigIntClear(&temp);
		BigIntShift(&buffer, 1);
	}
	BigIntClear(&buffer);
}
// возведение в степень (степень только положительная, так как отрицательная степень даёт дробное число)
void BigIntPowBigInt(BigInt* firstBigN, BigInt* secondBigN)
{
	if (secondBigN->Sign == '-') return;
	if (firstBigN->Sign == '-' && secondBigN->Value[0] % 2 == 0)
		firstBigN->Sign = '+';
	BigInt temp = BigIntInitCopy(firstBigN);
	BigInt one = BigIntInitValue(1);
	BigInt zero = BigIntInitValue(1);
	while (BigIntCompare(secondBigN, &zero) == 1)
	{
		BigIntMultBigInt(firstBigN, &temp);
		BigIntPlusBigInt(&zero, &one);
	}
	BigIntClear(&temp);
	BigIntClear(&one);
	BigIntClear(&zero);
}

int main()
{
	printf("Big numbers calculating. Scale of notation is %d.\n", CS_BASE);
	
	BigInt number1 = BigIntInitValue(3);
	BigInt number2 = BigIntInitValue(10000);

	printf("Initial values:");
	printf("\nFirst number ");
	BigIntPrint(&number1);
	printf("\nSecond number: ");
	BigIntPrint(&number2);

	printf("\nCalculating...");
	BigIntPowBigInt(&number1, &number2);
	
	printf("\n Result ");
	BigIntPrint(&number1);
	
	printf("\n");

	BigIntClear(&number1);
	BigIntClear(&number2);

	return 0;
}