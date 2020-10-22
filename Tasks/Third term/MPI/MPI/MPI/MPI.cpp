#include <iostream>
#include "mpi.h"
#include <fstream>
#include <vector>
#include "ParallelSort.h"

using namespace std;

int main(int argc, char* argv[])
{
    if (argc != 3)
    {
        cout << "Invalid parameters\n";
        return 0;
    }
    //string name_in = "../resourses/dat.in";
    string name_in = argv[1];
    int data_len = 0;
    int* result = parallel_sort(name_in, data_len);
    if (get_number_of_curr_proc() == 0)
    {
        //ofstream out("../resourses/dat.in");
        ofstream out(argv[2]);
        for (int i = 0; i < data_len; i++)
            out << result[i] << " ";
        out.close();
    }
    delete[] result;
    return 0;
}