using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(AvenueGameInit))]
[RequireComponent(typeof(AvenueGameHandCardSelect))]
[RequireComponent(typeof(AvenueGameCardSelect))]
public class AvenueGameHandler : MonoBehaviour
{
    [SerializeField] private AvenueGameState mState;
    [SerializeField] private AvenueGameContext mContext;
    [Space]
    [SerializeField] private AvenueGameInit mInit;
    [SerializeField] private AvenueGameHandCardSelect mHandCardSelect;
    [SerializeField] private AvenueGameCardSelect mCardSelect;

    private IAvenueGameState _mState;
    
    public bool IsServer { get; private set; }
    
    public void OnSceneLoaded(bool isServer, List<ulong> idList)
    {
        IsServer = isServer;
        
        if (!isServer)
        {
            return;
        }
        
        mContext.rpc = Instantiate(mContext.rpcOrigin);
        mContext.rpc.Spawn();
        mContext.rpc.Init_Rpc();

        StateChange(AvenueGameState.Init);
    }

    public void StateChange(AvenueGameState newState) => mContext.rpc.StateChange_Rpc(newState);

    public void OnStateChange(AvenueGameState newState)
    {
        mState = newState;
        
        _mState?.OnExit(this, mContext);
        _mState = newState switch
        {
            AvenueGameState.Init => mInit,
            AvenueGameState.HandCardSelect => mHandCardSelect,
            AvenueGameState.CardSelect => mCardSelect,
            _ => null
        };
        _mState?.OnEnter(this, mContext);
    }

    private void Update()
    {
        _mState?.OnUpdate(this, mContext);
    }
}