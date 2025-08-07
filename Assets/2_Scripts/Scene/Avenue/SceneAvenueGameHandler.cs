using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SceneAvenueGameHandler : SceneHandler
{
    [SerializeField] private CardDeck mCardDeck;
    
    public override void OnSceneLoaded(bool isServer, List<ulong> idList)
    {
        Debug.Log($"isServer: {isServer}");
        
        if (isServer)
        {
            mCardDeck.GetComponent<NetworkObject>().InstantiateAndSpawn(NetworkManager.Singleton);
        }
        
        foreach (ulong id in idList)
        {
            Debug.Log(id);
        }
    }
}