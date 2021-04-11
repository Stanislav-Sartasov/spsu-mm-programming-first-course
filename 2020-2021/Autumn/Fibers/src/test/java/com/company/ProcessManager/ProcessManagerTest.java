package com.company.ProcessManager;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

class ProcessManagerTest {
    private Process[] tasks;

    @BeforeEach
    void setup() {
        int n = 4;
        tasks = new Process[n];
        for (int i = 0; i < n; i++) {
            tasks[i] = new Process();
            ProcessManager.addTask(tasks[i]);
        }
    }

    @Test
    void addTasks() {
        assertEquals(tasks.length, ProcessManager.getAllFibers().values().size());
    }

    @Test
    void runWithoutPriority() {
        ProcessManager.run();
        ProcessManager.stop();
        assertEquals(0, ProcessManager.getAllFibers().values().size());
    }

    @Test
    void runWithPriority() {
        ProcessManager.setPriority(true);
        ProcessManager.run();
        ProcessManager.stop();
        assertEquals(0, ProcessManager.getAllFibers().values().size());
    }

}