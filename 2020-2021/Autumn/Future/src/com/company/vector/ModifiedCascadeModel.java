package com.company.vector;

import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

import static java.lang.Integer.min;

public class ModifiedCascadeModel implements IVectorLengthComputer {

    private int[] a;

    private long linearCompute(int l, int r) {
        long ans = 0;
        for (int i = l; i < min(r, a.length); i++)
            ans += a[i] * a[i];
        return ans;
    }

    private long computeSquareOfLength() throws ExecutionException, InterruptedException {
        int cores = Runtime.getRuntime().availableProcessors();
        ExecutorService executor = Executors.newFixedThreadPool(cores);
        int n = (a.length + cores - 1) / cores;
        Future<Long>[] results = new Future[cores];
        for (int i = 0; i < cores; i++) {
            int finalI = i;
           results[i] = executor.submit(() -> linearCompute(finalI * n, (finalI + 1) * n));
        }

        while (results.length > 1) {
            Future<Long>[] finalResults = results;
            Future<Long>[] newResults = new Future[(results.length + 1) / 2];
            for (int i = 0; i < results.length; i += 2) {
                int finalI = i;
                if (i + 1 < results.length) {
                    newResults[i / 2] = executor.submit(() -> finalResults[finalI].get() + finalResults[finalI + 1].get());
                }
                else
                    newResults[i / 2] = executor.submit(() -> finalResults[finalI].get());
            }
            results = newResults;
        }
        executor.shutdown();
        return results[0].get();
    }

    @Override
    public int computeLength(int[] a) {
        this.a = a;
        try {
            return (int) Math.sqrt(computeSquareOfLength());
        } catch (ExecutionException | InterruptedException e) {
            e.printStackTrace();
        }
        return 0;
    }
}
