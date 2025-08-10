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

    private readonly Dictionary<bool, AvenueCardHand> _mHands = new Dictionary<bool, AvenueCardHand>();
    
    public ulong Spawn()
    {
        mNetworkObject.Spawn();
        return mNetworkObject.NetworkObjectId;
    }

    public override void OnNetworkSpawn()
    {
        _mHands.Clear();
    }
    
    public void Init_Request(AvenueGameContext context)
    {
        Lobby? current = MultiManager.Instance.Current;
        
        if (!current.HasValue)
        {
            return;
        }

        // - add hand
        bool isMe = true;
        foreach (Friend friend in current.Value.Members)
        {
            // - spawn
            AvenueCardHand hand = Instantiate(context.handOrigin);
            ulong handId = hand.Spawn();
            
            // - add
            Add_Hand_Rpc(handId, isMe);

            isMe = !isMe;
            
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
        
        // - set 
        foreach (KeyValuePair<bool, AvenueCardHand> pair in _mHands)
        {
            if (pair.Key == true)
            {
                pair.Value.Set_Origin_Rpc(context.handMeOriginTr.position);
            }

            else
            {
                pair.Value.Set_Origin_Rpc(context.handOtherOriginTr.position);
                
            }
            
            // - spread
            pair.Value.Spread_Rpc();
        }
    }

    [Rpc(SendTo.Everyone)]
    private void Add_Hand_Rpc(ulong handId, bool isMe)
    {
        if (!NetCustomUtil.FindSpawned(handId, out AvenueCardHand hand))
        {
            return;
        }
        
        _mHands.Add(isMe, hand);
    }
}