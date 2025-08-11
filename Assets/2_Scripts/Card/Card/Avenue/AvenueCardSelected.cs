using Unity.Netcode;
using UnityEngine;

public class AvenueCardSelected : NetworkBehaviour
{
    [SerializeField] private NetworkObject mNetworkObject;
    [Space]
    [SerializeField] private AvenueCard mHandSelectedCard;
    [SerializeField] private AvenueCard mDeckSelectedCard;
    
    public void Spawn()
    {
        mNetworkObject.Spawn();
    }

    public void Set_Hand_Selected_Rpc(ulong id)
    {
        if (!NetCustomUtil.FindSpawned(id, out AvenueCard card))
        {
            return;
        }

        mHandSelectedCard = card;
    }

    public void Set_Deck_Selected_Rpc(ulong id)
    {
        if (!NetCustomUtil.FindSpawned(id, out AvenueCard card))
        {
            return;
        }

        mDeckSelectedCard = card;
    }
}