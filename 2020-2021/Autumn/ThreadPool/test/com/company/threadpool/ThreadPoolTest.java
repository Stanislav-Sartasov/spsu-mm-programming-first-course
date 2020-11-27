package com.company.threadpool;

import org.junit.jupiter.api.Test;

import java.util.Random;

import static org.junit.jupiter.api.Assertions.*;

class ThreadPoolTest {

    private static void doSomething() {
        long x = new Random().nextInt();
        for (long i = 0; i < 1_000_000_000L; i++)
            x++;
        System.out.println(Thread.currentThread().getName() + " is working!");
    }

    @Test
    void test(){
        ThreadPool threadPool = new ThreadPool(8);

        assertEquals(8, threadPool.getThreads().size());
        assertTrue(threadPool.getWorkQueue().isEmpty());

        for (int i = 0; i < 24; i++) {
            threadPool.enqueue(ThreadPoolTest::doSomething);
        }

        assertFalse(threadPool.getWorkQueue().isEmpty());

        threadPool.close();

        assertTrue(threadPool.getWorkQueue().isEmpty());
    }
}