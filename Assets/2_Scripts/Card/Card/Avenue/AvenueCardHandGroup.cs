using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Readonly = Sirenix.OdinInspector.ReadOnlyAttribute;

public class AvenueCardHandGroup : MonoBehaviour
{
    [SerializeField] private AvenueCardHand mCardHandOrigin;
    [Space] 
    [SerializeField] private Transform mCardHandSpawnPointMe;
    [SerializeField] private Transform mCardHandSpawnPointOther;
    [Space]
    [ShowInInspector, Readonly] private Dictionary<bool, AvenueCardHand> _handDict = new Dictionary<bool, AvenueCardHand>();
    
    public void Init_Request()
    {
        // - hand ins
        const int count = 2;
        
        bool[] isMes = new bool[count] { true, false };
        Vector3[] positions = new Vector3[count] { mCardHandSpawnPointMe.position, mCardHandSpawnPointOther.position };
        string[] names = new string[count] { "Hand [Me]", "Hande [Other]"};
        
        for (int i = 0; i < 2; i++)
        {
            AvenueCardHand hand = Instantiate(mCardHandOrigin);
            ulong handId = hand.Spawn();

            Init_Rpc(handId, new AvenueCardHandNetworkData()
            {
                id = handId,
                isMe = isMes[i],
                positionOrigin = positions[i],
                objName = new FixedString128Bytes(names[i])
            });
        }
    }
    
    [Rpc(SendTo.Everyone)]
    private void Init_Rpc(ulong handId, AvenueCardHandNetworkData networkData)
    {
        // - net
        if (!NetCustomUtil.FindSpawned(handId, out AvenueCardHand hand))
        {
            return;
        }
        
        // - data
        AvenueCardHandData data = AvenueCardDataConverter.ToData(networkData);

        hand
            .SetData(data)
            .SetName(data.objName);
            
        
        // - add
        _handDict.Add(data.isMe, hand);
    }
    
    public void DrawRequest(AvenueCardDeck deck)
    {
        foreach (KeyValuePair<bool, AvenueCardHand> pair in _handDict)
        {
            AvenueCard card = deck.Draw();
            deck.Set_Cursor_Add_Rpc(false);
            
            AvenueCardHand hand = _handDict[pair.Key];
        }
    }
}