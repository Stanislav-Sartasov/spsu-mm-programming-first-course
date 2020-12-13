package com.company;

import com.company.algo.FloydAlgo;

import java.io.*;
import java.util.Scanner;

public class Main {

    public static void main(String[] args) throws IOException {
        int n;
        int[][] w;
        Scanner scanner = new Scanner(new File("src/main/resources/input.txt"));
        n = scanner.nextInt();
        w = new int[n][n];
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                w[i][j] = 100_000;
                if (i == j)
                    w[i][j] = 0;
            }
        }
        int x, y, m;
        while (scanner.hasNext()) {
            x = scanner.nextInt();
            y = scanner.nextInt();
            m = scanner.nextInt();
            w[x][y] = m;
        }

        Writer writer = new FileWriter("src/main/resources/output.txt");
        FloydAlgo floydAlgo = new FloydAlgo(writer);
        floydAlgo.start(args, w);
        writer.close();
    }
}
