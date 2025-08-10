using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AvenueCard : NetworkBehaviour
{
    private static readonly int FrontTexture = Shader.PropertyToID("_Front_Texture");
    private static readonly int BackTexture = Shader.PropertyToID("_Back_Texture");

    [SerializeField] private NetworkObject mNetworkObject;
    [SerializeField] private MeshRenderer mMeshRender;
    [Space] 
    [SerializeField] private bool mIsMe;
    [SerializeField] private AvenueCardData mData;

    public AvenueCardData Data => mData;
    
    private readonly Queue<Vector3> _mPositionQueue = new Queue<Vector3>();
    
    public AvenueCard SetData(AvenueCardData data)
    {
        mData = data;
        return this;
    }

    public AvenueCard SetName(string value)
    {
        gameObject.name = value;
        return this;
    }
    
    public AvenueCard SetPosition(Vector3 position)
    {
        _mPositionQueue.Enqueue(position);
        return this;
    }

    public AvenueCard SetDeckCursor(int value)
    {
        mData.deckCursor = value;
        return this;
    }

    public AvenueCard SetFrontTexture(Texture texture)
    {
        mMeshRender.material.SetTexture(FrontTexture, texture);
        return this;
    }

    public AvenueCard SetBackTexture(Texture texture)
    {
        mMeshRender.material.SetTexture(BackTexture, texture);
        return this;
    }

    public ulong Spawn()
    {
        mNetworkObject.Spawn();
        return mNetworkObject.NetworkObjectId;
    }

    private void Update()
    {
        if (_mPositionQueue.TryDequeue(out Vector3 position))
        {
            transform.position = position;
        }
    }
}