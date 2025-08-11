using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AvenueGameInit))]
[RequireComponent(typeof(AvenueGameHandCardSelect))]
[RequireComponent(typeof(AvenueGameCardSelect))]
public class AvenueGameHandler : MonoBehaviour
{
    [SerializeField] private AvenueGameContext mContext;
    [Space]
    [SerializeField] private AvenueGameInit mInit;
    [SerializeField] private AvenueGameHandCardSelect mHandCardSelect;
    [SerializeField] private AvenueGameCardSelect mCardSelect;

    private IAvenueGameState _mState;
    
    public void OnSceneLoaded(bool isServer, List<ulong> idList)
    {
        if (!isServer)
        {
            return;
        }

        StateChange(AvenueGameState.Init);
    }

    public void StateChange(AvenueGameState newState)
    {
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