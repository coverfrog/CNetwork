using System.Collections.Generic;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;

public class AvenueCardDeck : NetworkBehaviour
{
    [SerializeField] private AvenueCard mCardOrigin;
    [SerializeField] private AvenueCardDataSoGroup mDataSoGroup;
    [Space]
    [SerializeField] private List<AvenueCardData> mDataList;
    
    public void Init_Request()
    {
        if (!IsServer)
        {
            return;
        }
        
        List<AvenueCardNetworkData> networkDataList = mDataSoGroup.GetNetworkDataList();
        
        foreach (AvenueCardNetworkData networkData in networkDataList)
        {
            // - ins
            AvenueCard card = Instantiate(mCardOrigin);
            ulong id = card.Spawn();
            
            Apply_Rpc(id, networkData);
        }
    }

    [Rpc(SendTo.Everyone)]
    private void Apply_Rpc(ulong id, AvenueCardNetworkData networkData)
    {
        // - net
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].TryGetComponent(out AvenueCard card))
        {
            return;
        }
        
        // - card
        AvenueCardDataConverter.ToData(networkData, data =>
        {
            card.SetData(data);
        });
    }
}