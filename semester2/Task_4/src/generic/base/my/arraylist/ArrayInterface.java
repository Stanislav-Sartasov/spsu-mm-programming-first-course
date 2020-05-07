package generic.base.my.arraylist;

public interface ArrayInterface<T> extends Iterable<T>
{
    void add(T value);
    boolean delete(int index);
    T findValue(T value);
    int findIndex(T value);
    T get(int index);
    void set(int index, T newValue);
    int size();
}
