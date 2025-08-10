using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SceneAvenueGameHandler : SceneHandler
{
    [SerializeField] private AvenueGameHandler mGameHandler;
    
    public override void OnSceneLoaded(bool isServer, List<ulong> idList)
    {
        mGameHandler.OnSceneLoaded(isServer, idList);
    }
}