package com.company.exam;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.locks.ReentrantLock;

public class CoarseExamSystem extends ExamSystem {

    private final ReentrantLock mutex;

    public CoarseExamSystem(int capacity) {
        super(capacity);
        mutex = new ReentrantLock();
    }

    protected void resize() {
        int oldCapacity = table.length;
        Pair p = new Pair(0, 0);
        acquire(p);
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
            release(p);
        }
    }

    protected void acquire(Pair x) {
        mutex.lock();
    }

    protected void release(Pair x) {
        mutex.unlock();
    }
}
