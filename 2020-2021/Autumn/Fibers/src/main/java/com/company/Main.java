package com.company;

import com.company.ProcessManager.*;
import com.company.ProcessManager.Process;

public class Main {
    public static void main(String[] args) {
        int n = 7;
        Process[] tasks = new Process[n];
        for (int i = 0; i < n; i++) {
            tasks[i] = new Process();
        }
        ProcessManager.setPriority(true);
        ProcessManager.addTasks(tasks);
        ProcessManager.run();
        ProcessManager.stop();
    }
}
