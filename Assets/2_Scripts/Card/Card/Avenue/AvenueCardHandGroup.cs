using System.Collections.Generic;
using Sirenix.OdinInspector;
using Steamworks;
using Steamworks.Data;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Readonly = Sirenix.OdinInspector.ReadOnlyAttribute;

public class AvenueCardHandGroup : NetworkBehaviour
{
    [SerializeField] private NetworkObject mNetworkObject;

    private int _mSpawnPointIndex;
    
    public ulong Spawn()
    {
        mNetworkObject.Spawn();
        return mNetworkObject.NetworkObjectId;
    }
    
    public void Init_Request(AvenueGameContext context)
    {
        Lobby? current = MultiManager.Instance.Current;
        
        if (!current.HasValue)
        {
            return;
        }

        Set_Origin_Index_Rpc(0);

        Vector3[] spawnPoints = new Vector3[2]
        {
            context.handMeOriginTr.position,
            context.handOtherOriginTr.position,
        };
        
        foreach (Friend friend in current.Value.Members)
        {
            // - spawn
            AvenueCardHand hand = Instantiate(context.handOrigin);
            hand.Spawn();
            
            // - point
            hand.Set_Origin_Rpc(spawnPoints[_mSpawnPointIndex]);
            
            for (int i = 0; i < context.initDrawCount; i++)
            {
                // - deck
                AvenueCard card = context.deck.Draw();
                context.deck.Draw_Rpc();
            
                // - hand 
                ulong id = card.NetworkObjectId;
                hand.Add_Card_Rpc(id);
            }
            
            // - spread
            hand.Spread_Rpc();
            
            // - index
            Set_Origin_Index_Rpc(_mSpawnPointIndex + 1);
        }
    }

    [Rpc(SendTo.NotServer)]
    private void Set_Origin_Index_Rpc(int value)
    {
        _mSpawnPointIndex = value;
        
        Debug.Log(_mSpawnPointIndex);
    }
}