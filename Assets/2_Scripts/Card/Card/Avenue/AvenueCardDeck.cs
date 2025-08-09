using System.Collections.Generic;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AvenueCardDeck : NetworkBehaviour
{
    [SerializeField] private float mCardHeight = 0.02f;
    [SerializeField] private AssetReference mBackTexture;
    [Space]
    [SerializeField] private NetworkObject mNetworkObject;
    [SerializeField] private AvenueCard mCardOrigin;
    [SerializeField] private AvenueCardDataSoGroup mDataSoGroup;
    [Space]
    [SerializeField] private List<AvenueCard> mCardList = new List<AvenueCard>();

    private int _mCardLoadedCount;
    private int _mCardLoadTargetCount;

    private int _mPlayerLoadedCount;
    private int _mPlayerLoadTargetCount;

    private Vector3 _mSpawnOriginPoint;
    
    public ulong Spawn()
    {
        mNetworkObject.Spawn();
        return mNetworkObject.NetworkObjectId;
    }
    
    public void Init_Request(Vector3 spawnOriginPoint)
    {
        if (!IsServer)
        {
            return;
        }

        Set_OriginPoint_Rpc(spawnOriginPoint);

        List<AvenueCardNetworkData> networkDataList = mDataSoGroup.GetNetworkDataList();

        _mCardLoadedCount = 0;
        _mCardLoadTargetCount = networkDataList.Count;

        _mPlayerLoadedCount = 0;
        _mPlayerLoadTargetCount = NetworkManager.Singleton.ConnectedClients.Count;
        
        mCardList.Clear();
        
        foreach (AvenueCardNetworkData networkData in networkDataList)
        {
            // - ins
            AvenueCard card = Instantiate(mCardOrigin);
            ulong id = card.Spawn();
            
            Apply_Rpc(id, networkData);
        }
    }

    [Rpc(SendTo.Everyone)]
    private void Set_OriginPoint_Rpc(Vector3 point)
    {
        _mSpawnOriginPoint = point;
    }
    
    private void OnApplyEnd()
    {
        // - pos
        Debug.Log("End");
    }

    [Rpc(SendTo.Everyone)]
    private void Apply_Rpc(ulong id, AvenueCardNetworkData networkData)
    {
        // - net
        if (!FindNet.Spawned(id, out AvenueCard card))
        {
            return;
        }
        
        // - data common
        networkData.backTexturePath = mBackTexture.RuntimeKey.ToString();
        
        // - card
        AvenueCardDataConverter.ToData(networkData, data =>
        {
            // - pos set
            Vector3 pos = _mSpawnOriginPoint + Vector3.up * mCardList.Count * mCardHeight;
            
            // - add
            mCardList.Add(card);
            
            // - set
            card
                .SetData(data)
                .SetName(data.displayName)
                .SetPosition(pos)
                .SetFrontTexture(data.frontTexture)
                .SetBackTexture(data.backTexture);
                
            
            // - counter
            ++_mCardLoadedCount;
            
            // - call
            if (_mCardLoadedCount < _mCardLoadTargetCount)
            {
                return;
            }

            ++_mPlayerLoadedCount;

            if (!IsServer || _mPlayerLoadedCount < _mPlayerLoadTargetCount)
            {
                return;
            }

            // - end
            OnApplyEnd();
        });
    }
}