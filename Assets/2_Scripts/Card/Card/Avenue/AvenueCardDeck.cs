using System;
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

    private int _mCursor;
    
    private Vector3 _mSpawnOriginPoint;

    private Action _mOnEnded;
    
    
    public ulong Spawn()
    {
        mNetworkObject.Spawn();
        return mNetworkObject.NetworkObjectId;
    }
    
    public void Init_Request(Vector3 spawnOriginPoint, Action onEnded)
    {
        if (!IsServer)
        {
            return;
        }

        List<AvenueCardNetworkData> networkDataList = mDataSoGroup.GetNetworkDataList();

        _mCardLoadedCount = 0;
        _mCardLoadTargetCount = networkDataList.Count;

        _mPlayerLoadedCount = 0;
        _mPlayerLoadTargetCount = NetworkManager.Singleton.ConnectedClients.Count;

        _mOnEnded = onEnded;
        
        Set_OriginPoint_Rpc(spawnOriginPoint);
        Set_Cursor_Rpc(networkDataList.Count - 1);
        
        foreach (AvenueCardNetworkData networkData in networkDataList)
        {
            // - ins
            AvenueCard card = Instantiate(mCardOrigin);
            ulong cardId = card.Spawn();
            
            // - add
            Add_Card_Rpc(cardId);
            
            // - apply
            Apply_Rpc(cardId, networkData);
        }
    }

    public AvenueCard Draw()
    {
        return mCardList[_mCursor];
    }
    
    private void OnEnd()
    {
        // -- end
        _mOnEnded?.Invoke();
    }

    [Rpc(SendTo.Everyone)]
    private void Set_OriginPoint_Rpc(Vector3 point)
    {
        _mSpawnOriginPoint = point;
    }

    [Rpc(SendTo.Everyone)]
    private void Set_Cursor_Rpc(int value)
    {
        _mCursor = value;
    }

    [Rpc(SendTo.Everyone)]
    private void Add_Card_Rpc(ulong netId)
    {
        // - net
        if (!NetCustomUtil.FindSpawned(netId, out AvenueCard card))
        {
            return;
        }
        
        // - add
        mCardList.Add(card);
    }

    [Rpc(SendTo.Everyone)]
    public void Set_Cursor_Add_Rpc(bool isAdd)
    {
        _mCursor = Mathf.Clamp(_mCursor + (isAdd ? +1 : -1), 0, int.MaxValue);
    }
    
    [Rpc(SendTo.Everyone)]
    private void Apply_Rpc(ulong cardId, AvenueCardNetworkData networkData)
    {
        // - net
        if (!NetCustomUtil.FindSpawned(cardId, out AvenueCard card))
        {
            return;
        }
        
        // - data common
        networkData.backTexturePath = mBackTexture.RuntimeKey.ToString();
        
        // - card
        AvenueCardDataConverter.ToData(networkData, data =>
        {
            // - pos set
            Vector3 pos = _mSpawnOriginPoint + Vector3.up * data.deckCursor * mCardHeight;
            
  
            
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
            OnEnd();
        });
    }
}