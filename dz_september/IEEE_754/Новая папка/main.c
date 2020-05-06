#include <stdio.h>
#include <string.h>

int main()
{
    char name[80] = "Makar", surname[80] = "Pelogeiko", fathern[80] = "Andreevich";
    int length=strlen(name) * strlen(surname) * strlen(fathern);

    printf("%d",length);
    return 0;
}