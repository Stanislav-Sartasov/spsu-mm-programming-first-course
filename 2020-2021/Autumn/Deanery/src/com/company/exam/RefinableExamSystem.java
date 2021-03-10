package com.company.exam;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.atomic.AtomicMarkableReference;
import java.util.concurrent.locks.ReentrantLock;

public class RefinableExamSystem extends ExamSystem {

    AtomicMarkableReference<Thread> owner;
    volatile ReentrantLock[] locks;

    public RefinableExamSystem(int capacity) {
        super(capacity);
        locks = new ReentrantLock[capacity];
        for (int i = 0; i < capacity; i++) {
            locks[i] = new ReentrantLock();
        }
        owner = new AtomicMarkableReference<>(null, false);
    }

    protected void resize() {
        int oldCapacity = table.length;
        int newCapacity = 2 * oldCapacity;

        Thread me = Thread.currentThread();
        if (owner.compareAndSet(null, me, false, true)) {
            try {
                if (table.length != oldCapacity) {
                    return;
                }

                for (ReentrantLock lock : locks) {
                    lock.lock();
                    lock.unlock();
                }

                List<Pair>[] oldTable = table;
                table = new ArrayList[newCapacity];
                for (int i = 0; i < newCapacity; i++) {
                    table[i] = new ArrayList<>();
                }

                locks = new ReentrantLock[newCapacity];
                for (int i = 0; i < newCapacity; i++) {
                    locks[i] = new ReentrantLock();
                }

                for (List<Pair> bucket: oldTable) {
                    for(Pair x: bucket) {
                        table[Math.abs(x.hashCode()) % table.length].add(x);
                    }
                }
            } finally {
                owner.set(null, false);
            }
        }
    }

    protected void acquire(Pair x) {
        boolean[] mark = new boolean[1];
        mark[0] = true;
        Thread me = Thread.currentThread();
        Thread who;
        while (true) {
            do {
                who = owner.get(mark);
            } while (mark[0] && who != me);

            ReentrantLock[] oldLocks = locks;
            ReentrantLock oldLock = oldLocks[Math.abs(x.hashCode() % oldLocks.length)];
            oldLock.lock();
            who = owner.get(mark);
            if ((!mark[0] || who == me) && locks == oldLocks) {
                return;
            } else {
                oldLock.unlock();
            }
        }
    }

    protected void release(Pair x) {
        locks[Math.abs(x.hashCode() % locks.length)].unlock();
    }
}
