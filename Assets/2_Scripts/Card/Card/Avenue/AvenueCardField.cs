using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AvenueCardField : NetworkBehaviour
{
    [SerializeField] private NetworkObject mNetworkObject;  
    [Space]
    [SerializeField] private bool mIsMe;
    [SerializeField] private Vector3 mOriginPoint = Vector3.zero;
    [SerializeField] private List<AvenueCard> mCardList = new List<AvenueCard>();

    public bool IsMe => mIsMe;

    public ulong Spawn()
    {
        mNetworkObject.Spawn();
        return NetworkObjectId;
    }

    public void Set_IsMe(bool isMe)
    {
        mIsMe = isMe;
    }

    public void Set_Origin(Vector3 position)
    {
        mOriginPoint = position;
    }

    public void On_Select(ulong selectId, ulong remainId)
    {
        ulong id = IsMe ? selectId : remainId;
        
        if (!NetCustomUtil.FindSpawned(id, out AvenueCard card))
        {
            return;
        }
        
        mCardList.Add(card);
        
        card.Set_Position_Tween(mOriginPoint);
        card.Set_Scale_Tween_Rpc(Vector3.one);
        card.Set_Rotation_Tween_Rpc(Vector3.zero);
    }
}
