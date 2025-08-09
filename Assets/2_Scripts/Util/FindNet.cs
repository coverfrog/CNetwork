using Unity.Netcode;
using UnityEngine;

public static class FindNet
{
    public static bool Spawned<T>(ulong id, out T t) where T : Object
    {
        return NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].TryGetComponent<T>(out t);
    }
}