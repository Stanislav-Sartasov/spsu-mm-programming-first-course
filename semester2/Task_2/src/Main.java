import java.awt.desktop.SystemEventListener;

public class Main
{
    public static void main(String[] args)
    {
        T34 t34 = new T34("USSR", 26, 4, 54.4f, 500,1942);
        t34.printTank();
        System.out.println("\n");
        Tiger tiger = new Tiger("Germany", 57, 5, 44.3f, 650, 1355);
        tiger.printTank();
    }
}
