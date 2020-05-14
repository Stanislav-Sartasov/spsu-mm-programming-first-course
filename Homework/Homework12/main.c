#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define CS_BASE 16				// система счисления, в которой выполняется расчёт и вывод на экран
#define SHIFT 5

typedef struct
{
	unsigned int length;		// размер массива
	unsigned int count;			// количество чисел в массиве
	char sign;					// знак числа
	unsigned __int8* value;		// массив чисел, представленный в виде символов
} 
bigInt;

// инициализация экземпляра структуры bigInt
bigInt bigIntInit()
{
	bigInt bigNumber;
	bigNumber.length = SHIFT;
	bigNumber.count = 1;
	bigNumber.value = (unsigned __int8*)malloc(bigNumber.length);
	memset(bigNumber.value, 0, (size_t)bigNumber.length);
	bigNumber.sign = '+';
	return bigNumber;
}
// очищает данные (нужно использовать, чтобы предотвратить утечки памяти, т.к. массив выделяется динамически)
void bigIntClear(bigInt* bigNumber)
{
	free(bigNumber->value);
	bigNumber->count = 0;
	bigNumber->length = 0;
	bigNumber->sign = NULL;
}
// изменение размера числа
int bigIntResize(bigInt* bigNumber, unsigned int newSize)
{
	if (newSize < bigNumber->count + SHIFT)
		return 0;
	unsigned __int8* array = (unsigned __int8*)malloc(newSize);
	memset(array, 0, (size_t)newSize);
	memcpy(array, bigNumber->value, (size_t)bigNumber->count);
	free(bigNumber->value);
	bigNumber->value = array;
	bigNumber->length = newSize;
	return 1;
}
// обнуление числа
void bigIntZero(bigInt* bigNumber)
{
	bigIntClear(bigNumber);
	bigNumber->length = SHIFT;
	bigNumber->count = 1;
	bigNumber->sign = '+';
	bigNumber->value = (unsigned __int8*)malloc(bigNumber->length);
	memset(bigNumber->value, 0, bigNumber->count);
}
// инициализация посредством значения
bigInt bigIntInitvalue(int value)
{
	bigInt bigNumber = bigIntInit();
	if (value < 0)
	{
		bigNumber.sign = '-';
		value *= -1;
	}
	unsigned int index = 0;
	unsigned int k = 0;
	unsigned int compare = 1;

	while (compare < value)
	{
		compare *= 10;
		index++;
	}
	bigIntResize(&bigNumber, index + SHIFT);
	index = 0;

	while (value > 0)
	{
		k = value / CS_BASE;
		bigNumber.value[index] = value - k * CS_BASE;
		value = k;
		index++;
	}
	bigNumber.count = index;
	return bigNumber;
}
// присвоение значения
int bigIntSetvalue(bigInt* bigNumber, int value)
{
	if (value < 0)
	{
		bigNumber->sign = '-';
		value *= -1;
	}
	else 
	{
		bigNumber->sign = '+';
	}
	bigIntZero(bigNumber);

	unsigned int index = 0;
	unsigned int k = 0;
	unsigned int compare = 1;

	while (compare < value)
	{
		compare *= 10;
		index++;
	}
	bigIntResize(bigNumber, index + SHIFT);
	index = 0;

	while (value > 0)
	{
		k = value / CS_BASE;
		bigNumber->value[index] = value - k * CS_BASE;
		value = k;
		index++;
	}
	bigNumber->count = index;
}

