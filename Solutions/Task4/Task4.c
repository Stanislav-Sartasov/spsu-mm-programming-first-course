#include <stdio.h>

char std_prime(int in)
{
	if ((in != 2 && !(in % 2)) ||  in == 1)
		return 0;
	for (int i = 3; i * i <= in; i += 2)
		if (!(in % i))
			return 0;
	return 1;
}

int main()
{
	int Mars = 0;
	for (int i = 1; i <= 31; i++)
	{
		Mars = (Mars << 1) | 1;
		if (std_prime(i))		//if 2^i − 1 is prime, then i is prime
		{
			char flag = 1;		//j that divides 2^i − 1 must be 1 + 2*i*k where k - natural
			for (int j = 2 * i + 1; (long long)j * (long long)j <= Mars; j += 2 * i)
			{
				if (!(Mars % j))
				{
					flag = 0;
					break;
				}
			}
			if (flag)
				printf("%d\n", Mars);
		}
	}

	return 0;
}