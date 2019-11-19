#pragma once
#include <stdio.h>
#include "functionToGo.h"


char stringOfAgainInput[80] = "try again";

double saveInDouble()
{
    double a = 0, delim = 1;
    char c = '\0';
    int first = 1, sign = 1, goodSet = 1, less1 = 0;

    do
    {
        a = 0; delim = 1;
        c = '\0';
        first = 1; sign = 1; goodSet = 1; less1 = 0;
        while ((c != ' ') && (c != '\n') && goodSet)
        {
            scanf("%c", &c);
            if (first)
            {
                first = 0;
                if (c == '-')
                {
                    sign = -1; continue;
                }
                if ((c >= '0') && (c <= '9'))
                {
                    a = c - '0'; continue;
                }

                goodSet = 0;
                break;
            }

            if (c == '.')
            {
                less1 = 1; continue;
            }

            if ((c >= '0') && (c <= '9') && !less1)
            {
                a = a * 10 + c - '0';
                continue;
            }

            if ((c >= '0') && (c <= '9') && less1)
            {
                delim = delim * 10;
                a = a + (double)(( c - '0' ))/delim;
                continue;
            }
            if ((c != ' ') && (c != '\n'))
            {
                goodSet = 0;  break;
            }
        }
        a = a * sign;
        if (!goodSet)
        {
            printf("%s\n", stringOfAgainInput);
            c = '\0';
            while (c != '\n')
                scanf("%c",&c);
        }
    }
    while (goodSet != 1);
    return a;
}

long long saveInInt()
{
    long long a = 0;
    char c = '\0';
    int first = 1, sign = 1, goodSet = 1;

    do
    {
        a = 0;
        c = '\0';
        first = 1; sign = 1; goodSet = 1;
        while ((c != ' ') && (c != '\n') && goodSet)
        {
            scanf("%c", &c);
            if (first)
            {
                first=0;
                if (c == '-')
                {
                        sign = -1; continue;
                }
                if ((c >= '0') && (c <= '9'))
                {
                    a = c - '0'; continue;
                }

                goodSet = 0;
                break;
            }

            if ((c >= '0') && (c <= '9'))
            {
                a = a * 10 + c - '0';
                continue;
            }

            if ((c != ' ') && (c != '\n'))
            {
                goodSet = 0;  break;
            }
        }
        a = a * sign;
        if (!goodSet)
        {
            printf("%s\n",stringOfAgainInput);
            c = '\0';
            while (c != '\n')
                scanf("%c",&c);
        }
    }
    while (goodSet != 1);
    return a;
}

void changeAgainInputText(const char *str)
{
    for (register int i = 0; i < 80; ++i)
        stringOfAgainInput[i] = str[i];
}
