#include <stdio.h>
#include <math.h>

#define SIZE 999999


/*
	Calculates the didital root.
	@param m_num:	The number that needs to be calculated.
*/
int digitRoot(int m_num){
	int sum = 0;

	while (m_num > 0)
	{
		sum += m_num % 10;
		m_num /= 10;
	}

	return sum;
}


/*
	Calculates the maximum sum of digital roots.
	@param m_num:	The number whoes MDR needs to be calculated.
	@param ptr:		The pointer where the results will be stored.
*/
int mdrs(int m_num, int* ptr)
{
	int max = 0;

	int i = m_num;
	int rest = i % 2;
	int j = 2 + rest;

	while (m_num > 10) {
		m_num = digitRoot(m_num);
	}

	max = digitRoot(m_num);
	while (j < sqrt(i) + 1)
	{
		if ((i % j) == 0)
		{
			if (max < ptr[j - 2] + ptr[i / j - 2])
			{
				max = ptr[j - 2] + ptr[i / j - 2];
			}
		}
		j += rest + 1;
	}

	return max;
}


// Main program starts here.
int main(){
	int* ptr;
	ptr = (int*)malloc(SIZE * sizeof(int));


	if (ptr == NULL){
		printf("Failed creating memory.\n");
		return 0;
	}
	else{
		int sum = 0;

		for (int i = 0; i < 8; i++)
		{
			ptr[i] = i + 2;
			sum += ptr[i];
		}

		for (int i = 8; i < SIZE; i++)
		{
			ptr[i] = mdrs(i + 2, ptr);
			sum += ptr[i];
		}

		printf("%d\n", sum);
	}



	free(ptr);

	return 0;
}