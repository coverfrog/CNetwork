using System;
using UnityEngine;

public interface IMultiConnector
{
    void Connect();
    
    void Connect(ulong id);
}

public abstract class MultiConnector : IMultiConnector
{
    protected const string KeyLobbyId = "lobby_id";

    public abstract void Connect();
    
    public abstract void Connect(ulong id);
}
