using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SceneAvenueGameHandler : SceneHandler
{
    [SerializeField] private CardDeck mCardDeckOrigin;
    
    private CardDeck _mCardDeck;
    
    public override void OnSceneLoaded(bool isServer, List<ulong> idList)
    {
        Debug.Log($"isServer: {isServer}");
        
        if (!isServer)
        {
            return;
        }
        
        foreach (ulong id in idList)
        {
            Debug.Log(id);
        }

        _mCardDeck = Instantiate(mCardDeckOrigin);
        _mCardDeck.GetComponent<NetworkObject>().Spawn();
    }
}