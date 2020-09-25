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

	long long* dp = (long long*)malloc(8 * n * sizeof(long long));

	for (int i = 0; i < 8; i++)
		dp[i] = 1;
	for (int i = 0; i < n; i++)
		dp[8 * i] = 1;

	for (int i = 1; i < n; i++)
		for (int j = 1; j < 8; j++)
			if (i + 1 <= coins[j])
				dp[8 * i + j] = dp[8 * i + j - 1] + (i + 1 == coins[j] ? 1 : 0);
			else
				dp[8 * i + j] = dp[8 * i + j - 1] + dp[8 * (i - coins[j]) + j];

	printf("%lld", dp[8 * n - 1]);

	free(dp);
	return 0;
}