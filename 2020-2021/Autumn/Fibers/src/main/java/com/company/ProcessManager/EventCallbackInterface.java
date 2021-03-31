package com.company.ProcessManager;

import com.sun.jna.Callback;

public interface EventCallbackInterface extends Callback {
    int callback(int param) throws InterruptedException;
}