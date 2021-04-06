package com.company.exam;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

class OptimisticExamSystemTest {
    OptimisticExamSystem examSystem;

    @BeforeEach
    void setup() {
        examSystem = new OptimisticExamSystem();
    }

    @Test
    void add() {
        examSystem.add(1, 1);
        assertTrue(examSystem.contains(1, 1));

        examSystem.add(1, 1);
        assertEquals(1, examSystem.setSize());

        for (int i = 0; i < 100; i++)
            examSystem.add(i + 2, i);
        assertEquals(101, examSystem.setSize());
    }

    @Test
    void remove() {
        examSystem.add(1, 1);

        examSystem.remove(1, 1);
        assertEquals(0, examSystem.setSize());

        examSystem.remove(0, 1);
        assertEquals(0, examSystem.setSize());
    }

    @Test
    void contains() {
        examSystem.add(1, 1);

        assertTrue(examSystem.contains(1, 1));
        assertFalse(examSystem.contains(1, 0));
        assertFalse(examSystem.contains(0, 1));
        assertFalse(examSystem.contains(0, 0));
    }
}