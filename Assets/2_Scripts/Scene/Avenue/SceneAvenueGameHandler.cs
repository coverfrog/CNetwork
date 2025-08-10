using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SceneAvenueGameHandler : SceneHandler
{
    [SerializeField] private AvenueCardDeck mCardDeckOrigin;
    [Space]
    [SerializeField] private AvenueCardHandGroup mCardHandGroup;
    [Space]
    [SerializeField] private Transform mCardDeckSpawnPoint;
   
    public Vector3 CardDeckSpawnPoint => mCardDeckSpawnPoint.position;
    
    private AvenueCardDeck _mCardDeck;
    private AvenueCardHand _mCardHand;
    
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
        _mCardDeck.Init_Request(this);
    }

    public void OnDeckInitEnd()
    {
        // - hand ins init ( server only )
        mCardHandGroup.Init_Request(_mCardDeck);
    }
}