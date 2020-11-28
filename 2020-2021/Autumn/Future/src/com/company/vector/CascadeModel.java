package com.company.vector;

import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

public class CascadeModel implements IVectorLengthComputer {

    @Override
    public int computeLength(int[] a) {
        double[] arr = new double[a.length];
        for (int i = 0; i < a.length; i++)
            arr[i] = a[i];
        return (int) computeLength(arr);
    }

    private double computeLength(double[] a) {
        if (a.length == 0) {
            return 0;
        }
        if (a.length == 1) {
            return a[0];
        }
        int cores = Runtime.getRuntime().availableProcessors();
        ExecutorService executor = Executors.newFixedThreadPool(cores);

        double[] newA = new double[(a.length + 1) / 2];
        Future<Double>[] futures = new Future[a.length];
        for (int i = 0; i < a.length - a.length % 2; i++) {
            int finalI = i;
            futures[i] = executor.submit(() -> a[finalI] * a[finalI]);
        }
        for (int i = 0; i < a.length; i += 2) {
            try {
                if (i + 1 < futures.length)
                    newA[i / 2] = Math.sqrt(futures[i].get() + futures[i + 1].get());
                else
                    newA[i / 2] = a[i];
            } catch (InterruptedException | ExecutionException e) {
                e.printStackTrace();
            }
        }
        executor.shutdown();
        return computeLength(newA);
    }
}
