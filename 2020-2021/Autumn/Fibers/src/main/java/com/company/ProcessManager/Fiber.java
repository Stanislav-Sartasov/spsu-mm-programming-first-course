package com.company.ProcessManager;

public class Fiber {
    /// The fiber action delegate.
    private Runnable action;
    /// Gets the fiber identifier.
    private long id;

    public long getId() {
        return id;
    }

    /// Gets the id of the primary fiber.
    /// <remarks>If the id is 0 then this means that there is no primary id on the fiber.</remarks>
    private static long primaryId;

    public static long getPrimaryId() {
        return primaryId;
    }
    private static Fiber primaryFiber;

    public static Fiber getPrimaryFiber() {
        return primaryFiber;
    }

    /// Gets the flag identifing the primary fiber (a fiber that can run other fibers).
    private boolean isPrimary;

    public boolean isPrimary() {
        return isPrimary;
    }

    /// Initializes a new instance of the <see cref="Fiber"/> class.
    /// <param name='action'>Action.</param>
    public Fiber(Runnable action) {
        try {
            innerCreate(action);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
    }

    /// Deletes the current fiber.
    /// <remarks>This method should only be used in the fiber action that's executing.</remarks>
    public void delete() {
        try {
            Thread.sleep(1);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        UnmanagedFiberAPI.kernel32.DeleteFiber(id);
    }

    /// Deletes the fiber with the specified fiber id.
    /// <param name='fiberId'>fiber id.</param>
    public static void delete(long fiberId) {
        try {
            Thread.sleep(1);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        UnmanagedFiberAPI.kernel32.DeleteFiber(fiberId);
    }

    /// Switches the execution context to the next fiber.
    /// <param name='fiberId'>Fiber id.</param>
    public static void fiberSwitch(long fiberId) {
    // for debug only and to show that indeed it works! Remove this line!!!
    //    System.out.println("Fiber [" + fiberId + "] Switch");
        try {
            Thread.sleep(1);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        UnmanagedFiberAPI.kernel32.SwitchToFiber(fiberId);
    }

    /// Creates the fiber.
    /// <remarks>This method is responsible for the *actual* fiber creation.</remarks>
    /// <param name='action'>Fiber action.</param>
    private void innerCreate(Runnable action) throws InterruptedException {
        this.action = action;
        if (primaryId == 0) {
            primaryId = UnmanagedFiberAPI.kernel32.ConvertThreadToFiber(0);
            isPrimary = true;
            primaryFiber = this;
        }
        EventCallbackInterface lpFiber = this::fiberRunnerProc;
        id = UnmanagedFiberAPI.kernel32.CreateFiber(100500, lpFiber, 0);
    }

    /// Fiber method that executes the fiber action.
    /// <param name='lpParam'>Lp parameter.</param>
    /// <returns>fiber status code.</returns>
    private int fiberRunnerProc(int param) throws InterruptedException {
        int status = 0;
        try {
            action.run();
        } catch (Exception e) {
            status = 1;
            System.out.println(e);
            e.printStackTrace();
        } finally {
            if (status == 1) {
                try {
                    Thread.sleep(1);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
                UnmanagedFiberAPI.kernel32.DeleteFiber(id);
            }
        }
        return status;
    }
}