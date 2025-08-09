using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SceneAvenueGameHandler : SceneHandler
{
    [SerializeField] private AvenueCardDeck mCardDeckOrigin;
    [Space] 
    [SerializeField] private AvenueCardHandGroup mHandGroup;
    [Space]
    [SerializeField] private Transform mCardDeckSpawnPoint;
   
    private AvenueCardDeck _mCardDeck;
    
    public override void OnSceneLoaded(bool isServer, List<ulong> idList)
    {
        Debug.Log($"isServer: {isServer}");
        
        if (!isServer)
        {
            return;
        }

        // - deck ins init
        _mCardDeck = Instantiate(mCardDeckOrigin);
        _mCardDeck.Spawn();
        _mCardDeck.Init_Request(mCardDeckSpawnPoint.position, OnDeckInitEnd);
    }

    private void OnDeckInitEnd()
    {
        // - hand init
        mHandGroup.Init_Request();
        
        // - hand draw
        mHandGroup.DrawRequest(_mCardDeck);
    }
}