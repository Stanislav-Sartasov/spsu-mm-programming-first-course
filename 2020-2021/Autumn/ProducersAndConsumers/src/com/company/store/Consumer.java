package com.company.store;

public class Consumer<E> extends Thread {
    private final Store<E> store;
    private volatile boolean running;

    public Consumer(Store<E> s) {
        store = s;
        running = true;
    }

    public void run() {
        while (running) {
            store.get();
            try {
                sleep(1000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    public void stopConsumer() {
        running = false;
    }
}
