#include <stdio.h>
#include <stdlib.h>
#include <math.h>

const double eps = 1e-6;
const double PI = 3.141592653589793;

double max(double a, double b) {
    if (a > b)
        return a;
    return b;
}

int correct(char s[]) {
    int b = 1, cnt = 0;
    for (int i = 0; s[i] != '\0'; i++)
    {
        if (!(('0' <= s[i] && s[i] <= '9') || s[i] == '.' || s[i] == ','))
            b = 0;
        if (s[i] == '.' || s[i] == ',')
            cnt++;
    }
    return (b && cnt <= 1);
}

long double convert(char s[]) {
    long double a = 0;
    int k = -1;
    for (int i = 0; s[i] != '\0'; i++)
    {
        if (s[i] != '.' && s[i] != ',')
            a = a * 10 + (s[i] - '0');
        else
            k = i;
    }
    if (k == -1)
        return a;
    for (int i = k + 1; s[i] != '\0'; i++)
        a /= 10.0;
    return a;
}

int main()
{
    printf("Please enter lengths of sides of the triangle to find out angles of the triangle\n");
    double a, b, c;
    char s1[10], s2[10], s3[10];
    scanf("%s", &s1);
    scanf("%s", &s2);
    scanf("%s", &s3);
    while (!correct(s1) || !correct(s2) || !correct(s3))
    {
        printf("Please, enter correct sides of the triangle\n");
        scanf("%s", &s1);
        scanf("%s", &s2);
        scanf("%s", &s3);
    }
    a = convert(s1);
    b = convert(s2);
    c = convert(s3);
    double m = max(max(a, b), c);
    if (a + b + c - 2 * m < eps)
    {
        printf("Such triangle doesn't exist\n");
        return 0;
    }
    printf("Angles of the triangle:\n");
    double alpha = acos((a * a + b * b - c * c) / (2 * a * b)) * 180.0 / PI,
           beta = acos((c * c + b * b - a * a) / (2 * c * b)) * 180.0 / PI,
           gamma = acos((a * a + c * c - b * b) / (2 * a * c)) * 180.0 / PI;
    printf("%d %d' %d''\n", (int)alpha, ((int)(alpha * 60)) % 60, ((int)(alpha * 360)) % 60);
    printf("%d %d' %d''\n", (int)beta, ((int)(beta * 60)) % 60, ((int)(beta * 360)) % 60);
    printf("%d %d' %d''\n", (int)gamma, ((int)(gamma * 60)) % 60, ((int)(gamma * 360)) % 60);
    return 0;
}
