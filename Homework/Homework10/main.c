#include <stdio.h>

#define MAX_PENSION         (1005)
#define MAX_COINS           (1005)
#define NUM_ENGLISH_COINS   (9)

unsigned long long dp[NUM_ENGLISH_COINS][MAX_PENSION][MAX_COINS];

void calculate(int pension) 
{

    int coins[NUM_ENGLISH_COINS] = {0, 1, 2, 5, 10, 20, 50, 100, 200};

    dp[0][0][0] = 1;

    for(int i = 1; i < NUM_ENGLISH_COINS; ++i) 
	{
        for(int amount = 0; amount <= pension; ++amount) 
		{
            for(int j = 0; j <= pension; ++j) 
			{
                for(int k = 0; k <= j; ++k) 
				{
                    unsigned long long useAmt = 1LL * k * coins[i];
                    if(useAmt > amount)
                        break;
                    unsigned long long remAmt = amount - useAmt;
                    // add number of ways, if we use k coins[i]
                    dp[i][amount][j] += dp[i-1][remAmt][j - k];
                }
            }
        }
    }
}

unsigned long long getSolution(int pension) 
{

    unsigned long long count = 0;
    for(int i = 1; i <= pension; ++i)
        count += dp[NUM_ENGLISH_COINS - 1][pension][i];
    return count;
}

int readNumber(int maxDigits, int* ret) 
{
    int digits = 0;
    int c = 0;
    (*ret) = 0;

    while ((c = getchar()) != '\n') 
	{
        if (!(c >= '0' && c <= '9') || (++digits > maxDigits)) 
		{
            while (getchar() != '\n');
            return 1;
        }
        (*ret) = (*ret) * 10 + (c - '0');
    }

    return !digits;
}

int main() 
{
    const char* description = "This program find's the number of ways an amount"
                              " can be selected using denomination's of \n"
                              "1, 2, 5, 10, 20, 50, 100, 200 coins\n"
                              "\tinput:  integer (< 1005)\n"
                              "\toutput: number of ways\n\n";

    printf("%s", description);

    printf("enter a positive integer < 1005: ");
    int pension = 0;
    while (1) 
	{
        if (!readNumber(4, &pension) && (pension < MAX_PENSION))
            break;
        printf("enter a positive integer < 1005: ");
    }

    calculate(pension);
    printf("number of ways: %lld\n", getSolution(pension));

    return 0;
}
