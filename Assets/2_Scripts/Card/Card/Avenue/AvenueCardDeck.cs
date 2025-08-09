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
            // - data set
            _ = AvenueCardDataConverter.ToData(networkData, data =>
            {
                // - ins
                AvenueCard card = Instantiate(mCardOrigin);
                card.SetData(data);
                card.Spawn();
            });
        }
    }

    // [Rpc(SendTo.Everyone)]
    // private void Init_Request_Rpc(AvenueCardNetworkData networkData)
    // {
    //     
    //     mDataList.Add(data);
    // }
}