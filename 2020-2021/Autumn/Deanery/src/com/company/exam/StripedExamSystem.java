package com.company.exam;

import java.util.ArrayList;
import java.util.List;

public class StripedExamSystem extends ExamSystem {

    private final Lock[] locks;

    public StripedExamSystem(int capacity) {
        super(capacity);
        locks = new Lock[capacity];
        for (int i = 0; i < capacity; i++)
            locks[i] = new Lock();
    }

    protected void resize() {
        int oldCapacity = table.length;
        for (Lock lock: locks)
            lock.lock();
        try {
            if (oldCapacity != table.length) {
                return;
            }
            int newCapacity = 2 * oldCapacity;
            List<Pair>[] oldTable = table;
            table = new List[newCapacity];
            for (int i = 0; i < newCapacity; i++)
                table[i] = new ArrayList<>();
            for (List<Pair> bucket: oldTable) {
                for (Pair x: bucket) {
                    table[Math.abs(x.hashCode()) % table.length].add(x);
                }
            }
        } finally {
            for (Lock lock: locks)
                lock.unlock();
        }
    }

    protected void acquire(Pair x) {
        locks[Math.abs(x.hashCode()) % locks.length].lock();
    }

    protected void release(Pair x) {
        locks[Math.abs(x.hashCode()) % locks.length].unlock();
    }
}
