using System.Collections.Generic;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;

public class AvenueCardDeck : NetworkBehaviour
{
    [SerializeField] private AvenueCardDataSoGroup mDataSoGroup;
    [SerializeField] private List<AvenueCardData> mDataList;
    public void Init_Request()
    {
        List<AvenueCardNetworkData> networkDataList = mDataSoGroup.GetNetworkDataList();

        foreach (AvenueCardNetworkData networkData in networkDataList)
        {
            Init_Request_Rpc(networkData);
        }
    }

    [Rpc(SendTo.Everyone)]
    private void Init_Request_Rpc(AvenueCardNetworkData networkData)
    {
        AvenueCardData data = AvenueCardDataConverter.ToData(networkData);
        
        mDataList.Add(data);
    }
}