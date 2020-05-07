package generic.base.tests;


import generic.base.my.arraylist.MyArrayList;
import org.junit.Assert;
import org.junit.jupiter.api.Test;

public class MyArrayListTest
{

    @Test
    public void testInteger()
    {
        MyArrayList<Integer> test = new MyArrayList<>();
        test.add(1);
        test.add(2);
        test.add(3);
        test.add(4);
        Assert.assertEquals((int)test.get(0), 1);
        Assert.assertEquals((int)test.get(1), 2);
        Assert.assertEquals((int)test.get(2), 3);
        Assert.assertEquals((int)test.get(3), 4);
        Assert.assertEquals(test.size(), 4);
        Assert.assertEquals(test.findIndex(1), 0);
        Assert.assertEquals((int)test.findValue(1), 1);
        test.delete(3);

        int x = 1;
        for (Integer i:test)
        {
            Assert.assertEquals((int)i, x++);
        }
    }

    @Test
    public void testString()
    {
        MyArrayList<String> test = new MyArrayList<>();
        test.add("W");
        test.add("O");
        test.add("R");
        test.add("D");
        Assert.assertEquals(test.get(0), "W");
        Assert.assertEquals(test.get(1), "O");
        Assert.assertEquals(test.get(2), "R");
        Assert.assertEquals(test.get(3), "D");
        Assert.assertEquals(test.size(), 4);
        Assert.assertEquals(test.findIndex("O"), 1);
        Assert.assertEquals(test.findIndex("X"), -1);
        Assert.assertNull(test.findValue("X"));
        Assert.assertEquals(test.findValue("W"), "W");
        test.delete(3);

        int x = 0;
        String[] mas = new String[3];
        mas[0] = "W";
        mas[1] = "O";
        mas[2] = "R";

        for (String i:test)
        {
            Assert.assertEquals(i, mas[x++]);
        }
    }

}