using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AvenueCardHand : NetworkBehaviour
{
    [SerializeField] private NetworkObject mNetworkObject;
    [Space] 
    [SerializeField] private AvenueCardHandData mData;
    [SerializeField] private List<AvenueCard> mCardList = new List<AvenueCard>();
    
    public AvenueCardHandData Data => mData;
    
    public ulong Spawn()
    {
        mNetworkObject.Spawn();
        return mNetworkObject.NetworkObjectId;
    }
    
    public AvenueCardHand SetData(AvenueCardHandData data)
    {
        mData = data;
        return this;    
    }

    public AvenueCardHand SetName(string value)
    {
        gameObject.name = value;
        return this;    
    }

    [Rpc(SendTo.Everyone)]
    public void Add_Rpc(Vector3 position, ulong id)
    {
        Debug.Log(id);
        // if (!NetCustomUtil.FindSpawned(id, out AvenueCard card))
        // {
        //     return;
        // }
        //
        // card.SetPosition(position);
        //
        // mCardList.Add(card);
    }
}
