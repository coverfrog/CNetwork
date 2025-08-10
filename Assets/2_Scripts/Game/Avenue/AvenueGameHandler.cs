using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AvenueGameInit))]
[RequireComponent(typeof(AvenueGameTurnBegin))]
public class AvenueGameHandler : MonoBehaviour
{
    [SerializeField] private AvenueGameContext mContext;
    [Space]
    [SerializeField] private AvenueGameInit mInit;
    [SerializeField] private AvenueGameTurnBegin mTurnBegin;

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
            AvenueGameState.TurnBegin => mTurnBegin,
            _ => null
        };
        _mState?.OnEnter(this, mContext);
    }
}