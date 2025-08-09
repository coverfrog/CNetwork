using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public static class NetCustomUtil
{
    public static bool FindSpawned<T>(ulong id, out T t) where T : Object
    {
        return NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].TryGetComponent<T>(out t);
    }

    public static IReadOnlyList<ulong> GetMemberIds()
    {
        return NetworkManager.Singleton.ConnectedClientsIds;
    }
}