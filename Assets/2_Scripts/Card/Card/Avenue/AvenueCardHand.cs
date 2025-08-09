using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AvenueCardHand : NetworkBehaviour
{
    [SerializeField] private NetworkObject mNetworkObject;
    [Space] 
    [SerializeField] private List<AvenueCard> mCardList = new List<AvenueCard>();

    private AvenueCardDeck _mDeck;
    private SceneAvenueGameHandler _mGameHandler;
    
    public ulong Spawn()
    {
        mNetworkObject.Spawn();
        return mNetworkObject.NetworkObjectId;
    }

    public void Init_Request(SceneAvenueGameHandler gameHandler, AvenueCardDeck deck)
    {
        _mDeck = deck;
        _mGameHandler = gameHandler;

        Vector3 position = 4.5f * (IsServer ? Vector3.down : Vector3.up);
    }
}
