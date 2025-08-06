using System;
using System.Collections.Generic;
using Unity.Netcode;using UnityEngine;

public interface IMultiSceneLoader
{
    void Init(Action<List<ulong>> onLoadSuccess);

    void SetCount(int target);
    
    void Request(string sceneName);
}

public abstract class MultiSceneLoader : NetworkBehaviour, IMultiSceneLoader
{
    
    public abstract void Init(Action<List<ulong>> onLoadSuccess);
    
    public abstract void SetCount(int target);

    public abstract void Request(string sceneName);
}