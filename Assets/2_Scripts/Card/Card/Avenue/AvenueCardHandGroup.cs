using System.Collections.Generic;
using Sirenix.OdinInspector;
using Steamworks;
using Steamworks.Data;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Readonly = Sirenix.OdinInspector.ReadOnlyAttribute;

public class AvenueCardHandGroup : MonoBehaviour
{
    public void Init_Request(AvenueGameContext context)
    {
        Lobby? current = MultiManager.Instance.Current;
        
        if (!current.HasValue)
        {
            return;
        }

        int idx = 0;
        
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
            int originIdx = friend.Id == SteamClient.SteamId ? 0 : 1;
            hand.Set_Origin_Rpc(spawnPoints[originIdx]);
            
            for (int i = 0; i < context.initDrawCount; i++)
            {
                // - deck
                AvenueCard card = context.deck.Draw();
                context.deck.Draw_Rpc();
            
                // - hand 
                ulong id = card.NetworkObjectId;
                hand.Add_Card_Rpc(id);
            }
            
            // - cursor
            idx++;
            
            // - spread
            hand.Spread_Rpc();
        }
    }
}