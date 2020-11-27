package com.company;

import com.company.threadpool.ThreadPool;

import java.util.Random;

public class Main {

    private static void doSomething() {
        long x = new Random().nextInt();
        for (long i = 0; i < 1_000_000_000L; i++)
            x++;
        System.out.println(Thread.currentThread().getName() + " is working!");
    }

    public static void main(String[] args) {
        ThreadPool threadPool = new ThreadPool(8);

        try(threadPool) {
            for (int i = 0; i < 24; i++) {
                threadPool.enqueue(Main::doSomething);
           }
        }

    }
}
