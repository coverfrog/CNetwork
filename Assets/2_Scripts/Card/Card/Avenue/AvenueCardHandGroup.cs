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
    [Space]
    [SerializeField] private List<AvenueCardHand> mHandList = new List<AvenueCardHand>();
    
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

            Add_Hand_Rpc(handId);
            Set_IsMe_Rpc(handId, friend.Id);
            
            for (int i = 0; i < context.initDrawCount; i++)
            {
                // - deck
                AvenueCard card = context.deck.Draw();
                context.deck.Draw_Rpc();
            
                // - hand 
                ulong cardId = card.NetworkObjectId;
                hand.Add_Card_Rpc(cardId);
            }
        }
        
        foreach (AvenueCardHand avenueCardHand in mHandList)
        {
            Debug.Log(avenueCardHand.IsMe);
            
            // - set
            avenueCardHand.Set_Origin_Rpc(avenueCardHand.IsMe ?
                context.handMeOriginTr.position : 
                context.handOtherOriginTr.position);
            
            // - spread
            avenueCardHand.Spread_Rpc();
        }
    }
    
    [Rpc(SendTo.Everyone)]
    private void Add_Hand_Rpc(ulong handId)
    {
        if (!NetCustomUtil.FindSpawned(handId, out AvenueCardHand hand))
        {
            return;
        }
        
        mHandList.Add(hand);
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