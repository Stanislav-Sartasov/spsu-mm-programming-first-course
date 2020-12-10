package com.company.store;

import java.util.ArrayList;
import java.util.List;

import static java.lang.Thread.currentThread;

public class Store<E> {
    Producer<E>[] producers;
    Consumer<E>[] consumers;
    private final List<E> objects;
    private volatile boolean running;

    public List<E> getObjects() {
        synchronized (objects) {
            return objects;
        }
    }

    public Store() {
        producers = new Producer[0];
        consumers = new Consumer[0];
        objects = new ArrayList<>();
        running = true;
    }

    public void get() {
        synchronized (objects) {
            while (objects.isEmpty() && running) {
                try {
                    objects.wait();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
            if (running) {
                System.out.printf("[ %s ] - Get an object %s.%n", currentThread().getName(), objects.get(0));
                objects.remove(0);
            }
        }
    }

    public void put(E obj) {
        synchronized (objects) {
            objects.add(obj);
            objects.notify();
            System.out.printf("[ %s ] + Put an object %s.%n", currentThread().getName(), obj);
        }
    }

    public void set(Producer<E>[] producers, Consumer<E>[] consumers) {
        for (Producer<E> p: producers)
                p.start();
        for (Consumer<E> c: consumers)
                c.start();
        this.producers = producers;
        this.consumers = consumers;
    }

    public void stop() {
        synchronized (objects) {
            running = false;
            objects.notifyAll();
        }
        for (Producer<E> p: producers)
            p.stopProducer();
        for (Consumer<E> c: consumers)
            c.stopConsumer();
    }
}
