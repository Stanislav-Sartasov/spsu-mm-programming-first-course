package com.company.vector;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.util.Random;

import static org.junit.jupiter.api.Assertions.*;

class VectorLengthComputerTest {

    private int[] a;

    @BeforeEach
    void init() {
        Random random = new Random();
        int n = 1_000_000;
        a = new int[n];
        for (int i = 0; i < n; i++) {
            a[i] = random.nextInt(100);
        }
    }

    int compute() {
        long ans = 0;
        for (int j : a) {
            ans += j * j;
        }
        return (int) Math.sqrt(ans);
    }

    @Test
    void testCascadeModel() {
        CascadeModel cascadeModel = new CascadeModel();
        assertEquals(compute(), cascadeModel.computeLength(a));
    }

    @Test
    void testModifiedCascadeModel() {
        ModifiedCascadeModel modifiedCascadeModel = new ModifiedCascadeModel();
        assertEquals(compute(), modifiedCascadeModel.computeLength(a));
    }

}