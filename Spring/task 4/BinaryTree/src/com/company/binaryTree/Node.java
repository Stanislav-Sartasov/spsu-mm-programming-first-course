package com.company.binaryTree;

public class Node<T> {
    private int key;
    private T value;
    private Node<T> left;
    private Node<T> right;

    public int getKey() {
        return key;
    }
    
    public T getValue() {
        return value;
    }
    
    public Node<T> getLeft() {
        return left;
    }
    
    public Node<T> getRight() {
        return right;
    }

    public Node() {
        value = null;
    }


    public Node(int x, T obj, Node<T> l, Node<T> r) {
        key = x;
        value = obj;
        left = l;
        right = r;
    }

    public void addNode(int x, T obj) throws Exception {
        if (value == null) {
            key = x;
            value = obj;
            left = new Node<>();
            right = new Node<>();
            return;
        }
        if (key > x) {
            left.addNode(x, obj);
        } else if (key < x) {
            right.addNode(x, obj);
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
        if (key > x)
            return left.findNode(x);
        return right.findNode(x);
    }

    public void deleteNode(int x) {
        this.deleteNode(x, new Node<>());
    }

    private void deleteNode(int x, Node<T> parent) {
        if (value == null)
            return;
        if (key > x) {
            this.left.deleteNode(x, this);
            return;
        }
        if (key < x) {
            this.right.deleteNode(x, this);
             return;
        }

        if (left.getValue() == null && right.getValue() == null) { // нет детей
            if (parent.getValue() == null) {
                return;
            }
            if (parent.getKey() > key)
                parent.left = new Node<>();
            else {
                parent.right = new Node<>();
            }
            return;
        }

        if (left.getValue() == null) { // только правый ребенок
            if (parent.getKey() > key)
                parent.left = right;
            else {
                parent.right = right;
            }
            return;
        }

        if (right.getValue() == null) { // только левый ребенок
            if (parent.getKey() > key)
                parent.left = left;
            else {
                parent.right = left;
            }
            return;
        }
        // два ребенка
        parent = new Node<>(key, value, left, right);
        Node<T> node = parent.right;
        while (node.left.getValue() != null) { // найти самую левую правую вершину
            parent = node;
            node = node.left;
        }

        this.deleteNode(node.getKey(), parent);

        key = node.getKey();
        value = node.getValue();
    }

}
