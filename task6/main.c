// ./bin/Debug/task6 input.txt output.txt

#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <sys/mman.h>
#include <fcntl.h>
#include <unistd.h>
#include <errno.h>
#include <string.h>

int fail (char *filename, int linenumber)
{
    fprintf(stderr, "%s:%d %s\n", filename, linenumber, strerror(errno));
    exit(1);
    return 0; /*Make compiler happy */
}

#define QUIT fail(__FILE__, __LINE__ )

int cmp (const void *p1, const void *p2)
{
    char *a = *(char * const *) p1;
    char *b = *(char * const *) p2;
    int i;
    for (i = 0; (*(a + i)) != '\n' && (*(b + i)) != '\n'; i++)
    {
        if (*(a + i) != *(b + i))
            return (*(a + i) > *(b + i));
    }
    return (i != 0 && *(a + i) != '\n');
}

int main(int argc, char* argv[])
{
    int fdin, fdout;
    void *src/*, *dst*/;
	if (argc != 3)
	{
		printf("Check the parameters are correct");
		exit(-1);
	}

	if ((fdin = open(argv[1], O_RDONLY)) == -1)
	{
		printf("Input file open failed");
		exit(-1);
	}
	if ((fdout = open(argv[2], O_RDWR | O_CREAT | O_TRUNC, 0600)) == -1)
	{
		printf("Output file create failed");
		exit(-1);
	}
    struct stat st;
    fstat(fdin, &st);
    int size = st.st_size;
    /*lseek(fdout, size - 1, SEEK_SET);
    write(fdout, "", 1);*/
    src = mmap(0, size, PROT_READ | PROT_WRITE, MAP_PRIVATE, fdin, 0);
    //dst = mmap(0, size, PROT_READ | PROT_WRITE, MAP_PRIVATE, fdout, 0);

    if (src == (void*) -1 /*|| dst == (void*) -1*/)
        QUIT;

    char *array = src;

    int i = 0, n = 0, sz = size / sizeof(char);
    while (i < sz)
    {
        while (i < sz && array[i] != '\n')
            i++;
        i++;
        n++;
    }

    char **words = (char**)malloc(n * sizeof(char*));

    i = 0;
    int k = 0;
    while (i < sz)
    {
        words[k] = &(array[i]);
        while (i < sz && array[i] != '\n')
            i++;
        i++;
        k++;
    }

    qsort(words, n, sizeof(char*), cmp);

    i = 0;
    char ch = '\n';
    for (int k = 0; k < n; k++)
    {
        int j = 0;
        while (*(words[k] + j) != '\n')
        {
            write(fdout, (words[k] + j), sizeof(char));
            j++;
        }
        write(fdout, &ch, sizeof(char));
        j++;
    }

    free(words);

    munmap(src, size);
    close(fdin);
    close(fdout);
    //munmap(dst, size);
    return 0;
}
