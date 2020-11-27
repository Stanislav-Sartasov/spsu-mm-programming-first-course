package com.company.threadpool;

import java.util.ArrayList;
import java.util.List;
import java.util.Queue;
import java.util.concurrent.ConcurrentLinkedQueue;

public class ThreadPool implements AutoCloseable {
    private final Queue<Runnable> workQueue = new ConcurrentLinkedQueue<>();
    private final List<Thread> threads;
    private volatile boolean finished = false;

    public List<Thread> getThreads() {
        return threads;
    }

    public Queue<Runnable> getWorkQueue() {
        return workQueue;
    }

    public ThreadPool(int n) {
        threads = new ArrayList<>();
        for (int i = 0; i < n; i++) {
            Thread newTask = new Thread(new Task());
            threads.add(newTask);
            newTask.start();
        }
    }

    public void enqueue(Runnable command) {
        if (!finished) {
            workQueue.offer(command);
        }
    }

    @Override
    public void close() {
        finished = true;
        for (Thread thread : threads) {
            try {
                thread.join();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    private final class Task implements Runnable {

        @Override
        public void run() {
            while (true) {
                Runnable nextTask = workQueue.poll();
                if (nextTask != null) {
                    nextTask.run();
                } else if (finished) {
                    return;
                }
            }
        }

    }
}