#define _CRT_SECURE_NO_WARNINGS
#define N 9
#define M 12
#include <stdio.h>
#include <map.h>
#include <time.h>


char used[N * M];
map_long_t map;


long long go(int x, int y)
{
	if (x == N)
	{
		x = 0;
		y++;
	}
	if (y == M)
		return 1;

	long long result = 0;

	if (used[y * N + x] == '1')
	{
		if (x + 1 < N && y + 1 < M && used[y * N + x + 1] == '0' && used[(y + 1) * N + x] == '0' && used[(y + 1) * N + x + 1] == '0')
		{
			used[(y + 1) * N + x + 1] = '1'; used[(y + 1) * N + x] = '1'; used[y * N + x + 1] = '1';
			result += go(x + 1, y);
			used[(y + 1) * N + x + 1] = '0'; used[(y + 1) * N + x] = '0'; used[y * N + x + 1] = '0';
			return result + go(x + 1, y);
		}
		else
			return go(x + 1, y);
	}

	long long* val = map_get(&map, used);
	if (val)
		return *val;

	if (x + 1 < N && y + 1 < M)
	{
		if (used[(y + 1) * N + x] == '0' && used[y * N + x + 1] == '0')
		{
			used[(y + 1) * N + x] = '1'; used[y * N + x + 1] = '1'; used[y * N + x] = '1';
			result += go(x + 1, y);
			used[(y + 1) * N + x] = '0'; used[y * N + x + 1] = '0'; used[y * N + x] = '0';
		}

		if (used[y * N + x + 1] == '0' && used[(y + 1) * N + x + 1] == '0')
		{
			used[(y + 1) * N + x + 1] = '1'; used[y * N + x + 1] = '1'; used[y * N + x] = '1';
			result += go(x + 1, y);
			used[(y + 1) * N + x + 1] = '0'; used[y * N + x + 1] = '0'; used[y * N + x] = '0';
		}

		if (used[(y + 1) * N + x] == '0' && used[(y + 1) * N + x + 1] == '0')
		{
			used[(y + 1) * N + x + 1] = '1'; used[(y + 1) * N + x] = '1'; used[y * N + x] = '1';
			result += go(x + 1, y);
			used[(y + 1) * N + x + 1] = '0'; used[(y + 1) * N + x] = '0'; used[y * N + x] = '0';
		}
	}

	if (y + 2 < M && used[(y + 1) * N + x] == '0' && used[(y + 2) * N + x] == '0')
	{
		used[(y + 1) * N + x] = '1'; used[(y + 2) * N + x] = '1'; used[y * N + x] = '1';
		result += go(x + 1, y);
		used[(y + 1) * N + x] = '0'; used[(y + 2) * N + x] = '0'; used[y * N + x] = '0';
	}

	if (x + 2 < N && used[y * N + x + 1] == '0' && used[y * N + x + 2] == '0')
	{
		used[y * N + x + 1] = '1'; used[y * N + x + 2] = '1'; used[y * N + x] = '1';
		result += go(x + 1, y);
		used[y * N + x + 1] = '0'; used[y * N + x + 2] = '0'; used[y * N + x] = '0';
	}

	map_set(&map, used, result);

	return result;
}

int main()
{
	clock_t begin = clock();
	map_init(&map);

	for (int i = 0; i < M; i++)
		for (int j = 0; j < N; j++)
			used[i * N + j] = '0';

	printf("%lld\n", go(0, 0));
	clock_t end = clock();
	double time_spent = (double)(end - begin) / CLOCKS_PER_SEC;
	printf("%f\n", time_spent);
	return 0;
}