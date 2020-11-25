package com.company;

import com.company.store.*;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

public class Main {

    public static void main(String[] args) throws IOException {
	    Store<Integer> store = new Store<>();
	    Producer<Integer>[] p = new Producer[5];
	    Consumer<Integer>[] c = new Consumer[10];
	    for (int i = 0; i < 5; i++) {
            p[i] = new Producer<>(store, i + 1);
        }
	    for (int i = 0; i < 10; i++) {
            c[i] = new Consumer<>(store);
        }
	    store.set(p, c);
        BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
	    String s = reader.readLine();
        store.stop();

    }
}
