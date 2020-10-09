#include <iostream>
#include "mpi.h"
#include <fstream>
#include <vector>
#include "ParallelSort.h"

using namespace std;

int main()
{
    string name_in = "../resourses/dat.in";
    /*cout << "Enter file name\n";
    cin >> name;*/
    int data_len = 0;
    int* result = parallel_sort(name_in, data_len);
    if (get_number_of_curr_proc() == 0)
    {
        ofstream out("../resourses/data.out");
        for (int i = 0; i < data_len; i++)
            out << result[i] << " ";
        out.close();
    }
    delete[] result;
    return 0;
}