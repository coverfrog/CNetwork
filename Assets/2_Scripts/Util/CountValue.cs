using System;

[Serializable]
public struct CountValue<T>
{
    public T value;
    public int count;
}