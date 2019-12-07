//
//  main.c
//  task1.3_Углы треугольника
//
//  Created by 李泽民 on 15.10.2019.
//  Copyright © 2019 Ли Цзэминь. All rights reserved.
//

#include "stdio.h"
#include "stdlib.h"
#include "math.h"

#define PI 3.1415

int check_triangular(float a, float b, float c)
{
    if ( (a + b > c) && (a + c > b) && (b + c > a) )
        return 1;
    return 0;
}

float find_angle(float a, float b, float c)
{
    float angle = acos((a*a + b*b - c*c) / (2 * a * b));

    angle = 180 * angle / PI;

    return angle;
}

void angle_min_sec(float angle)
{
    int grad = angle;

    float frac_part = angle - grad;

    float minutes = frac_part * 60;

    float seconds = (minutes - (int) minutes) * 60;

    printf("Grad: %-5d Minutes: %-5d Seconds: %-5d\n", grad, (int) round(minutes), (int) round(seconds));
}

int main()
{
    setbuf(stdout, NULL);

    float a, b, c;

    printf("Input a, b, c: ");
    scanf("%f %f %f", &a, &b, &c);


    if (!check_triangular(a, b, c))
    {
        printf("Triangular does not exist\n");
        return 0;
    }

    printf("Triangular exists\n");

    float first_angle = find_angle(a, b, c);
    float second_angle = find_angle(b, c, a);
    float third_angle = find_angle(a, c, b);


    angle_min_sec(first_angle);
    angle_min_sec(second_angle);
    angle_min_sec(third_angle);

    return 0;
}

// a + b > c
// a + c > b
// b + c > a

// 1 / 3 = 0.(3)
// 2 / 3 = 0.(6)


