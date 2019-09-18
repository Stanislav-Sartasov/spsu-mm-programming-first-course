#include <stdio.h>

int main()
{
	int Mars = 0;			//for Mersenne numbers
	for (int i = 1; i <= 31; i++)
	{
		Mars = (Mars << 1) | 1; //Marsenne number for i because Marsenne number = 111...111 in binary
		char flag = 1;			//flag for division finding
		for (int j = i + 1; j < Mars / 2; j += i)	//finding division of Marsenne number
		{											//Marsenne number division = 1(mod i) by theoreme
			if (Mars % j == 0)
			{
				flag = 0;
				break;
			}
		}
		if (flag)
			printf("%d\n", Mars);
	}

	return 0;
}