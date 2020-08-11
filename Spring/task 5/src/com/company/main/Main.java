package com.company.main;

import com.company.binaryTree.Node;

import java.lang.ref.WeakReference;

public class Main {

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

    public static void main(String[] args) throws Exception {
	    WeakReference<Node<String>> weakBinaryTree = new WeakReference<>(new Node<>());
		weakBinaryTree.get().addNode(10, "Hello");
	    weakBinaryTree.get().addNode(5, "from");
	    weakBinaryTree.get().addNode(4, "the outside");
	    weakBinaryTree.get().addNode(8, "At least");
	    weakBinaryTree.get().addNode(7, "I can");
	    weakBinaryTree.get().addNode(6, "say that");
	    weakBinaryTree.get().addNode(9, "I've");
	    weakBinaryTree.get().addNode(11, "tried...");

	    System.out.print(weakBinaryTree.get().findNode(9) + " ");
	    System.out.println(weakBinaryTree.get().findNode(11));

		System.gc();
		System.out.println("Now: " + dfs(weakBinaryTree.get()));

		Thread.sleep(1000);
		System.gc();
		System.out.println("After 1000 ms: " + dfs(weakBinaryTree.get()));
    }

}

