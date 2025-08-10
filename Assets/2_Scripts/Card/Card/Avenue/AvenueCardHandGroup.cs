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

        // - add hand
        foreach (Friend friend in current.Value.Members)
        {
            // - spawn
            AvenueCardHand hand = Instantiate(context.handOrigin);
            ulong handId = hand.Spawn();

            Set_IsMe_Rpc(handId, (ulong)friend.Id);
            
            for (int i = 0; i < context.initDrawCount; i++)
            {
                // - deck
                AvenueCard card = context.deck.Draw();
                context.deck.Draw_Rpc();
            
                // - hand 
                ulong cardId = card.NetworkObjectId;
                hand.Add_Card_Rpc(cardId);
            }
            
            // - spread
            hand.Spread_Rpc();
        }
    }
    
    [Rpc(SendTo.Everyone)]
    private void Set_IsMe_Rpc(ulong handId, ulong steamId)
    {
        if (!NetCustomUtil.FindSpawned(handId, out AvenueCardHand hand))
        {
            return;
        }
        
        bool isMe = steamId == SteamClient.SteamId;
        
        hand.Set_IsMe(isMe);
    }
}