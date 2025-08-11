using System;
using System.Collections.Generic;
using DG.Tweening;
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
    [SerializeField] private bool mIsSelected;
    [SerializeField] private AvenueCardData mData;

    public AvenueCardData Data => mData;
    
    public bool IsSelected => mIsSelected;
    
    private readonly Queue<Vector3> _mPositionQueue = new Queue<Vector3>();
    private readonly Queue<Vector3> _mRotationQueue = new Queue<Vector3>();
    private readonly Queue<Vector3> _mScaleQueue = new Queue<Vector3>();
    
    private Tween _mScaleTween;
    private Tween _mMoveTween;
    private Tween _mMScaleTween;
    
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
    
    public AvenueCard Set_Position_Tween(Vector3 position)
    {
        _mPositionQueue.Enqueue(position);
        return this;
    }

    [Rpc(SendTo.Everyone)]
    public void Set_Position_Tween_Rpc(Vector3 position)
    {
        _mPositionQueue.Enqueue(position);
    }

    [Rpc(SendTo.Everyone)]
    public void Set_Scale_Tween_Rpc(Vector3 scale)
    {
        _mScaleQueue.Enqueue(scale);
    }
    
    public AvenueCard Set_Rotation_Tween(Vector3 eulerAngles)
    {
        _mRotationQueue.Enqueue(eulerAngles);
        return this;
    }
    
    [Rpc(SendTo.Everyone)]
    public void Set_Rotation_Tween_Rpc(Vector3 eulerAngles)
    {
        _mRotationQueue.Enqueue(eulerAngles);
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

    //
    
    [Rpc(SendTo.Everyone)]
    public void On_Focus_Rpc()
    {
        _mScaleTween?.Kill();
        _mScaleTween = transform.
            DOScale(Vector3.one * 1.5f, 0.2f).
            SetEase(Ease.OutBack);
    }

    [Rpc(SendTo.Everyone)]
    public void On_UnFocus_Rpc()
    {
        _mScaleTween?.Kill();
        _mScaleTween = transform.
            DOScale(Vector3.one * 1.0f, 0.05f).
            SetEase(Ease.OutBack);
    }
    
    //

    [Rpc(SendTo.Everyone)]
    public void Set_Select_Rpc(bool value)
    {
        mIsSelected = value;
    }
    
    //

    private void Update()
    {
        if (_mPositionQueue.TryDequeue(out Vector3 position))
        {
            _mMoveTween?.Kill();
            _mMoveTween = transform.
                DOMove(position, 0.2f);
        }
        
        if (_mRotationQueue.TryDequeue(out Vector3 eulerAngles))
        {
            transform.eulerAngles = eulerAngles;
        }
        
        if (_mScaleQueue.TryDequeue(out Vector3 scale))
        {
            _mScaleTween?.Kill();
            _mScaleTween = transform.
                DOScale(scale, 0.2f);
        }
    }
}