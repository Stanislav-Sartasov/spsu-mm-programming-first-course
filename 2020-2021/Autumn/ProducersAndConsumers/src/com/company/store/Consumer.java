package com.company.store;

import java.util.concurrent.atomic.AtomicBoolean;

public class Consumer<E> extends Thread {
    private final Store<E> store;
    private final AtomicBoolean running = new AtomicBoolean(false);

    public Consumer(Store<E> s) {
        store = s;
    }

    public void run() {
        running.set(true);
        while (running.get()) {
            boolean fl = true;
            while (fl && running.get()) {
                fl = store.get();
                try {
                    sleep(10);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
            try {
                sleep(1000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    public void stopConsumer() {
        running.set(false);
    }
}
