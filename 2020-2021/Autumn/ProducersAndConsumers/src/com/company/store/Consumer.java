package com.company.store;

public class Consumer<E> extends Thread {
    private final Store<E> store;
    private Boolean running = false;

    public Consumer(Store<E> s) {
        store = s;
    }

    public void run() {
        running = true;
        while (running) {
            boolean fl = true;
            while (fl && running) {
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
        running = false;
    }
}
