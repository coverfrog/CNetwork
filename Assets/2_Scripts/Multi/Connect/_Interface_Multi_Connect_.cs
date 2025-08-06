using System;
using UnityEngine;

public interface IMultiConnector
{
    void Set(ulong id);
    
    void Connect();
}

public abstract class MultiConnector : IMultiConnector
{
    public abstract void Set(ulong id);

    public abstract void Connect();
}
