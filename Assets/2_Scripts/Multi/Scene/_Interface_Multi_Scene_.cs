using System;
using Unity.Netcode;using UnityEngine;

public interface IMultiSceneLoader
{
    void Init(Action<ulong, int> onLoadSuccess);
    
    void Request(string sceneName);
}

public abstract class MultiSceneLoader : NetworkBehaviour, IMultiSceneLoader
{
    public abstract void Init(Action<ulong, int> onLoadSuccess);

    public abstract void Request(string sceneName);
}