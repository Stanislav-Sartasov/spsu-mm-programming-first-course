package project2.base;

import project2.derivedclass.tank.*;
public class Main
{
    public static void main(String[] args)
    {
        T34 t34 = new T34("USSR", 26, 4, 54.4f, 500,1942);
        System.out.println(t34.printTank());
        System.out.println("\n");
        Tiger tiger = new Tiger("Germany", 57, 5, 44.3f, 650, 1355);
        System.out.println(tiger.printTank());
    }
}

