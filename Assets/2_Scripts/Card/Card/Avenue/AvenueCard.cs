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
    
    private Tween _mPositionTween;
    private Tween _mRotationTween;
    private Tween _mScaleTween;
    
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

    #region > Position

    public AvenueCard Set_Position_Tween(Vector3 position, float duration = 0.2f)
    {
        _mPositionTween?.Kill();
        _mPositionTween = transform.DOMove(position, duration);
        
        return this;
    }

    [Rpc(SendTo.Everyone)]
    public void Set_Position_Tween_Rpc(Vector3 position, float duration = 0.2f)
    {
        Set_Position_Tween(position, duration);
    }

    #endregion

    #region > Scale

    [Rpc(SendTo.Everyone)]
    public void Set_Scale_Tween_Rpc(Vector3 scale, float duration = 0.2f)
    {
        _mScaleTween?.Kill();
        _mScaleTween = transform.DOScale(scale, duration);
    }

    #endregion

    #region > Rotation
    
    public AvenueCard Set_Rotation(Vector3 eulerAngles)
    {
        _mRotationTween?.Kill();
        
        transform.eulerAngles = eulerAngles;
        
        return this;
    }
    
    [Rpc(SendTo.Everyone)]
    public void Set_Rotation_Tween_Rpc(Vector3 eulerAngles, float duration = 0.2f)
    {
        _mRotationTween?.Kill();
        _mRotationTween = transform.DORotate(eulerAngles, duration);
    }

    #endregion
    
    
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
}