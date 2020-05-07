// strlen("ШемякинАндрейАлександрович") = 26

package generic.base;

import generic.base.my.arraylist.*;

public class Main {

    public static void main(String[] args)
    {
        MyArrayList<String> example = new MyArrayList<String>();
        example.add("One");
        example.add("Two");
        example.add("Tree");
        for(String i:example)
        {
            System.out.println(i);
        }

        System.out.println("\n");
        example.delete(1);
        for(String i:example)
        {
            System.out.println(i);
        }

        System.out.println("\n");
        System.out.println(example.size());
        System.out.println(example.findValue("One"));
        System.out.println(example.findIndex("One"));
        System.out.println(example.get(0));

        System.out.println("\n");
        example.set(0, "Hellow");
        for(String i:example)
        {
            System.out.println(i);
        }


    }
}
