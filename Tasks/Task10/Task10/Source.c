#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>

int check()
{
	int c, flag = 1;
	while ((c = getchar()) != '\n' && c != EOF)
		if (c != '\n' && c != ' ' && c != EOF)
			flag = 0;
	return flag;
}

void input(int* x)
{
	int result = scanf("%d", x);
	int flag = check();

	while ((flag == 0) || (result - 1) || (*x < 1))
	{
		printf("Invalid Input.Try Again.\n");
		result = scanf("%d", x);
		flag = check();
	}

}

int main()
{
	int n;
	input(&n);

	int coins[] = { 1, 2, 5, 10, 20, 50, 100, 200 };

	long long* dp = (long long*)malloc(8 * (n + 1) * sizeof(long long));

	for (int i = 0; i < 8; i++)
		dp[i * n] = 1;
	for (int i = 0; i <= n; i++)
		dp[i] = 1;

	for (int i = 1; i < 8; i++)
		for (int w = 1; w <= n; w++)
			if (w < coins[i])
				dp[i* n + w] = dp[(i - 1) * n + w];
			else
				dp[i * n + w] = dp[(i - 1) * n + w] + dp[i * n + w - coins[i]];

	for (int i = 1; i < 8; i++)
		if (n < coins[i])
		{
			printf("%lld", dp[i * n]);
			return 0;
		}

	printf("%lld", dp[8 * n]);
	free(dp);
	return 0;
}