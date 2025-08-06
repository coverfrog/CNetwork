using System;
using System.Collections.Generic;
using Unity.Netcode;using UnityEngine;

public interface IMultiSceneLoader
{
    void Init(Action<List<ulong>> onLoadSuccess);

    void Request(string sceneName);
}

public abstract class MultiSceneLoader : NetworkBehaviour, IMultiSceneLoader
{
    
    public abstract void Init(Action<List<ulong>> onLoadSuccess);
    
    public abstract void Request(string sceneName);
}