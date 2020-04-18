#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#define NUMBER_COUNTS 8

int main()
{
    int coins[] = { 1, 2, 5, 10, 20, 50, 100, 200 };
    int sum = 0; int n = 0;
    char eol = 0;
    printf("The program solves the task \"English coin\"\n");
    printf("Please, enter the number\n");
    while (n == 0)
    {
        int sumscanf = scanf("%d%c", &sum, &eol);
        if (sumscanf && eol == '\n' && sum > 0)
        {
            n = 1;
            long long int** mas = (long long int**)calloc(NUMBER_COUNTS + 1, sizeof(long long int*));
            for (int i = 0; i <= NUMBER_COUNTS; i++)
            {
                mas[i] = (int*)calloc(sum + 1, sizeof(long long int));
            }

            for (int i = 0; i <= NUMBER_COUNTS; i++)
                for (int j = 0; j <= sum; j++)
                {
                    mas[i][0] = 0;
                    mas[0][j] = 1;
                }

            for (int i = 1; i < NUMBER_COUNTS; i++)
            {
                for (int j = 0; j <= sum; j++)
                {
                    if (j < coins[i])
                    {
                        mas[i][j] = mas[i - 1][j];
                    }
                    else
                    {
                        mas[i][j] = mas[i - 1][j] + mas[i][j - coins[i]];
                    }
                }
            }

            printf("result : %lld", mas[NUMBER_COUNTS - 1][sum]);
            for (int i = 0; i < NUMBER_COUNTS; i++)
            {
                free(mas[i]);
            }
            free(mas);

        }
        else
        {
            printf("Error input. Try again\n");
            scanf("%*[^\n]");
        }
    }
    return 0;
}