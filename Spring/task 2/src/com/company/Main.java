package com.company;

public class Main {

    public static void main(String[] args) {
	    tank t34 = new T_34();
	    System.out.println(t34.getTheCharacteristic());

	    tank a = new T_34();

        tank mkv = null;
        try {
            mkv = new MkV(1919, 28);
            System.out.println(mkv.getTheCharacteristic());
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}