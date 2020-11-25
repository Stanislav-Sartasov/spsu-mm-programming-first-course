package com.company.store;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.util.ArrayList;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;

class StoreTest {
    private Store<Integer> store;
    private List<Integer> expected;

    @BeforeEach
    void init() {
        store = new Store<>();
        expected = new ArrayList<>();
    }

    @Test
    void put() {
        store.put(5);
        expected.add(5);
        assertEquals(expected, store.getObjects());
    }

    @Test
    void getExisting() {
        store.put(5);
        expected.add(5);
        assertFalse(store.get());

        expected.remove(0);
        assertEquals(expected, store.getObjects());
    }

    @Test
    void getNonExisting() {
        assertTrue(store.get());
    }
}