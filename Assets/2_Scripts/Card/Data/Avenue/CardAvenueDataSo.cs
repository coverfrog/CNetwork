using System;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Data", menuName = "Avenue/Card/Data")]
public class CardAvenueDataSo : CardDataSo
{
    [SerializeField] private string mCodeName;
    [SerializeField] private string mDisplayName;
    [SerializeField][TextArea] private string mDescription;
    [SerializeField] private Texture2D mFrontTexture;
    
    public string CodeName => mCodeName;
    public string DisplayName => mDisplayName;
    public string Description => mDescription;
}