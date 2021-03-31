package com.company.ProcessManager;

import com.sun.jna.Library;
import com.sun.jna.Native;

public class UnmanagedFiberAPI {
    public interface Kernel32 extends Library  {
        long ConvertThreadToFiber(long lpParameter);

        void SwitchToFiber(long lpFiber);

        void DeleteFiber(long lpFiber);

        long CreateFiber(long dwStackSize, EventCallbackInterface lpStartAddress, long lpParameter);
    }

    public static Kernel32 kernel32 = Native.load("kernel32", Kernel32.class);
}