package com.company.t34;

import com.company.abstractTank.Tank;

public class T34 extends Tank {
    public T34(int year) {
        super(year, 1, 4,27.5, "USSR", "T-34", "medium tank");
    }

    public T34() {
        super(1940, 1, 4, 27.5, "USSR", "T-34", "medium tank");
    }
}
