#include <stdio.h>

int isPrime(int number)
{
	if (number % 2 == 0)
		return number == 2;

	int d = 3;

	while (d * d <= number)
	{
		if (number % d == 0)
			return 0;
		d += 2;
	}
	return 1;
}

int digitalRoot(int number)
{
	return 1 + ((number - 1) % 9);
}

int solve(int number)
{
	int factor[19] = { 0 };
	int count = 0;

	while (number != 1)
	{
		for (int i = 2; i * i <= number; i++)
		{
			if (number % i == 0)
			{
				factor[count] = i;
				count++;
				number /= i;
				break;
			}
		}
		if (isPrime(number))
		{
			factor[count] = number;
			count++;
			number = 1;
		}
	}

	int maxNow = -1, maxNowI, maxNowJ, maxNowK;

	while (maxNow != 0)
	{
		maxNow = 0;
		maxNowI = -1;
		maxNowJ = -1;
		maxNowK = -1;

		for (int i = 0; i < 19; i++)
		{
			if (factor[i] == 0)
				continue;

			for (int j = i + 1; j < 19; j++)
			{
				if (factor[j] == 0)
					continue;

				if (digitalRoot(factor[i] * factor[j]) > digitalRoot(factor[i]) + digitalRoot(factor[j]) &&
					digitalRoot(factor[i] * factor[j]) - (digitalRoot(factor[i]) + digitalRoot(factor[j])) >= maxNow)
				{
					maxNow = digitalRoot(factor[i] * factor[j]) - (digitalRoot(factor[i]) + digitalRoot(factor[j]));
					maxNowI = i;
					maxNowJ = j;
					maxNowK = -1;
				}

				for (int k = j + 1; k < 19; k++)
				{
					if (factor[k] == 0)
						continue;
					if (digitalRoot(factor[i] * factor[j] * factor[k]) > digitalRoot(factor[i]) + digitalRoot(factor[j]) + digitalRoot(factor[k]) &&
						digitalRoot(factor[i] * factor[j] * factor[k]) - (digitalRoot(factor[i]) + digitalRoot(factor[j]) + digitalRoot(factor[k])) > maxNow)
					{
						maxNow = digitalRoot(factor[i] * factor[j] * factor[k]) - (digitalRoot(factor[i]) + digitalRoot(factor[j]) + digitalRoot(factor[k]));
						maxNowI = i;
						maxNowJ = j;
						maxNowK = k;
					}
				}
			}
		}

		if (maxNow != 0)
		{
			if (maxNowK != -1)
			{
				factor[maxNowI] = factor[maxNowI] * factor[maxNowJ] * factor[maxNowK];
				factor[maxNowJ] = 0;
				factor[maxNowK] = 0;
			}
			else
			{
				factor[maxNowI] = factor[maxNowI] * factor[maxNowJ];
				factor[maxNowJ] = 0;
			}
		}
	}

	int answer = 0;

	for (int i = 0; i < 19; i++)
		answer += digitalRoot(factor[i]);

	return answer;
}


int main()
{
	int answer = 0;

	for (int number = 2; number < 1000000; number++)
		answer += solve(number);

	printf("%d", answer);
	return 0;
}