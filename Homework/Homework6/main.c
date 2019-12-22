#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include "mman.h"
#include <fcntl.h>
#include <string.h>
#include <sys/stat.h>


/*
	Compare two char objects.
	@param p1:		The first char.
	@param p2:		The second char.
	@return int:	Returns an integral value indicating the relationship between the strings:
			<0:		The first character that does not match has a lower value in p1 than in p2,
			0:		The contents of both strings are equal,
			>0:		The first character that does not match has a greater value in p1 than in p2.
*/
int charCmp(const void* p1, const void* p2)
{
    const char* s1;
    const char* s2;

    s1 = *(char**)p1;
    s2 = *(char**)p2;
    return(strcmp(s1, s2));
}

void printUsage() 
{
    const char description[] = "Usage: progname <input> <output>\n";
    printf("%s", description);
}

// The main program starts here.
int main(int argc, const char** argv)
{

    if (argc != 3) 
	{
        printUsage();
        return 0;
    }

    const char* input = argv[1];
    const char* output = argv[2];
    
    int flin, flout, flinSize;
    char* flinMapped;
    struct stat flinInfo;
    int i, k, len, j, maxLine, m, u;

    flin = _open(input, O_RDWR, 0);
    flout = _open(output, O_WRONLY | O_CREAT | O_TRUNC, S_IWRITE);

    if (flin == -1) 
	{
        printf("Unable to open file.\n");
        exit(0);
    }

    fstat(flin, &flinInfo);
    flinSize = flinInfo.st_size;
    flinMapped = mmap(0, flinSize, PROT_READ | PROT_WRITE, MAP_PRIVATE, flin, 0);
    len = strlen(flinMapped);
    u = 0;
    m = 0;
    maxLine = 0;

    for (i = 0; i < len; i++) 
	{
        if (flinMapped[i] != '\n') 
		{
            m++;
            if (m > maxLine) 
			{
                maxLine = m;
            }
        }
        else
		{
            m++;
            u++;
            if (m > maxLine)
			{
                maxLine = m;
            }
            m = 0;
        }
    }

    char** linePtr = (char**)malloc(u * sizeof(char*));
    char* str = (char*)malloc(maxLine * sizeof(char));

    if (flinMapped == MAP_FAILED) 
	{
        printf("Unable to map file.\n");
        exit(1);
    }

    k = 0;
    for (i = 0; i < u; ) 
	{
        j = 0;
        while (k < len) 
		{
            if (flinMapped[k] == '\n') {
                str[j] = '\n';
                j++;
                k++;
                break;
            }
            str[j] = flinMapped[k];
            j++;
            k++;
        }

        linePtr[i++] = _strdup(str);
        if (k == len)
		{
            break;
        }
    }

    qsort(linePtr, u, sizeof(char*), charCmp);
    char* floutMapped = (char*)malloc(len * sizeof(char*));
    k = 0;

    for (i = 0; i < u; i++) 
	{
        char* tmp = linePtr[i];
        while (*tmp != '\n')
		{
            floutMapped[k] = *tmp;
            k++;
            tmp++;
        }

        if (*tmp == '\n')
		{
            floutMapped[k] = '\n';
            k++;
        }

        free(linePtr[i]);
    }

    _write(flout, floutMapped, len);
    munmap(flinMapped, len);

    free(floutMapped);
    free(linePtr);
    free(str);

    _close(flin);
    _close(flout);
    return(0);
}