package com.company.algo;

import mpi.MPI;
import mpi.Request;

import java.io.IOException;
import java.io.Writer;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Random;

public class QuickSort {
    private final Writer writer;

    public QuickSort(Writer writer) {
        this.writer = writer;
    }

    static class Pair<L,R> {
        private final L l;
        private final R r;
        public Pair(L l, R r){
            this.l = l;
            this.r = r;
        }
        public L getL(){ return l; }
        public R getR(){ return r; }
    }


    private int[] toArray(List<Integer> l) {
        int[] array = new int[l.size()];

        for (int i = 0; i < l.size(); i++) {
            array[i] = l.get(i);
        }
        return array;
    }

    private void printAnswer(int[] a) {
        for (int j : a) {
            try {
                writer.write(j + " ");
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }

    private void printAnswer(List<Integer>[] blocks) {
        for (List<Integer> block : blocks) {
            for (Integer elem : block) {
                try {
                    writer.write(elem + " ");
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }
    }

    private int log2(int n) {
        int ans = 0;
        double m = n;
        while (m > 0) {
            m /= 2;
            ans++;
        }
        return ans;
    }
    
    public void start(String[] args, int[] a) {
        MPI.Init(args);
        int myRank = MPI.COMM_WORLD.Rank();
        int size = MPI.COMM_WORLD.Size();

        if (size == 1) {
            Arrays.sort(a);
            printAnswer(a);
            MPI.Finalize();
            return;
        }

        int n = a.length;
        int k = 2 * (size - 1);
        int m = (n + k - 1) / k;

        if (myRank == 0) {
            List<Integer>[] blocks = new ArrayList[k];
            for (int i = 0; i < k; i++) {
                blocks[i] = new ArrayList<>();
                for (int j = 0; i * m + j < a.length && j < m; j++) {
                    blocks[i].add(a[i * m + j]);
                }
            }

            int dest = 1;
            Request[] requests = new Request[2 * k];
            int ir = 0;
            for (int i = 0; i < k; i++) {
                requests[ir++] = MPI.COMM_WORLD.Isend(new int[]{blocks[i].size()}, 0, 1, MPI.INT, dest, 5);
                requests[ir++] = MPI.COMM_WORLD.Isend(toArray(blocks[i]), 0, blocks[i].size(), MPI.INT, dest, 6);
                if (i % 2 == 1) dest++;
            }

            for (Request request : requests) {
                request.Wait();
            }

            dest = 1;
            for (int i = 0; i < k; i++) {
                int[] recv = new int[blocks[i].size()];
                MPI.COMM_WORLD.Recv(recv, 0, recv.length, MPI.INT, dest, 7);
                blocks[i] = new ArrayList<>(0);
                for (int value : recv)
                    blocks[i].add(value);
                if (i % 2 == 1) dest++;
            }

            Random r = new Random();
            for (int iteration = log2(k) - 1; iteration >= 0; iteration--) {
                dest = 1;
                requests = new Request[3 * (size - 1)];
                Pair<Integer, Integer>[] d = new Pair[size];
                ir = 0;
                int sep = r.nextInt();
                for (int first = 0; first < k; first++) {
                    if ((first & (1 << iteration)) != 0) {
                        int second = first - (1 << iteration);
                        d[dest] = new Pair<>(first, second);
                        requests[ir++] = MPI.COMM_WORLD.Isend(new int[]{blocks[first].size(), blocks[second].size(), sep},
                                                            0, 3, MPI.INT, dest, 0);
                        requests[ir++] = MPI.COMM_WORLD.Isend(toArray(blocks[first]), 0, blocks[first].size(), MPI.INT, dest, 1);
                        requests[ir++] = MPI.COMM_WORLD.Isend(toArray(blocks[second]), 0, blocks[second].size(), MPI.INT, dest, 2);
                        dest++;
                    }
                }
                for (int i = dest; i < size; i++) {
                    requests[ir++] = MPI.COMM_WORLD.Isend(new int[]{0, 0}, 0, 2, MPI.INT, i, 0);
                    requests[ir++] = MPI.COMM_WORLD.Isend(new int[0], 0, 0, MPI.INT, i, 1);
                    requests[ir++] = MPI.COMM_WORLD.Isend(new int[0], 0, 0, MPI.INT, i, 2);
                    d[i] = null;
                }

                for (Request request : requests) {
                    request.Wait();
                }

                for (int p = 1; p < size; p++) {
                    int[] sz = new int[2];
                    MPI.COMM_WORLD.Recv(sz, 0, 2, MPI.INT, p, 3);
                    int[] recv1 = new int[sz[0]];
                    int[] recv2 = new int[sz[1]];
                    MPI.COMM_WORLD.Recv(recv1, 0, sz[0], MPI.INT, p, 4);
                    MPI.COMM_WORLD.Recv(recv2, 0, sz[1], MPI.INT, p, 5);
                    if (d[p] == null)
                        continue;
                    blocks[d[p].getL()] = new ArrayList<>(0);
                    for (int i = 0; i < sz[0]; i++)
                        blocks[d[p].getL()].add(recv1[i]);
                    blocks[d[p].getR()] = new ArrayList<>(0);
                    for (int i = 0; i < sz[1]; i++)
                        blocks[d[p].getR()].add(recv2[i]);
                }
            }
            printAnswer(blocks);
        }
        else {
            int[] sz = new int[1];
            for (int i = 0; i < 2; i++) {
                MPI.COMM_WORLD.Recv(sz, 0, 1, MPI.INT, 0, 5);
                int[] arr = new int[sz[0]];
                MPI.COMM_WORLD.Recv(arr, 0, sz[0], MPI.INT, 0, 6);
                Arrays.sort(arr);
                MPI.COMM_WORLD.Isend(arr, 0, sz[0], MPI.INT, 0, 7);
            }
            sz = new int[3];
            for (int iteration = log2(k) - 1; iteration >= 0; iteration--) {
                MPI.COMM_WORLD.Recv(sz, 0, 3, MPI.INT, 0, 0);
                int[] first = new int[sz[0]];
                int[] second = new int[sz[1]];
                MPI.COMM_WORLD.Recv(first, 0, sz[0], MPI.INT, 0, 1);
                MPI.COMM_WORLD.Recv(second, 0, sz[1], MPI.INT, 0, 2);

                List<Integer> less = new ArrayList<>();
                List<Integer> more = new ArrayList<>();

                int i = 0;
                int j = 0;
                while (i < sz[0] || j < sz[1]) {
                    if ((i < sz[0] && j < sz[1] && first[i] <= second[j])
                            || (i < sz[0] && j == sz[1])) {
                        if (first[i] <= sz[2]) {
                            less.add(first[i]);
                        } else {
                            more.add(first[i]);
                        }
                        i++;
                    } else if ((i < sz[0] && j < sz[1] && first[i] > second[j])
                            || (j < sz[1] && i == sz[0])) {
                        if (second[j] <= sz[2]) {
                            less.add(second[j]);
                        } else {
                            more.add(second[j]);
                        }
                        j++;
                    }
                }
                MPI.COMM_WORLD.Send(new int[]{more.size(), less.size()}, 0, 2, MPI.INT, 0, 3);
                MPI.COMM_WORLD.Send(toArray(more), 0, more.size(), MPI.INT, 0, 4);
                MPI.COMM_WORLD.Send(toArray(less), 0, less.size(), MPI.INT, 0, 5);
            }
        }
        MPI.Finalize();
    }
}
