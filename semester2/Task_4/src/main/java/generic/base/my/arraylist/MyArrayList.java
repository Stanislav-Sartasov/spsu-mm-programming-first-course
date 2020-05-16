package generic.base.my.arraylist;
import java.util.Iterator;

public class MyArrayList<T> implements ArrayInterface<T>
{
    private T[] mas;
    public MyArrayList()
    {
        this.mas = (T[]) new Object[0];
    }

    @Override
    public void add(T value)
    {
        try
        {
            T[] masPoiter = mas;
            mas = (T[]) new Object[masPoiter.length + 1];
            System.arraycopy(masPoiter, 0, mas, 0, masPoiter.length);
            mas[mas.length - 1] = value;
        }
        catch(ClassCastException e)
        {
            e.printStackTrace();
        }
    }

    @Override
    public boolean delete(int index)
    {
        try {
            T[] masPointer = mas;
            mas = (T[]) new Object[masPointer.length - 1];
            System.arraycopy(masPointer, 0, mas, 0, index);
            System.arraycopy(masPointer, index + 1, mas, index, masPointer.length - index - 1);
            return true;
        }
        catch(ClassCastException e)
        {
            e.printStackTrace();
        }
        return false;
    }

    @Override
    public T findValue(T value)
    {
        for (T i:mas)
        {
            if (i == value)
            {
                return value;
            }
        }
        return null;
    }

    @Override
    public int findIndex(T value)
    {
        for (int i = 0; i < mas.length; i++)
        {
            if (mas[i] == value)
            {
                return i;
            }
        }
        return -1;
    }

    @Override
    public T get(int index)
    {
        try
        {
            return mas[index];
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }
        return null;
    }

    @Override
    public void set(int index, T newValue)
    {
        try
        {
            mas[index] = newValue;
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }
    }

    @Override
    public int size()
    {
        return mas.length;
    }

    @Override
    public Iterator<T> iterator()
    {
        return new MyIterator<>(mas);
    }
}