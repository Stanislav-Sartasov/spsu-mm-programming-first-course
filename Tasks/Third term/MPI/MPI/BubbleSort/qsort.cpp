#include <iostream>
#include <fstream>
#include <vector>

#include <time.h>
#include <string>
using namespace std;

int* Input(string name, int& len)
{
    ifstream in(name);
    in >> len;
    int* res = new int[len];
    for (int i = 0; i < len; i++)
        in >> res[i];
    return res;
}

int Comparator(const void* a, const void* b)
{
    return (*(int*)a - *(int*)b);
}

int main()
{
    cout << "Enter file name\n";
    string name;
    cin >> name;

    clock_t begin = clock();
    int data_len;
    int* input_data = Input(name, data_len);
    /*cout << data_len << endl;
    for (int i = 0; i < data_len; i++)
        cout << input_data[i] << " ";*/
    for (int i = 0; i < data_len; i++)
    {
        if (input_data[i] < 0)
            cout << i << " ";
    }
    qsort(input_data, data_len, sizeof(int), Comparator);
    clock_t end = clock();
    double time_spent = (double)(end - begin) / CLOCKS_PER_SEC;
    printf("\nTime of compilation = %f", time_spent);

    
    return 0;
}