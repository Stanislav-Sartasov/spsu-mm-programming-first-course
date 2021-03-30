#include "myMalloc.h"
#define endl printf("\n")

int main()
{
	int *mas1 = myMalloc(10 * sizeof(int));
	for (int i = 0; i < 10; i++)
	{
		mas1[i] = 1;
		printf("%d", mas1[i]);
	}
	endl;
	char *mas2 = myMalloc(10 * sizeof(char));
	for (int i = 0; i < 10; i++)
	{
		mas2[i] = 'a';
		printf("%c", mas2[i]);
	}
	endl;

	endl;

	for (int i = 0; i < 10; i++) printf("%d", mas1[i]);
	endl;
	for (int i = 0; i < 10; i++)printf("%c", mas2[i]);

	endl; endl;
	float *mas3 = myMalloc(5 * sizeof(float));
	for (int i = 0; i < 5; i++)
	{
		mas3[i] = 1.1 + i;
		printf("%f%c", mas3[i], ' ');
	}

	myFree(mas2);

	mas1 = myRealloc(mas1, 15 * sizeof(int));
	mas3 = myRealloc(mas3, 10 * sizeof(float));
	endl;

	for (int i = 10; i < 15; i++) mas1[i] = 1;
	for (int i = 0; i < 15; i++) printf("%d", mas1[i]);
	endl; endl;

	for (int i = 5; i < 10; i++) mas3[i] = 1.1 + i;
	for (int i = 0; i < 10; i++) printf("%f%c", mas3[i], ' ');
	endl;

	system("pause");
}