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
    create(a);
    while (1)
    {
        char q[MAX_LEN];
        scanf("%s", &q);
        int fl = 0;
        if (!strcmp(q, "insert"))
        {
            char k[MAX_LEN];
            char v[MAX_LEN];
            scanf("%s", k);
            scanf("%s", v);
            if (insert(a, k, v))
                printf("Action completed successfully.\n");
            else
                printf("The value for this key already exists.\n");
            fl = 1;
        }
        if (!strcmp(q, "find"))
        {
            char k[MAX_LEN];
            scanf("%s", &k);
            Node *ans = find(a, k);
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
        if (!strcmp(q, "delete"))
        {
            char k[MAX_LEN];
            scanf("%s", &k);
            if (del(a, k))
                printf("Action completed successfully.\n");
            else
                printf("The value for this key doesn't exist.\n");
            fl = 1;
        }
        if (!strcmp(q, "finish"))
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
    return 0;
}
