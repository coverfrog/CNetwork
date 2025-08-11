using Unity.Netcode;
using UnityEngine;

public class AvenueGameRpc : NetworkBehaviour
{
    [SerializeField] private NetworkObject mNetworkObject;
    [Space]
    [SerializeField] private AvenueGameHandler mGameHandler;
    
    public ulong Spawn()
    {
        mNetworkObject.Spawn();
        return NetworkObjectId;
    }

    [Rpc(SendTo.Everyone)]
    public void Init_Rpc()
    {
        mGameHandler = FindAnyObjectByType<AvenueGameHandler>();
    }
    
    [Rpc(SendTo.Everyone)]
    public void StateChange_Rpc(AvenueGameState newState)
    {
        mGameHandler.OnStateChange(newState);
    }
}