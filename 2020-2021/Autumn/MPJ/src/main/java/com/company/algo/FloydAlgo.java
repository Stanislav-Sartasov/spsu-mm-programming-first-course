package com.company.algo;

import mpi.MPI;

import java.io.IOException;
import java.io.Writer;

import static java.lang.Integer.min;

public class FloydAlgo {
    private final Writer writer;
    
    public FloydAlgo(Writer writer) {
        this.writer = writer;
    }

    private void printAnswer(int[][] a) {
        for (int i = 0; i < a.length; i++) {
            for (int j = 0; j < a[i].length; j++) {
                try {
                    writer.write(a[i][j] + " ");
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
            try {
                writer.write("\n");
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }

    private void linearFloyd(int[][] a) {
        int n = a.length;
        for (int k = 0; k < n; k++) {
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    a[i][j] = min(a[i][j], a[i][k] + a[k][j]);
                }
            }
        }
        printAnswer(a);
    }
    
    public void start(String[] args, int[][] a) {
        MPI.Init(args);
        int myRank = MPI.COMM_WORLD.Rank();
        int size = MPI.COMM_WORLD.Size();

        if (size == 1) {
            linearFloyd(a);
            MPI.Finalize();
            return;
        }

        int n = a.length;

        if (myRank == 0) {
            int[] number = new int[size];

            for(int i = 1; i < size; i++) {
                number[i] = n / (size - 1);
            }
            number[size - 1] += n % (size - 1);

            for (int i = 1; i < size; i++) {
                MPI.COMM_WORLD.Send(number, i, 1, MPI.INT, i, 1);
            }

            for(int k = 0; k < n; k++) {
                for(int p = 1; p < size; p++) {
                    MPI.COMM_WORLD.Isend(a[k], 0, n, MPI.INT, p, 2);
                }


                int num = 0;
                for(int i = 1; i < size; i++) {
                    for(int j = 0; j < number[i]; j++) {
                        MPI.COMM_WORLD.Isend(a[num], 0, n, MPI.INT, i, 3);
                        num++;
                    }
                }


                num = 0;
                for(int i = 1; i < size; i++) {
                    for(int j = 0; j < number[i]; j++) {
                        MPI.COMM_WORLD.Recv(a[num], 0, n, MPI.INT, i, 4);
                        num++;
                    }
                }
            }

            printAnswer(a);

        }
        else {
            int[] m = new int[1];
            MPI.COMM_WORLD.Recv(m, 0, 1, MPI.INT, 0, 1);
            int[] row = new int[n];
            int[][] arr = new int[m[0]][n];

            for (int k = 0; k < n; k++) {
                MPI.COMM_WORLD.Recv(row, 0, n, MPI.INT, 0, 2);
                for (int j = 0; j < m[0]; j++) {
                    MPI.COMM_WORLD.Recv(arr[j], 0, n, MPI.INT, 0, 3);
                }

                for (int i = 0; i < m[0]; i++) {
                    for (int j = 0; j < n; j++) {
                        arr[i][j] = min(arr[i][j], arr[i][k] + row[j]);
                    }
                }

                for (int i = 0; i < m[0]; i++) {
                    MPI.COMM_WORLD.Send(arr[i], 0, n, MPI.INT, 0, 4);
                }
            }
        }
        MPI.Finalize();
    }
}
