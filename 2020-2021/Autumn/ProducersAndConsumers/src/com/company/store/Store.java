package com.company.store;

import java.util.ArrayList;
import java.util.List;

import static java.lang.Thread.currentThread;

public class Store<E> {
    Producer<E>[] producers;
    Consumer<E>[] consumers;
    private final List<E> objects;

    public List<E> getObjects() {
        synchronized (objects) {
            return objects;
        }
    }

    public Store() {
        objects = new ArrayList<>();
    }

    public boolean get() {
        synchronized (objects) {
            if (objects.isEmpty())
                return true;
            System.out.printf("[ %s ] - Get an object %s.%n", currentThread().getName(), objects.get(0));
            objects.remove(0);
            return false;
        }
    }

    public void put(E obj) {
        synchronized (objects) {
            objects.add(obj);
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
        for (Producer<E> p: producers)
            p.stopProducer();
        for (Consumer<E> c: consumers)
            c.stopConsumer();
    }
}
