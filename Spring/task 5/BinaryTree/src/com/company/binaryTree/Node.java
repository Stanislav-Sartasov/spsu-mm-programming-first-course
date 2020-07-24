package com.company.binaryTree;

import java.lang.ref.WeakReference;

public class Node<T> {
    private int key;
    private int lifetime = 1000;
    private T value;
    private WeakReference<Node<T>> left;
    private WeakReference<Node<T>> right;

    public int getKey() {
        return key;
    }

    public T getValue() {
        return value;
    }
    
    public Node<T> getLeft() {
        if (left != null)
            return left.get();
        return null;
    }
    
    public Node<T> getRight() {
        if (right != null)
            return right.get();
        return null;
    }

    public Node() {
        value = null;
    }

    public Node(int x, T obj) {
        key = x;
        value = obj;
        left = null;
        right = null;
    }

    public Node(int x, T obj, int time) {
        key = x;
        lifetime = time;
        value = obj;
        left = null;
        right = null;
    }

    private void save(int time) {
        new Thread(() -> {
            Node<T> a = this;
            try {
                Thread.sleep(lifetime);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }).start();
    }

    public void addNode(int x, T obj) throws Exception {
        if (value == null) {
            key = x;
            value = obj;
            left = null;
            right = null;
            this.save(lifetime);
            return;
        }
        if (key > x) {
            if (left != null && left.get() != null) {
                left.get().addNode(x, obj);
            } else {
                left = new WeakReference<>(new Node<>(x, obj));
                left.get().save(lifetime);
            }
        } else if (key < x) {
            if (right != null && right.get() != null) {
                right.get().addNode(x, obj);
            } else {
                right = new WeakReference<>(new Node<>(x, obj));
                right.get().save(lifetime);
            }
        } else {
            throw new Exception("Object with key " + key + " already exists!");
        }
    }

    public T findNode(int x) {
        if (value == null) {
            return null;
        }
        if (key == x)
            return value;
        if (key > x) {
            if (left != null && left.get() != null)
                return left.get().findNode(x);
            return null;
        }
        if (right != null && right.get() != null)
            return right.get().findNode(x);
        return null;
    }

    public void deleteNode(int x) {
        this.deleteNode(x, null);
    }

    private void deleteNode(int x, WeakReference<Node<T>> parent) {
        if (value == null)
            return;
        if (key > x) {
            if (left != null && left.get() != null)
                left.get().deleteNode(x, new WeakReference<>(this));
            return;
        }
        if (key < x) {
            if (right != null && right.get() != null)
                right.get().deleteNode(x, new WeakReference<>(this));
            return;
        }

        if ((left == null || left.get() == null) && (right == null || right.get() == null)) { // нет детей
            if (parent == null || parent.get() == null) {
                return;
            }
            if (parent.get().getKey() > key) {
                parent.get().left = null;
            } else {
                parent.get().right = null;
            }
            return;
        }

        if (left == null) { // только правый ребенок
            if (parent.get().getKey() > key) {
                parent.get().left = right;
            } else {
                parent.get().right = right;
            }
            return;
        }

        if (right == null) { // только левый ребенок
            if (parent.get().getKey() > key) {
                parent.get().left = left;
            } else {
                parent.get().right = left;
            }
            return;
        }

        // два ребенка
        parent = new WeakReference<>(this);
        WeakReference<Node<T>> node = parent.get().right;
        while (node.get() != null && node.get().left != null && node.get().left.get() != null) { // найти самую левую правую вершину
            parent = node;
            node = node.get().left;
        }

        this.deleteNode(node.get().getKey(), parent);

        key = node.get().getKey();
        value = node.get().getValue();
    }

}
