using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SceneAvenueGameHandler : SceneHandler
{
    [SerializeField] private CardAvenueDeck mCardDeckOrigin;
    
    private CardAvenueDeck _mCardDeck;
    
    public override void OnSceneLoaded(bool isServer, List<ulong> idList)
    {
        Debug.Log($"isServer: {isServer}");
        
        if (!isServer)
        {
            return;
        }

        // - deck ins init
        _mCardDeck = Instantiate(mCardDeckOrigin);
        _mCardDeck.GetComponent<NetworkObject>().Spawn();
        _mCardDeck.Init();
    }
}