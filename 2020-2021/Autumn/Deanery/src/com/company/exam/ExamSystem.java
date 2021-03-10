package com.company.exam;

import java.util.ArrayList;
import java.util.List;

public abstract class ExamSystem implements IExamSystem {
    protected List<Pair>[] table;
    protected int setSize;

    public boolean isCrowded() {
        return setSize / table.length > 4;
    }

    public ExamSystem(int capacity) {
        if (capacity <= 0)
            throw new IllegalArgumentException("Capacity should be positive.");
        setSize = 0;
        table = new ArrayList[capacity];
        for (int i = 0; i < capacity; i++) {
            table[i] = new ArrayList<>();
        }
    }

    abstract protected void acquire(Pair x);
    abstract protected void release(Pair x);
    abstract protected void resize();

    public void add(long studentId, long courseId) {
        Pair x = new Pair(studentId, courseId);
        acquire(x);
        try {
            int myBucket = Math.abs(x.hashCode() % table.length);
            if (!table[myBucket].contains(x)) {
                table[myBucket].add(x);
                setSize++;
            }
        } finally {
            release(x);
        }
        if (isCrowded())
            resize();
    }

    public void remove(long studentId, long courseId) {
        Pair x = new Pair(studentId, courseId);
        acquire(x);
        try {
            int myBucket = Math.abs(x.hashCode() % table.length);
            if (table[myBucket].contains(x)) {
                table[myBucket].remove(x);
                setSize--;
            }
        } finally {
            release(x);
        }
    }

    public boolean contains(long studentId, long courseId) {
        Pair x = new Pair(studentId, courseId);
        acquire(x);
        try {
            int myBucket = Math.abs(x.hashCode() % table.length);
            return table[myBucket].contains(x);
        } finally {
            release(x);
        }
    }
}
