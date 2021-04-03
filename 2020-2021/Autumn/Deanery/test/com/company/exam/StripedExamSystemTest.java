package com.company.exam;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

class StripedExamSystemTest {
    StripedExamSystem examSystem;

    @BeforeEach
    void setup() {
        examSystem = new StripedExamSystem(10);
    }

    @Test
    void add() {
        examSystem.add(1, 1);
        assertEquals(1, examSystem.setSize);

        examSystem.add(1, 1);
        assertEquals(1, examSystem.setSize);

        for (int i = 0; i < 100; i++)
            examSystem.add(i + 2, i);
        assertEquals(101, examSystem.setSize);
    }

    @Test
    void remove() {
        Pair pair = new Pair(1, 1);
        examSystem.table[Math.abs(pair.hashCode()) % examSystem.table.length].add(pair);
        examSystem.setSize += 1;

        examSystem.remove(1, 1);
        assertEquals(0, examSystem.setSize);

        examSystem.remove(0, 1);
        assertEquals(0, examSystem.setSize);
    }

    @Test
    void contains() {
        Pair pair = new Pair(1, 1);
        examSystem.table[Math.abs(pair.hashCode()) % examSystem.table.length].add(pair);
        examSystem.setSize += 1;

        assertTrue(examSystem.contains(1, 1));
        assertFalse(examSystem.contains(1, 0));
        assertFalse(examSystem.contains(0, 1));
        assertFalse(examSystem.contains(0, 0));
    }

    @Test
    void resize() {
        Pair[] pairs = new Pair[5];
        for (int i = 0; i < 5; i++) {
            pairs[i] = new Pair(i, 1);
            examSystem.table[Math.abs(pairs[i].hashCode()) % examSystem.table.length].add(pairs[i]);
            examSystem.setSize += 1;
        }

        examSystem.resize();
        assertEquals(examSystem.setSize, 5);
        for (int i = 0; i < 5; i++) {
            int bucket = Math.abs(pairs[i].hashCode()) % examSystem.table.length;
            assertTrue(examSystem.table[bucket].contains(pairs[i]));
        }
    }
}