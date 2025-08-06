using UnityEngine;

public interface IMultiSceneLoader
{
    public void Request(string sceneName);
}

public abstract class MultiSceneLoader : IMultiSceneLoader
{
    public abstract void Request(string sceneName);
}