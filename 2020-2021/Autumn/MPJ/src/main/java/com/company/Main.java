package com.company;

import com.company.algo.QuickSort;

import java.io.*;
import java.util.Scanner;

public class Main {

    public static void main(String[] args) throws IOException {
        int n;
        int[] arr;
        Scanner scanner = new Scanner(new File("src/main/resources/input.txt"));
        n = scanner.nextInt();
        arr = new int[n];
        for (int i = 0; i < n; i++) {
            arr[i] = scanner.nextInt();
        }

        Writer writer = new FileWriter("src/main/resources/output.txt");
        QuickSort quickSort  = new QuickSort(writer);
        quickSort.start(args, arr);
        writer.close();
    }
}
