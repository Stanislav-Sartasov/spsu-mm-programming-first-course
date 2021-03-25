package com.company;

import com.company.exam.*;

import java.util.Random;

public class Main {

    public static void main(String[] args) {
        IExamSystem coarseExamSystem = new CoarseExamSystem(10);
        IExamSystem refinableExamSystem = new RefinableExamSystem(10);
        System.out.println("Computing...");

        long startTime = System.nanoTime();
        work(coarseExamSystem);
        long endTime   = System.nanoTime();
        long totalTime = endTime - startTime;
        System.out.println("Coarse Exam System:    " + totalTime + " ms.");

        startTime = System.nanoTime();
        work(refinableExamSystem);
        endTime   = System.nanoTime();
        totalTime = endTime - startTime;
        System.out.println("Refinable Exam System: " + totalTime + " ms.");
    }

    private static void work(IExamSystem examSystem) {
        int n = 1000;
        Thread[] users = new Thread[n];
        for (int i = 0; i < n; i++) {
            users[i] = new Thread(() -> {
                Random random = new Random();
                for (int j = 0; j < 100; j++) {
                    int student = random.nextInt(10);
                    int exam = random.nextInt(10);
                    if (j == 55) {
                        examSystem.remove(student, exam);
                    } else if (j % 10 == 5) {
                        examSystem.add(student, exam);
                    } else {
                        examSystem.contains(student, exam);
                    }
                }
            });
            users[i].start();
        }
        for (int i = 0; i < n; i++) {
            try {
                users[i].join();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

}
