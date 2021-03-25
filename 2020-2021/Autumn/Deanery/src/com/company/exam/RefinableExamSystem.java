package com.company.exam;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.atomic.AtomicMarkableReference;

public class RefinableExamSystem extends ExamSystem {

    AtomicMarkableReference<Thread> owner;
    volatile Lock[] locks;

    public RefinableExamSystem(int capacity) {
        super(capacity);
        locks = new Lock[capacity];
        for (int i = 0; i < capacity; i++) {
            locks[i] = new Lock();
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

                for (Lock lock : locks) {
                    lock.lock();
                    lock.unlock();
                }

                List<Pair>[] oldTable = table;
                table = new ArrayList[newCapacity];
                for (int i = 0; i < newCapacity; i++) {
                    table[i] = new ArrayList<>();
                }

                locks = new Lock[newCapacity];
                for (int i = 0; i < newCapacity; i++) {
                    locks[i] = new Lock();
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

            Lock[] oldLocks = locks;
            Lock oldLock = oldLocks[Math.abs(x.hashCode() % oldLocks.length)];
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
