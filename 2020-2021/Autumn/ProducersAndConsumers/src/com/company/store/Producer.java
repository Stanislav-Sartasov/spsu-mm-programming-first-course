package com.company.store;

public class Producer<E> extends Thread {
    private final Store<E> store;
    private final E object;
    private volatile boolean running;

    public Producer(Store<E> s, E obj) {
        store = s;
        object = obj;
        running = true;
    }

    public void run() {
        while (running) {
            store.put(object);
            try {
                sleep(1000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    public void stopProducer() {
        running = false;
    }
}
