package com.company.ProcessManager;

import java.util.*;

import static java.lang.Math.max;

public class ProcessManager {
    private static Fiber curFiber;
    private static int curPrior = 0;
    private static Map<Fiber, Process> allFibers = new HashMap<>();
    private static final Random random = new Random();
    private static boolean isRunning = false;
    private static boolean isPriority = false;

    public static boolean isRunning() {
        return isRunning;
    }
    public static boolean isPriority() {
        return isPriority;
    }

    public static Map<Fiber, Process> getAllFibers() {
        return allFibers;
    }

    public static void setPriority(boolean f) {
        if (!isRunning())
            isPriority = f;
        else
            System.out.println("You can't set priority while process manager is running");
    }

    public static void addTasks(Process[] processes) {
        for (Process p: processes) {
            Fiber fiber = new Fiber(() -> {
                try {
                    p.run();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            });
            allFibers.put(fiber, p);
        }
    }

    public static void run() {
        if (isRunning || allFibers.size() == 0)
            return;
        isRunning = true;
        processManagerSwitch(false);
    }

    public static void processManagerSwitch(boolean isFinished) {
        if (isFinished) {
            System.out.println("Fiber " + curFiber.getId() + " finished");
        }
        curFiber = getNextFiber();
        if (allFibers.get(curFiber).isFinished()) {
            System.out.println("All tasks done. Switching to primary fiber");
        }

        String info = "Fiber " + curFiber.getId() + " with priority ";
        System.out.println(info + allFibers.get(curFiber).getPriority() + " is running");
        Fiber.fiberSwitch(curFiber.getId());
    }

    private static Fiber getNextFiber() {
        if (!isPriority) {
            return getRandomFiber(allFibers);
        }
        int nextPrior = curPrior;
        Map<Fiber, Process> nextFibers = new HashMap<>();
        for (Fiber fiber: allFibers.keySet()) {
            Process pr = allFibers.get(fiber);
            if (!pr.isFinished() && fiber != curFiber) {
                if (pr.getPriority() > nextPrior) {
                    nextPrior = pr.getPriority();
                    nextFibers.clear();
                    nextFibers.put(fiber, pr);
                } else if (pr.getPriority() == nextPrior) {
                    nextFibers.put(fiber, pr);
                } else if (nextFibers.isEmpty()) {
                    nextPrior = pr.getPriority();
                    nextFibers.put(fiber, pr);
                }
            }
        }
        curPrior = nextPrior;
        return getRandomFiber(nextFibers);
    }

    private static Fiber getRandomFiber(Map<Fiber, Process> fibers) {
        int size = (int) fibers.values().stream().filter(x -> !x.isFinished()).count();
        if (size == 0) {
            return Fiber.getPrimaryFiber();
        }
        int index = random.nextInt(max(1, size));              // переполнение
        for (Fiber fiber : fibers.keySet()) {
            if (!fibers.get(fiber).isFinished()) {
                if (index == 0)
                    return fiber;
                index--;
            }
        }
        return Fiber.getPrimaryFiber();
    }

    public static void stop() {
        for (Fiber fiber: allFibers.keySet()) {
            if (!fiber.isPrimary())
                fiber.delete();
        }
        curFiber = null;
        allFibers = new HashMap<>();
        isRunning = false;
        isPriority = false;
        curPrior = 0;
    }
}
