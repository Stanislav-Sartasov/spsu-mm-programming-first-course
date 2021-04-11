package com.company;

import com.company.ProcessManager.*;
import com.company.ProcessManager.Process;

public class Main {
    public static void main(String[] args) {
        Object lock = new Object();

        Process pr1 = new Process();
        pr1.setPriority(3);
        pr1.setResource(lock);
        ProcessManager.addTask(pr1);

        Process pr2 = new Process();
        pr2.setPriority(10);
        pr2.setResource(lock);
        ProcessManager.addTask(pr2);

        Process pr3 = new Process();
        pr3.setPriority(7);
        ProcessManager.addTask(pr3);

        Process pr4 = new Process();
        pr4.setPriority(5);
        pr4.setResource(lock);
        ProcessManager.addTask(pr4);

        Process pr5 = new Process();
        pr5.setPriority(2);
        ProcessManager.addTask(pr5);

        ProcessManager.setPriority(true);
        System.out.println("Info: Fibers with priority 3, 5, 10 have a common critical section\n");

        ProcessManager.run();
        ProcessManager.stop();
    }
}
