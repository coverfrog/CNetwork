using System;
using UnityEngine;

public enum MultiConnectType
{
    Server,
    Client
}

public interface IMultiConnector
{
    void Connect();
}

public abstract class MultiConnector : IMultiConnector
{
    public abstract void Connect();
}
