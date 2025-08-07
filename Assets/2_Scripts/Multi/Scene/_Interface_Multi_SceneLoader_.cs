using System;
using System.Collections.Generic;
using Unity.Netcode;using UnityEngine;

public interface IMultiSceneLoader
{
    void Request(string sceneName);
}

public abstract class MultiSceneLoader : NetworkBehaviour, IMultiSceneLoader
{
    
    public abstract void Request(string sceneName);
}