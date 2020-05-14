#include <stdio.h>
#include <fcntl.h>
#include <sys/stat.h>
#include <string.h>
#include <stdlib.h>
#include "mman.h"

#define N 50

typedef struct
{
	char* line;				// указатель на строку
	unsigned int length;	// длина строки
} 
string;
// вывод на печать строки
void stringPrint(string* string)
{
	for (size_t index = 0; index < string->length; ++index)
		printf("%c", string->line[index]);
}
// сравнение двух по содержимому
int stringCompare(string* first, string* second)
{
	return strncmp(first->line, second->line, 
		first->length < second->length ? first->length : second->length);
}

int main(int argc, const char** argv)
{
	printf("This programm sorts text\n");
	char inFileName[N] = { 0 }, outFileName[N] = { 0 };
	
	// считывание названий файлов из параметров для exe файла или
	// при их несоответствии из консоли
	if (argc == 3)
	{
		for (uint32_t index = 0; argv[1][index] != 0; ++index)
			inFileName[index] = argv[1][index];
		for (uint32_t index = 0; argv[2][index] != 0; ++index)
			outFileName[index] = argv[2][index];
	}
	else 
	{
		printf("Enter file name to read: ");
		scanf_s("%s", inFileName, N - 1);
		printf("Enter file name to write: ");
		scanf_s("%s", outFileName, N - 1);
	}
	printf("You have entered: %s, %s.\n", inFileName, outFileName);

	// открытие файлов для чтения (дескриптор inFile) и для записи (дескриптор outFile)
	FILE* inFile = NULL, * outFile = NULL;
	if (fopen_s(&inFile, inFileName, "r+") != 0)
	{
		printf("%s could not be opened for reading.\n", inFileName);
		return -1;
	}
	if (fopen_s(&outFile, outFileName, "w+") != 0)
	{
		printf("%s could not be opened for writing.\n", outFileName);
		return -1;
	}

	// если в конце файла нет символа конца строки, то добавляем его
	// иначе не будет перевода на новую строку после сортировки
	if (fseek(inFile, -1, SEEK_END) != 0)
	{
		printf("Could not size the input file.\n");
		fclose(inFile);
		fclose(outFile);
		return -1;
	}
	char symbol;
	fread(&symbol, 1, 1, inFile);
	if (symbol != '\n')
	{
		fwrite("\n", 1, 1, inFile);
		fclose(inFile);
		fopen_s(&inFile, inFileName, "r");
	}

	// считываем информацию о размере файла для чтения
	struct stat fileStat;
	if (fstat(_fileno(inFile), &fileStat) != 0)
	{
		printf("Could not get the file size.\n");
		fclose(inFile);
		fclose(outFile);
		return -1;
	}
	else
		printf("The size of the file %s is %ld.\n", inFileName,	 fileStat.st_size);
	// устанавливаем соответствующий размер файла для записи
	if (fseek(outFile, fileStat.st_size - 1, SEEK_SET) != 0)
	{
		printf("Could not size the output file.\n");
		fclose(inFile);
		fclose(outFile);
		return -1;
	}
	//fwrite("", 1, 1, outFile);

	// отображение файлов в память
	char* source, * destin;
	source = (char*)mmap(0, fileStat.st_size, PROT_READ, MAP_SHARED, _fileno(inFile), 0);
	if (source == MAP_FAILED)
	{
		printf("Could not map file %s.\n", inFileName);
		fclose(inFile);
		fclose(outFile);
		return -1;
	}
	destin = (char*)mmap(0, fileStat.st_size, PROT_READ | PROT_WRITE, MAP_SHARED, _fileno(outFile), 0);
	if (destin == MAP_FAILED)
	{
		printf("Could not map file %s.\n", outFile);
		fclose(inFile);
		fclose(outFile);
		return -1;
	}

	// проводим манипуляции по сортировке строк

	size_t count = 1;
	// рассчитываем количество строк
	for (size_t index = 0; index < fileStat.st_size; ++index)
	{
		if (source[index] == '\n')
			count++;
	}
	// массив строк представления
	string* lines = (string*)malloc(count * sizeof(string));
	lines[0].line = &(source[0]);
	count = 0;
	size_t length = 0;

	// запоминаем указатели на начала строк файла и их длины
	for (size_t index = 0; source[index] != 0; ++index)
	{
		length++;
		if (source[index] == '\n')
		{
			lines[count].length = length;
			length = 0;
			lines[++count].line = &(source[index + 1]);
		}
	}
	lines[count].length = length + 1;

	qsort(lines, count, sizeof(string), stringCompare);
	
	length = 0;
	for (size_t index = 0; index < count + 1; ++index) 
	{
		for (size_t number = 0; number < lines[index].length; ++number)
			destin[length + number] = lines[index].line[number];
		length += lines[index].length;
	}


	free(lines);
	fclose(inFile);
	fclose(outFile);

	return 0;
}