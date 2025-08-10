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
    [Space]
    [ShowInInspector, Readonly] private Dictionary<bool, AvenueCardHand> _handDict = new Dictionary<bool, AvenueCardHand>();

    public void Init_Request(AvenueCardDeck deck)
    {
        Lobby? current = MultiManager.Instance.Current;
        
        if (!current.HasValue)
        {
            return;
        }

        foreach (Friend friend in current.Value.Members)
        {
            AvenueCardHand hand = Instantiate(mCardHandOrigin);
            hand.Spawn();
            
            bool isMe = friend.IsMe;
            
            hand.Init_Request(deck);
        }
    }
}