package com.company.exam;

import java.util.concurrent.atomic.AtomicReference;

public class Lock {
    private final AtomicReference<QNode> tail = new AtomicReference<>(null);
    private final ThreadLocal<QNode> myNode = ThreadLocal.withInitial(QNode::new);

    public void lock() {
        QNode qnode = myNode.get();
        QNode pred = tail.getAndSet(qnode);
        if (pred != null) {
            qnode.locked = true;
            pred.next = qnode;

            while (qnode.locked) {
                Thread.yield();
            }
        }
    }

    public void unlock() {
        QNode qnode = myNode.get();
        if (qnode.next == null) {
            if (tail.compareAndExchange(qnode, null) == qnode)
                return;

            while (qnode.next == null) {
                Thread.yield();
            }
        }
        qnode.next.locked = false;
        qnode.next = null;
    }

    static class QNode {
        public volatile boolean locked = false;
        public volatile QNode next = null;
    }
}
