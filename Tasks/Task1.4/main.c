//
//  main.c
//  task1.4_Числа Мерсенна
//
//  Created by 李泽民 on 20.10.2019.
//  Copyright © 2019 Ли Цзэминь. All rights reserved.
//

#include "stdio.h"
#include "math.h"

int is_prime(long int n)
{
    for (int i = 2; i < n; i++)
    {
        if (n % i == 0)
            return 0;
    }
    return 1;
}

int main()
{
    int n_start = 1;
    long int curr_mersenne = 1;

    long int end = pow(2, 32) - 1;

    while (curr_mersenne <= end)
    {
        if (is_prime(curr_mersenne))
            printf("%ld\n", curr_mersenne);
        n_start++;

        curr_mersenne = pow(2, n_start) - 1;
    }

    return 0;
}

