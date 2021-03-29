import java.util.HashMap;
import java.util.Map;
import java.util.Random;

import static java.lang.Math.max;

public class ProcessManager {
    private static Fiber curFiber;
    private static final Map<Fiber, Process> activeFibers = new HashMap<>();
    private static final Random random = new Random();
    private static boolean isRunning = false;

    public static boolean isRunning() {
        return isRunning;
    }

    public static void runTasks(Process[] processes) {
        for (Process p: processes) {
            Fiber fiber = new Fiber(() -> {
                try {
                    p.run();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            });
            activeFibers.put(fiber, p);
        }
        run();
    }

    private static void run() {
        if (isRunning || activeFibers.size() == 0)
            return;
        isRunning = true;
        processManagerSwitch(false);
        while (isRunning()) {
            Thread.yield();
        }
        stop();
    }

    public static void processManagerSwitch(boolean isFinished) {
        if (isFinished) {
            System.out.println("Fiber " + curFiber.getId() + " finished");
        }
        curFiber = getNextFiber();
        if (activeFibers.get(curFiber).isFinished()) {
            System.out.println("All tasks done");
            return;
        }
        System.out.println("Fiber " + curFiber.getId() + " is running");
        Fiber.fiberSwitch(curFiber.getId());
    }

    private static Fiber getNextFiber() {
        int size = (int) activeFibers.values().stream().filter(x -> !x.isFinished()).count();
        if (size == 0) {
            return Fiber.getPrimaryFiber();
        }
        int index = random.nextInt(max(1, size));
        for (Fiber fiber: activeFibers.keySet()) {
            if (!activeFibers.get(fiber).isFinished()) {
                if (index == 0)
                    return fiber;
                index--;
            }
        }
        return Fiber.getPrimaryFiber();
    }

    private static void stop() {
        for (Fiber fiber: activeFibers.keySet()) {
            if (!fiber.isPrimary())
                fiber.delete();
        }
        curFiber = null;
        activeFibers.clear();
        isRunning = false;
    }
}
