#include <iostream>
#include "mpi.h"
#include <fstream>
#include <vector>

#include <time.h>
using namespace std;

int* input(string name, int &len)
{
    ifstream in(name);
    in >> len;
    int* res = new int[len]; 
    for (int i = 0; i < len; i++)
        in >> res[i];
    return res;
}

int comparator(const void* a, const void* b)
{
    return (*(int*)a - *(int*)b);
}

void merge_split(int* res, int* input, int len, bool isMin)
{
    int* temp = new int[len];
    int a, b, c;
    if (isMin)
    {
        a = 0;
        b = 0;
        c = 0;
        while (c < len)
        {
            if ((res[a] < input[b]))
                temp[c++] = res[a++];
            else
                temp[c++] = input[b++];
        }
    }
    else
    {
        a = len - 1;
        b = len - 1;
        c = len - 1;
        while (c >= 0)
        {
            if ((res[a] >= input[b]))
                temp[c--] = res[a--];
            else
                temp[c--] = input[b--];
        }
    }
    for (int i = 0; i < len; i++)
        res[i] = temp[i];
    delete[] temp;
}

int main()
{
    string name = "../resourses/data.in";
    /*cout << "Enter file name\n";
    cin >> name;*/
    
    int number_of_curr_proc;
    int amount_of_procs;
    int block_size;
    int data_len = 0;
    int* input_data = new int[0];

    MPI_Init(NULL, NULL);
    MPI_Barrier(MPI_COMM_WORLD);
    MPI_Comm_rank(MPI_COMM_WORLD, &number_of_curr_proc);
    MPI_Comm_size(MPI_COMM_WORLD, &amount_of_procs);
    cout << "processor - " << number_of_curr_proc << endl;
    if (number_of_curr_proc == 0)
    {
        input_data = input(name, data_len);
        /*cout << data_len << endl;
        for (int i = 0; i < data_len; i++)
            cout << input_data[i] << " ";*/
        block_size = data_len / amount_of_procs;
    }
    
    MPI_Bcast(&block_size, 1, MPI_INT, 0, MPI_COMM_WORLD);

    int* data_in_proc = new int[block_size];
    int* received_data = new int[block_size];
    int* ans = new int[data_len];

    MPI_Scatter(input_data, block_size, MPI_INT, data_in_proc, block_size, MPI_INT, 0, MPI_COMM_WORLD);

    qsort(data_in_proc, block_size, sizeof(int), comparator);

    int odd_iteration_proc = number_of_curr_proc;
    int even_iteration_proc = number_of_curr_proc;

    if (number_of_curr_proc % 2 == 0)
    {
        odd_iteration_proc--;
        even_iteration_proc++;
    }
    else
    {
        odd_iteration_proc++;
        even_iteration_proc--;
    }

    MPI_Status status;
    for (int i = 0; i < amount_of_procs; i++)
        if (i % 2 == 0)
        {
            if (0 <= even_iteration_proc && even_iteration_proc < amount_of_procs)
            {
                MPI_Sendrecv(data_in_proc, block_size, MPI_INT, even_iteration_proc, 0, received_data, block_size, MPI_INT, even_iteration_proc, 0, MPI_COMM_WORLD, &status);
                if (number_of_curr_proc % 2 == 0)
                    merge_split(data_in_proc, received_data, block_size, true);
                else
                    merge_split(data_in_proc, received_data, block_size, false);
            }
        }
        else if (0 <= odd_iteration_proc && odd_iteration_proc < amount_of_procs)
            {
                MPI_Sendrecv(data_in_proc, block_size, MPI_INT, odd_iteration_proc, 0, received_data, block_size, MPI_INT, odd_iteration_proc, 0, MPI_COMM_WORLD, &status);
                if (number_of_curr_proc % 2 == 1)
                    merge_split(data_in_proc, received_data, block_size, true);
                else
                    merge_split(data_in_proc, received_data, block_size, false);
            }

    MPI_Gather(data_in_proc, block_size, MPI_INT, ans, block_size, MPI_INT, 0, MPI_COMM_WORLD);
    cout << "processor " << number_of_curr_proc << " finished";
    ofstream out("../resourses/data.out");
    if (number_of_curr_proc == 0)
    {
        for (int i = 0; i < data_len; i++)
            out << ans[i] << " ";
    }
    MPI_Finalize();
    delete[] input_data;
    delete[] data_in_proc;
    delete[] received_data;
    delete[] ans;
    return 0;
}