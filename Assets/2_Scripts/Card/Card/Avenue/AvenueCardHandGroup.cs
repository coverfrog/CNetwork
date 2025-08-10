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
    [SerializeField] private uint mInitDrawCount = 4;
    [Space]
    [SerializeField] private AvenueCardHand mCardHandOrigin;
    [Space] 
    [SerializeField] private Transform mCardHandSpawnPointMe;
    [SerializeField] private Transform mCardHandSpawnPointOther;

    public void Init_Request(AvenueCardDeck deck)
    {
        Lobby? current = MultiManager.Instance.Current;
        
        if (!current.HasValue)
        {
            return;
        }

        int idx = 0;
        
        Vector3[] spawnPoints = new Vector3[2]
        {
            mCardHandSpawnPointMe.position,
            mCardHandSpawnPointOther.position,
        };

        foreach (Friend friend in current.Value.Members)
        {
            // - spawn
            AvenueCardHand hand = Instantiate(mCardHandOrigin);
            hand.Spawn();
            
            // - point
            hand.Set_Origin_Rpc(spawnPoints[idx]);
            
            for (int i = 0; i < mInitDrawCount; i++)
            {
                // - deck
                AvenueCard card = deck.Draw();
                deck.Draw_Rpc();
            
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