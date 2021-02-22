package com.company.algo;

import org.junit.jupiter.api.Test;

import java.io.*;
import java.util.Arrays;
import java.util.Scanner;

import static org.junit.jupiter.api.Assertions.*;

class QuickSortTest {

    @Test
    void compareResults() throws IOException {
        int n;
        int[] arr;
        Scanner scanner = new Scanner(new File("src/main/resources/input.txt"));
        n = scanner.nextInt();
        arr = new int[n];
        for (int i = 0; i < n; i++) {
            arr[i] = scanner.nextInt();
        }
        Arrays.sort(arr);
        scanner = new Scanner(new File("src/main/resources/output.txt"));
        for (int i = 0; i < n; i++) {
            assertTrue(scanner.hasNextInt());
            assertEquals(arr[i], scanner.nextInt());
        }
    }
}