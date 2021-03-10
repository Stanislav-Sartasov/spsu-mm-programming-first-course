package com.company.exam;

class Pair {
    private long first;
    private long second;
    private static final int p = 11939;

    public long getFirst() {
        return first;
    }

    public long getSecond() {
        return second;
    }

    public Pair(long f, long s) {
        first = f;
        second = s;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Pair pair = (Pair) o;
        return first == pair.first && second == pair.second;
    }

    @Override
    public int hashCode() {
        return (int)(first * p + second);
    }
}
