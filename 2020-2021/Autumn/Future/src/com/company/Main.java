package com.company;

import com.company.vector.CascadeModel;
import com.company.vector.ModifiedCascadeModel;

import java.util.Random;

public class Main {

    public static void main(String[] args) {
        int[] a = new int[1_000_000];
        for (int i = 0; i < 1_000_000; i++)
            a[i] = new Random().nextInt(100);

        long startTime;
        long endTime;

        CascadeModel cascadeModel = new CascadeModel();
        startTime = System.currentTimeMillis();
        System.out.print("Answer: " + cascadeModel.computeLength(a));
        endTime = System.currentTimeMillis();
        System.out.println(". Calculated for " + (endTime - startTime));

        ModifiedCascadeModel modifiedCascadeModel = new ModifiedCascadeModel();
        startTime = System.currentTimeMillis();
        System.out.print("Answer: " + modifiedCascadeModel.computeLength(a));
        endTime = System.currentTimeMillis();
        System.out.println(". Calculated for " + (endTime - startTime));
    }
}