// вывод в консоль в десятичном формате
void bigIntPrint(bigInt* bigNumber)
{
	if (bigNumber->sign == '-')
		printf("%c", bigNumber->sign);
	for (int index = bigNumber->count - 1; index >= 0; --index)
	{
		unsigned __int8 value = bigNumber->value[index];
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

// сравнение значений чисел по модулю
int bigIntCompare(bigInt* firstBigN, bigInt* secondBigN)
{
	if (firstBigN->count > secondBigN->count)
		return 1;
	else if (secondBigN->count > firstBigN->count)
		return 2;
	for (int index = firstBigN->count - 1; index >= 0; --index)
	{
		if (firstBigN->value[index] > secondBigN->value[index])
			return 1;
		else if (secondBigN->value[index] > firstBigN->value[index])
			return 2;
	}
	return 0;
}
// меняет числа местами
int bigIntSwap(bigInt* firstBigN, bigInt* secondBigN)
{
	if (firstBigN == NULL || secondBigN == NULL)
		return 0;
	bigInt* bigInt = secondBigN;
	secondBigN = firstBigN;
	firstBigN = bigInt;
	return 1;
}
// копирование числа
bigInt bigIntInitCopy(bigInt* bigNumber)
{
	bigInt newNumber;
	newNumber.sign = bigNumber->sign;
	newNumber.length = bigNumber->length;
	newNumber.count = bigNumber->count;
	newNumber.value = (unsigned __int8*)malloc(newNumber.length);
	memcpy(newNumber.value, bigNumber->value, (size_t)bigNumber->length);
	return newNumber;
}
// копирование значения из одного числа в другое
void bigIntCopy(bigInt* copyTo, bigInt* copyFrom)
{
	if (copyTo->length < copyFrom->count)
		bigIntResize(copyTo, copyFrom->length);
	memcpy(copyTo, copyFrom, copyFrom->count);
	copyTo->count = copyFrom->count;
	memset(copyTo->value[copyTo->count], 0, copyTo->length - copyTo->count);
	copyTo->sign = copyFrom->sign;
}

// умножение числа на порядок
void bigIntShift(bigInt* bigNumber, unsigned int times)
{
	if (times == 0) return;
	bigIntResize(bigNumber, bigNumber->length + times);
	for (unsigned int index = bigNumber->count + times - 1; index >= times; --index)
		bigNumber->value[index] = bigNumber->value[index - 1];
	for (unsigned int index = 0; index < times; ++index)
		bigNumber->value[index] = 0;
	bigNumber->count += times;
}

// вспомогательная функция сложения больших чисел
void bigIntPlusBigUInt(bigInt* firstBigN, bigInt* secondBigN)
{
	unsigned int count = firstBigN->count > secondBigN->count ? firstBigN->count : secondBigN->count;
	if (count >= firstBigN->length)
		bigIntResize(firstBigN, secondBigN->length);
	unsigned __int8 buffer = 0;
	unsigned int index = 0;
	while (index < count || buffer > 0)
	{
		if (index >= firstBigN->length - SHIFT)
		{
			firstBigN->count = index + 1;
			bigIntResize(firstBigN, firstBigN->length + SHIFT);
		}
		buffer += firstBigN->value[index] + secondBigN->value[index];
		firstBigN->value[index] = buffer % CS_BASE;
		buffer /= CS_BASE;
		index++;
	}
	firstBigN->count = index;
}
// вспомогательная функция вычитания больших чисел
void bigIntMinusBigUInt(bigInt* firstBigN, bigInt* secondBigN)
{
	__int8 buffer = 0;
	unsigned int index = 0;
	while (index < firstBigN->count)
	{
		buffer = firstBigN->value[index] - secondBigN->value[index];
		if (buffer < 0)
		{
			buffer += CS_BASE;
			firstBigN->value[index + 1]--;
		}
		firstBigN->value[index] = buffer;
		index++;
	}
	for (index = firstBigN->count - 1; index >= 0; --index)
		if (firstBigN->value[index] != 0)
		{
			firstBigN->count = index + 1;
			break;
		}
}
// сложение больших чисел
void bigIntPlusBigInt(bigInt* firstBigN, bigInt* secondBigN)
{
	if (firstBigN->sign == secondBigN->sign)
	{
		bigIntPlusBigUInt(firstBigN, secondBigN);
	}
	else 
	{
		int comp = bigIntCompare(firstBigN, secondBigN);
		if (comp == 0)
			bigIntZero(firstBigN);
		else if (comp == 1)
			bigIntMinusBigUInt(firstBigN, secondBigN);
		else 
		{
			bigInt temp = bigIntInitCopy(secondBigN);
			bigIntMinusBigUInt(&temp, firstBigN);
			(*firstBigN) = bigIntInitCopy(&temp);
			bigIntClear(&temp);
		}
	}
}
// вычитание больших чисел
void bigIntMinusBigInt(bigInt* firstBigN, bigInt* secondBigN)
{
	char sign = secondBigN->sign;
	if (sign == '+')
		secondBigN->sign = '-';
	else
		secondBigN->sign = '+';
	bigIntPlusBigInt(firstBigN, secondBigN);
	secondBigN->sign = sign;
}

// вспомогательная функция умножения на число от 0 до 9
void bigIntMultInt(bigInt* firstBigN, unsigned __int8 number)
{
	unsigned __int8 buffer = 0;
	unsigned int index = 0;
	unsigned int count = firstBigN->count;
	while (index < count || buffer > 0)
	{
		if (index >= firstBigN->length - SHIFT)
		{
			firstBigN->count = index + 1;
			bigIntResize(firstBigN, firstBigN->length + SHIFT);
		}
			
		buffer += firstBigN->value[index] * number;
		firstBigN->value[index] = buffer % CS_BASE;
		buffer /= CS_BASE;
		index++;
	}
	firstBigN->count = index;
}
// умножение двух больших чисел
void bigIntMultBigInt(bigInt* firstBigN, bigInt* secondBigN)
{
	if (firstBigN->sign != secondBigN->sign)
		firstBigN->sign = '-';
	else
		firstBigN->sign = '+';

	bigInt buffer = bigIntInitCopy(firstBigN);
	bigIntZero(firstBigN);

	for (unsigned int index = 0; index < secondBigN->count; ++index)
	{
		bigInt temp = bigIntInitCopy(&buffer);
		bigIntMultInt(&temp, secondBigN->value[index]);
		bigIntPlusBigInt(firstBigN, &temp);
		bigIntClear(&temp);
		bigIntShift(&buffer, 1);
	}
	bigIntClear(&buffer);
}
// возведение в степень (степень только положительная, так как отрицательная степень даёт дробное число)
void bigIntPowBigInt(bigInt* firstBigN, bigInt* secondBigN)
{
	if (secondBigN->sign == '-') return;
	if (firstBigN->sign == '-' && secondBigN->value[0] % 2 == 0)
		firstBigN->sign = '+';
	bigInt temp = bigIntInitCopy(firstBigN);
	bigInt one = bigIntInitvalue(1);
	bigInt zero = bigIntInitvalue(1);
	while (bigIntCompare(secondBigN, &zero) == 1)
	{
		bigIntMultBigInt(firstBigN, &temp);
		bigIntPlusBigInt(&zero, &one);
	}
	bigIntClear(&temp);
	bigIntClear(&one);
	bigIntClear(&zero);
}

int main()
{
	printf("Big numbers calculating. Scale of notation is %d.\n", CS_BASE);
	
	bigInt number1 = bigIntInitvalue(3);
	bigInt number2 = bigIntInitvalue(10000);

	printf("Initial values:");
	printf("\nFirst number ");
	bigIntPrint(&number1);
	printf("\nSecond number: ");
	bigIntPrint(&number2);

	printf("\nCalculating...");
	bigIntPowBigInt(&number1, &number2);
	
	printf("\n Result ");
	bigIntPrint(&number1);
	
	printf("\n");

	bigIntClear(&number1);
	bigIntClear(&number2);

	return 0;
}