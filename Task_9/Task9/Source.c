#include <stdio.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>
#define SIZE_MEMORY ((long long int)pow(1024, 3))
#define SIZE_BLOCK  (size + check_size_block(size))

typedef unsigned char u_char;
void* memory = NULL;

/*функция определяет сколько байт нужно для хранения размера выделенного блока памяти*/
int check_size_block(size_t size)
{
    int n = 0;
    for (n = 0; size > 0; size >>= 1)
        n++;
    return ((n % 8 == 0) ? (int)(n / 8) : (int)(n / 8) + 1);
}

void* init()
{
    memory = (void*)calloc(SIZE_MEMORY, 1);
}

void* find(u_char* p, size_t size)
{
    u_char flag = 0;
    if (p + SIZE_BLOCK > & (((u_char*)memory)[SIZE_MEMORY]))
    {
        printf("Out of memory");
        return NULL;
    }
    if (!(*p))
    {
        for (int i = 0; i < SIZE_BLOCK; i++)
        {
            if (*p)
            {
                flag = 1;
                break;
            }
            p++;
        }
    }
    else
    {
        flag = 1;
    }

    if (!flag)
    {
        return p - SIZE_BLOCK;
    }

    int len = *p;
    while (*p == 255)
    {
        len += *p;
        p++;
    }
    p = p + len;
    return find(p, size);
}

void* my_malloc(size_t size)
{
    u_char* p = find((u_char*)memory, size);
    *p = (u_char)(size % 255);
    p++;
    for (int i = 0; i < (int)(size / 255); i++)
    {
        *p = 255;
        p++;
    }
    p++;
    return (void*)p;
}

void my_free(void* p)
{
    u_char* ptr_free = p;
    ptr_free = ptr_free - 2;
    int len = 2;
    while (*ptr_free == 255)
    {
        len += 256;
        *ptr_free--;
    }
    len += *ptr_free;
    for (int i = 0; i < len; i++)
    {
        *ptr_free = (u_char)NULL;
        ptr_free++;
    }
}

void* my_realloc(void* p, size_t size)
{
    void* new_mas;
    new_mas = (void*)my_malloc(size);
    memcpy(new_mas, p, size);
    my_free(p);
    return (void*)new_mas;
}

void total_free()
{
    free(memory);
}

typedef struct test
{
    int x;
    int y;
}test;

int main()
{
    printf("The program solves the problem \"memory manager\"\n");
    init();

    int* mas = (int*)my_malloc(10 * sizeof(int));
    for (int i = 0; i < 10; i++)
    {
        mas[i] = i;
        printf("%d ", mas[i]);
    }
    printf("\n");

    float* mas_f = (float*)my_malloc(10 * sizeof(float));
    for (int i = 0; i < 10; i++)
    {
        mas_f[i] = (float)(i);
        printf("%f ", mas_f[i]);
    }
    printf("\n");

    mas = (int*)my_realloc(mas, 20 * sizeof(int));
    for (int i = 0; i < 20; i++)
    {
        mas[i] = i;
        printf("%d ", mas[i]);
    }
    printf("\n");

    test* new_test = (test*)my_malloc(sizeof(test) * 2);
    for (int i = 0; i < 2; i++)
    {
        new_test[i].x = i + 1;
        new_test[i].y = (i + 1) * 10;
    }
    int test_var = 0;
    for (int i = 0; i < 5; i++)
    {
        test_var += (new_test[i].x + new_test[i].y) * (new_test[i].x + new_test[i].y);
    }
    printf("%d ", test_var);

    my_free(mas);
    my_free(mas_f);
    my_free(new_test);
    total_free();
    return 0;
}