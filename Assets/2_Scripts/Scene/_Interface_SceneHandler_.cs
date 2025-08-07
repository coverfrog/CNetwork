using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface ISceneHandler
{
    void OnSceneLoaded(bool isServer, List<ulong> idList);
}

public abstract class SceneHandler : MonoBehaviour, ISceneHandler
{
    public abstract void OnSceneLoaded(bool isServerr, List<ulong> idList);
}