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

    public Node(int x, T obj) {
        key = x;
        value = obj;
        left = null;
        right = null;
    }

    public void addNode(int x, T obj) throws Exception {
        if (value == null) {
            key = x;
            value = obj;
            left = null;
            right = null;
            return;
        }
        if (key > x) {
            if (left != null)
                left.addNode(x, obj);
            else
                left = new Node<>(x, obj);
        } else if (key < x) {
            if (right != null)
                right.addNode(x, obj);
            else
                right = new Node<>(x, obj);
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
            if (left != null)
                return left.findNode(x);
            else
                return null;
        }
        if (right != null)
            return right.findNode(x);
        return null;
    }

    public void deleteNode(int x) {
        this.deleteNode(x, null);
    }

    private void deleteNode(int x, Node<T> parent) {
        if (value == null)
            return;
        if (key > x) {
            if (left != null)
                this.left.deleteNode(x, this);
            return;
        }
        if (key < x) {
            if (right != null)
                this.right.deleteNode(x, this);
            return;
        }

        if (left == null && right == null) { // нет детей
            if (parent == null) {
                return;
            }
            if (parent.getKey() > key)
                parent.left = null;
            else {
                parent.right = null;
            }
            return;
        }

        if (left == null) { // только правый ребенок
            if (parent.getKey() > key)
                parent.left = right;
            else {
                parent.right = right;
            }
            return;
        }

        if (right == null) { // только левый ребенок
            if (parent.getKey() > key)
                parent.left = left;
            else {
                parent.right = left;
            }
            return;
        }

        // два ребенка
        parent = this;
        Node<T> node = parent.right;
        while (node.left != null) { // найти самую левую правую вершину
            parent = node;
            node = node.left;
        }

        this.deleteNode(node.getKey(), parent);

        key = node.getKey();
        value = node.getValue();
    }

}
