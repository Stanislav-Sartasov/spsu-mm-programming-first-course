#include <stdio.h>
#include <math.h>

int readDoubleStdin(double* ret) 
{
    int parts[2] = {0, 0};
    int sizes[2] = {1, 1};
    int index = 0;
    int c = 0;

    while((c = getchar()) != '\n') 
	{
        if ( (!(c >= '0' && c <= '9') && c != '.') || (c == '.' && (++index) == 2)) 
		{
            while (getchar() != '\n');
            return 1;
        }

        if (c == '.')
            continue;
        parts[index] = parts[index] * 10 + (c - '0');
        sizes[index] *= 10;
    }

    (*ret) = (double)parts[0] + ((double)parts[1] / (double) sizes[1]);
    return (sizes[0] == 1 && sizes[1] == 1);
}

void readDouble(const char* prompt, double* num) 
{
    do {
        printf("%s", prompt);
    } while (readDoubleStdin(num));
}

int isTriangle(double a, double b, double c) 
{
    if ((a + b > c) && (a + c > b) && (b + c > a))
        return 1;
    return 0;
}

void printAngles(double a, double b, double c) 
{
    double cos[3];
    cos[0] = (b * b + c * c - a * a) / (2.0 * b * c);
    cos[1] = (c * c + a * a - b * b) / (2.0 * c * a);
    cos[2] = (a * a + b * b - c * c) / (2.0 * a * b);

    double radians[3];
    double degrees[3];
    double minutes[3];
    double seconds[3];

    for (int i = 0; i < 3; i++) 
	{
        radians[i] = acos(cos[i]);
        degrees[i] = radians[i] * 180.0 / acos(-1.0);
        minutes[i] = (degrees[i] - (int)degrees[i]) * 60;
        seconds[i] = (minutes[i] - (int)minutes[i]) * 60;
        seconds[i] = floor(seconds[i] + 0.5f);
    }

    printf("The angles in dms: \n");
    for (int i = 0; i < 3; i++) 
	{
        printf("\t%d degree, %d minute, %d second\n", (int)degrees[i], (int)minutes[i], (int)seconds[i]);
    }
}

int main() 
{

    const char description[] = "This programs ask for 3 integer/floating point numbers\n"
                               "and checks if it can form a non-degenerate triangle\n"
                               "with those sides. If yes, it prints the angles in dms format\n\n";
    printf("%s", description);

    double a, b, c;
    readDouble("please enter 1st +ve floating num (example: 3.14): ", &a);
    readDouble("please enter 2nd +ve floating num (example: 3.14): ", &b);
    readDouble("please enter 3rd +ve floating num (example: 3.14): ", &c);
    printf("\n");

    if(isTriangle(a, b, c)) 
	{
        printf("yes, (%f, %f, %f) can form a non-degenerate triangle\n",
                a, b, c);
        printAngles(a, b, c);
    } else
	{
        printf("(%f, %f, %f) forms a degenerate triangle\n", a, b, c);
    }
	getchar(); getchar(); getchar(); getchar();
}