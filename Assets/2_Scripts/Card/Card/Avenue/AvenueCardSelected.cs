using Unity.Netcode;
using UnityEngine;

public class AvenueCardSelected : NetworkBehaviour
{
    [SerializeField] private NetworkObject mNetworkObject;
    [Space]
    public AvenueCard mHandSelectedCard;
    public AvenueCard mDeckSelectedCard;
    
    public void Spawn()
    {
        mNetworkObject.Spawn();
    }

    [Rpc(SendTo.Everyone)]
    public void Set_Hand_Selected_Rpc(ulong id)
    {
        if (!NetCustomUtil.FindSpawned(id, out AvenueCard card))
        {
            return;
        }

        mHandSelectedCard = card;
    }

    [Rpc(SendTo.Everyone)]
    public void Set_Deck_Selected_Rpc(ulong id)
    {
        if (!NetCustomUtil.FindSpawned(id, out AvenueCard card))
        {
            return;
        }

        mDeckSelectedCard = card;
    }
}