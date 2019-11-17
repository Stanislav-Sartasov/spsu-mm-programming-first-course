#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <sys/mman.h>
#include <fcntl.h>
#include <unistd.h>
#include <errno.h>
#include <string.h>

int fail(char *filename, int linenumber) {
    fprintf(stderr, "%s:%d %s\n", filename, linenumber, strerror(errno));
    exit(1);
    return 0; /*Make compiler happy */
}

#define QUIT fail(__FILE__, __LINE__ )

int cmp(const void *p1, const void *p2){
    return strcmp(* (char * const *) p1, * (char * const *) p2);
}

int main() {
    int fdin, fdout;
    void *src/*, *dst*/;
    fdin = open("input.txt", O_RDONLY);
    fdout = open("output.txt", O_RDWR | O_CREAT | O_TRUNC, 0600);
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

    int i = 0, max_len = 0, n = 0, sz = size / sizeof(char);
    while (i < sz)
    {
        int cur = 0;
        while (i < sz && array[i] != '\n')
        {
            cur++;
            i++;
        }
        if (i < sz)
            cur++;
        if (cur > max_len)
            max_len = cur;
        i++;
        n++;
    }

    max_len++;
    char **words = (char**)malloc(n * sizeof(char*));
    for (i = 0; i < n; i++)
    {
        words[i] = (char*)malloc(max_len * sizeof(char));
        memset(words[i], '\0', max_len);
    }

    i = 0;
    int k = 0;
    while (i < sz)
    {
        int j = 0;
        while (i < sz && array[i] != '\n')
        {
            words[k][j] = array[i];
            i++; j++;
        }
        if (i < sz)
            words[k][j] = array[i];
        i++;
        k++;
    }

    qsort(words, n, sizeof(char *), cmp);

    i = 0;

    for (int j = 0; j < n; j++)
    {
        for (int l = 0; l < max_len && words[j][l] != '\0'; l++)
        {
            array[i] = words[j][l];
            i++;
        }
    }

    write(fdout, src, size);

    for (i = 0; i < n; i++)
        free(words[i]);
    free(words);

    munmap(src, size);
    //munmap(dst, size);
    return 0;
}
