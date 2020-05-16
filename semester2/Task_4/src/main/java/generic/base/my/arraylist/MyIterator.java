package generic.base.my.arraylist;

import java.util.Iterator;

public class MyIterator<T> implements Iterator<T>
{
    private final T[] value;
    private int i = 0;

    MyIterator(T[] value)
    {
        this.value = value;
    }

    @Override
    public boolean hasNext()
    {
        return i < value.length;
    }

    @Override
    public T next()
    {
        return value[i++];
    }
}