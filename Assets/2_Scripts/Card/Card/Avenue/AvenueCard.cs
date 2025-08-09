using Unity.Netcode;
using UnityEngine;

public class AvenueCard : NetworkBehaviour
{
    [SerializeField] private NetworkObject mNetworkObject;
    [SerializeField] private AvenueCardData mNetworkData;

    public void SetData(AvenueCardData data)
    {
        mNetworkData = data;
    }

    public ulong Spawn()
    {
        mNetworkObject.Spawn();

        return mNetworkObject.NetworkObjectId;
    }
}