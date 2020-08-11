package com.company.binaryTree;

import com.company.binaryTree.Node;
import org.junit.Assert;
import org.junit.jupiter.api.Test;

import java.util.Objects;

class NodeTest {

    static String dfs(Node<String> v) {
        if (v != null && v.getValue() != null) {
            String ans = v.getValue();
            String left = dfs(v.getLeft());
            String right = dfs(v.getRight());
            if (!left.equals(""))
                ans += " " + left;
            if (!right.equals(""))
                ans += " " + right;
            return ans;
        }
        return "";
    }

    @Test
    void addNode() throws Exception {
        Node<String> binaryTree = new Node<>();
        binaryTree.addNode(10, "Hello");
        binaryTree.addNode(5, "from");
        binaryTree.addNode(4, "the outside!");
        binaryTree.addNode(8, "At least");
        binaryTree.addNode(7, "I can");
        binaryTree.addNode(6, "say that");
        binaryTree.addNode(9, "I've");
        binaryTree.addNode(11, "tried...");
        try {
            binaryTree.addNode(11, "not tried...");
        }catch (Exception e) {
            if (!Objects.equals(e.getMessage(), "Object with key 11 already exists!"))
                e.printStackTrace();
        }
        Assert.assertEquals(dfs(binaryTree), "Hello from the outside! At least I can say that I've tried...");
    }

    @Test
    void findNode() throws Exception {
        Node<Integer> binaryTree = new Node<>();
        binaryTree.addNode(1, 1);
        binaryTree.addNode(2, 4);
        binaryTree.addNode(3, 9);
        binaryTree.addNode(4, 16);
        binaryTree.addNode(5, 25);
        binaryTree.addNode(6, 36);
        for (int i = 1; i < 6; i++) {
            if (binaryTree.findNode(i) != i * i)
                throw new Exception("Can't find.");
        }
        if (binaryTree.findNode(7) != null)
            throw new Exception("Found smth that wasn't to be found.");
    }

    @Test
    void deleteNode() throws Exception {
        Node<String> binaryTree = new Node<>();
        binaryTree.addNode(10, "1");
        binaryTree.addNode(5, "2");
        binaryTree.addNode(3, "3");
        binaryTree.addNode(4, "4");
        binaryTree.addNode(8, "5");
        binaryTree.addNode(7, "6");
        binaryTree.addNode(6, "7");
        binaryTree.addNode(9, "8");
        binaryTree.addNode(11, "9");

        // нечего удалять
        binaryTree.deleteNode(12);
        Assert.assertEquals(dfs(binaryTree), "1 2 3 4 5 6 7 8 9");

        // нет детей
        binaryTree.deleteNode(9);
        Assert.assertEquals(dfs(binaryTree), "1 2 3 4 5 6 7 9");
        binaryTree.addNode(9, "8");

        // только левый ребенок
        binaryTree.deleteNode(7);
        Assert.assertEquals(dfs(binaryTree), "1 2 3 4 5 7 8 9");
        binaryTree.addNode(7, "6");

        // только правый ребенок
        binaryTree.deleteNode(3);
        Assert.assertEquals(dfs(binaryTree), "1 2 4 5 7 6 8 9");
        binaryTree.addNode(3, "3");

        // два ребенка
        binaryTree.deleteNode(5);
        Assert.assertEquals(dfs(binaryTree), "1 7 4 3 5 6 8 9");
    }
}