package com.company.main;

import com.company.binaryTree.Node;

public class Main {

    public static void main(String[] args) throws Exception {
	    Node<String> binaryTree = new Node<>();
	    binaryTree.addNode(10, "Hello");
	    binaryTree.addNode(5, "from");
	    binaryTree.addNode(4, "the outside");
	    binaryTree.addNode(8, "At least");
	    binaryTree.addNode(7, "I can");
	    binaryTree.addNode(6, "say that");
	    binaryTree.addNode(9, "I've");
	    binaryTree.addNode(11, "tried...");

	    System.out.print(binaryTree.findNode(9) + " ");
	    System.out.println(binaryTree.findNode(11));

        binaryTree.deleteNode(11);
    }

}

