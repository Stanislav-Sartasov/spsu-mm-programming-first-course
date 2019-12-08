#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>
#include <math.h>
#include <string.h>
#include "myHashTable.h"

int main()
{
    printf("To add the key and value, enter: insert *key* *value*\n");
    printf("To delete the key, enter: delete *key*\n");
    printf("To find the value by key, enter: find *key*\n");
    printf("To finish the program, enter: finish\n");
    Node *a[LEN];
    for (int i = 0; i < LEN; i++)
    {
        a[i] = (Node*) malloc(sizeof(Node));
        char e[] = {'\0'};
        strcpy(a[i]->key, e);
        strcpy(a[i]->value, e);
        a[i]->next = NULL;
    }
    P = get_p();
    MOD = get_mod();
    char s1[] = {'i', 'n', 's', 'e', 'r', 't', '\0'},
         s2[] = {'f', 'i', 'n', 'd', '\0'},
         s3[] = {'d', 'e', 'l', 'e', 't', 'e', '\0'},
         s4[] = {'f', 'i', 'n', 'i', 's', 'h', '\0'};
    while (1)
    {
        char q[MAX_LEN];
        scanf("%s", &q);
        int fl = 0;
        if (!strcmp(q, s1))
        {
            char k[MAX_LEN];
            char v[MAX_LEN];
            scanf("%s", &k);
            scanf("%s", &v);
            if (insert(&a, k, v))
                printf("Action completed successfully.\n");
            else
                printf("The value for this key already exists.\n");
            fl = 1;
        }
        if (!strcmp(q, s2))
        {
            char k[MAX_LEN];
            scanf("%s", &k);
            Node *ans = find(&a, k);
            if (ans == NULL)
            {
                printf("Not found.\n");
            }
            else
            {
                printf("The value: %s.\n", ans->value);
            }
            fl = 1;
        }
        if (!strcmp(q, s3))
        {
            char k[MAX_LEN];
            scanf("%s", &k);
            if (del(&a, k))
                printf("Action completed successfully.\n");
            else
                printf("The value for this key doesn't exist.\n");
            fl = 1;
        }
        if (!strcmp(q, s4))
        {
            for (int i = 0; i < LEN; i++)
                free(a[i]);
            return 0;
        }
        if (!fl)
        {
            printf("Please enter correct request!\n");
        }
    }
}
