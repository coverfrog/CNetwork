using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class AvenueCardData
{
    [Header("[ Text ]")]
    public string codeName;
    public string displayName;
    public string description;
    
    [Header("[ Texture ]")]
    public Texture2D frontTexture;

    public void Load(FixedString512Bytes frontTexturePath)
    {
        Addressables.LoadAssetAsync<Texture2D>(frontTexturePath.Value).Completed += ao =>
        {
            frontTexture = ao.Result;
        };
    }
}