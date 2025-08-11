using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AvenueCardHand : NetworkBehaviour
{
    [SerializeField] private float mCardSpace = 1.5f;
    [SerializeField] private NetworkObject mNetworkObject;
    [Space] 
    [SerializeField] private bool mIsMe;
    [SerializeField] private Vector3 mOriginPoint = Vector3.zero;
    [SerializeField] private List<AvenueCard> mCardList = new List<AvenueCard>();

    public bool IsMe => mIsMe;
    
    public ulong Spawn()
    {
        mNetworkObject.Spawn();
        return mNetworkObject.NetworkObjectId;
    }

    public void Set_IsMe(bool value)
    {
        mIsMe = value;
    }

    public void Set_Origin(Vector3 position)
    {
        mOriginPoint = position;
    }

    public void Set_Rotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
    
    public void Set_Rotation(Vector3 eulerAngles)
    {
        transform.eulerAngles = eulerAngles;
    }
    
    [Rpc(SendTo.Everyone)]
    public void Add_Card_Rpc(ulong id)
    {
        if (!NetCustomUtil.FindSpawned(id, out AvenueCard card))
        {
            return;
        }
        
        mCardList.Add(card);

        card.transform.position = mOriginPoint;
    }

    [Rpc(SendTo.Everyone)]
    public void Spread_Rpc()
    {
        int count = mCardList.Count;
        
        float width = mCardSpace * count * 0.5f;
        
        for (int i = 0; i < count; i++)
        {
            float p = count <= 1 ? 0.5f : i / (float)(count - 1);

            float l = Mathf.Lerp(-width, +width, p);

            mCardList[i].SetPosition(mOriginPoint + new Vector3(l, 0, 0));
        }
    }
}
