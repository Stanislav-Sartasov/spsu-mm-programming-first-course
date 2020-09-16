package com.company;

import com.company.abstractTank.Tank;
import com.company.markV.MkV;
import com.company.t34.T34;

public class Main {

    public static void main(String[] args) {
        Tank t34 = new T34();
        System.out.println(t34.getTheCharacteristic());

        Tank mkv;
        try {
            mkv = new MkV(1919, 28);
            System.out.println(mkv.getTheCharacteristic());
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}