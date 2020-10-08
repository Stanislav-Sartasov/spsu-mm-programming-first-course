#include "myMalloc.h"

int main()
{
	init();

	char* characters = (char*)myMalloc(sizeof(char) * 4);
	characters[0] = 'a';
	characters[1] = 'b';
	characters[2] = 'c';
	characters[3] = 'd';

	for (int i = 0; i < 4; ++i)
		printf("%c\n", characters[i]); // проверка 1 массива символов после создания

	int* digits = (int*)myMalloc(sizeof(int) * 5);
	digits[0] = 5;
	digits[1] = 8;
	digits[2] = 9;
	digits[3] = 15;
	digits[4] = 20;

	for (int i = 0; i < 5; ++i)
		printf("%d\n", digits[i]); // проверка 1 массива чисел после создания

	char* characters2 = (char*)myMalloc(sizeof(char) * 10);
	characters2[0] = 'e';
	characters2[1] = 'f';
	characters2[2] = 'g';
	characters2[3] = 'h';
	characters2[4] = 'i';
	characters2[5] = 'j';
	characters2[6] = 'k';
	characters2[7] = 'l';
	characters2[8] = 'm';
	characters2[9] = 'n';


	for (int i = 0; i < 10; ++i)
		printf("%c\n", characters2[i]); // проверка 2 массива символов после создания

	myFree(digits);

	int* digits2 = (int*)myMalloc(sizeof(int) * 5);
	digits2[0] = 10;
	digits2[1] = 11;
	digits2[2] = 12;
	digits2[3] = 13;
	digits2[4] = 14;

	
	for (int i = 0; i < 5; ++i)
		printf("%d\n", digits2[i]); // проверка 2 массива чисел после создания

	for (int i = 0; i < 5; ++i)
		printf("%d\n", digits[i]); // проверка 1 массива чисел после удаления первого 

	myFree(characters);
	myFree(characters2);

	char* characters3 = (char*)myMalloc(sizeof(char) * 2);
	characters3[0] = 'x';
	characters3[1] = 'y';

	for (int i = 0; i < 2; ++i)
		printf("%c\n", characters3[i]); // проверка 3 массива символов после создания

	for (int i = 0; i < 4; ++i)
		printf("%c\n", characters[i]); // проверка 1 массива символов после удаления

	for (int i = 0; i < 3; ++i)
		printf("%c\n", characters2[i]); // проверка 2 массива символов после удаления

	characters3 = myRealloc(characters3, sizeof(char) * 4);
	characters3[2] = 'w';
	characters3[3] = 'z';

	for (int i = 0; i < 3; ++i)
		printf("%c\n", characters3[i]); // проверка 3 массива символов после создания

	digits2 = (int*)myRealloc(digits2, sizeof(int) * 7);
	digits2[5] = 15;
	digits2[6] = 16;

	for (int i = 0; i < 7; ++i)
		printf("%d\n", digits2[i]); // проверка 2 массива чисел после добавления элементов в существующий массив

	digits2 = (int*)myRealloc(digits2, sizeof(int) * 14);
	digits2[7] = 31;
	digits2[8] = 32;
	digits2[9] = 33;
	digits2[10] = 34;
	digits2[11] = 35;
	digits2[12] = 36;
	digits2[13] = 37;

	for (int i = 0; i < 14; ++i)
		printf("%d\n", digits2[i]); // проверяем 2 массив чисел после добавления элементов, которым требуется больше памяти, чем существует в выделенном блоке

	for (int i = 0; i < 5; ++i)
		printf("%d\n", digits[i]); // проверка содержимого в 1 массиве чисел
	init();
	return 0;
}