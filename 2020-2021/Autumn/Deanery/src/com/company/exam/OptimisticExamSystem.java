package com.company.exam;

public class OptimisticExamSystem implements IExamSystem {
    private final Node<Pair> head = new Node<>(new Pair(Long.MIN_VALUE, Long.MIN_VALUE));
    private final Node<Pair> tail = new Node<>(new Pair(Long.MAX_VALUE, Long.MAX_VALUE));

    public int setSize() {
        Node<Pair> curr = head.next;
        int ans = 0;
        while (curr != tail) {
            curr = curr.next;
            ans++;
        }
        return ans;
    }

    private static class Node<T> {
        public Node<T> next;
        public T key;
        public Lock lock;

        public Node(T pair) {
            key = pair;
            lock = new Lock();
        }
    }

    public OptimisticExamSystem() {
        head.next = tail;
    }

    private boolean validate(Node<Pair> pred, Node<Pair> curr) {
        Node<Pair> node = head;
        while (node.key.compareTo(pred.key) <= 0) {
            if (node == pred)
                return pred.next == curr;
            node = node.next;
        }
        return false;
    }

    public void add(long studentId, long course) {
        Pair item = new Pair(studentId, course);
        while (true) {
            Node<Pair> pred = head;
            Node<Pair> curr = head.next;
            while (curr.key.compareTo(item) < 0) {
                pred = curr;
                curr = curr.next;
            }
            pred.lock.lock();

            try {
                curr.lock.lock();
                try {
                    if (validate(pred, curr)) {
                        if (!curr.key.equals(item)) {
                            Node<Pair> node = new Node<>(item);
                            node.next = curr;
                            pred.next = node;
                        }
                        return; // не получилось
                    }
                } finally {
                    pred.lock.unlock();
                }
            } finally {
                curr.lock.unlock();
            }
        }
    }

    public void remove(long studentId, long course) {
        Pair item = new Pair(studentId, course);
        while (true) {
            Node<Pair> pred = head;
            Node<Pair> curr = head.next;
            while (curr.key.compareTo(item) < 0) {
                pred = curr;
                curr = curr.next;
            }
            pred.lock.lock();
            curr.lock.lock();

            try {
                if (validate(pred, curr)) {
                    if (curr.key.equals(item)) {
                        pred.next = curr.next;
                    }
                    return;
                }
            } finally {
                curr.lock.unlock();
                pred.lock.unlock();
            }
        }
    }


    public boolean contains(long studentId, long course) {
        Pair item = new Pair(studentId, course);
        while (true) {
            Node<Pair> pred = head;
            Node<Pair> curr = head.next;
            while (curr.key.compareTo(item) < 0) {
                pred = curr;
                curr = curr.next;
            }
            pred.lock.lock();

            try {
                curr.lock.lock();
                try {
                    if (validate(pred, curr)) {
                        return curr.key.equals(item);
                    }
                } finally {
                    curr.lock.unlock();
                }
            } finally {
                pred.lock.unlock();
            }
        }
    }
}
