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
    public Texture2D backTexture;

    [Header("[ Id ]")] 
    public int deckCursor;
    
    private int _loadedCount;
    private const int LoadTargetCount = 2;

    private Action<AvenueCardData> _onLoadedAction;
    
    public void Load(AvenueCardNetworkData networkData, Action<AvenueCardData> onLoaded)
    {
        _onLoadedAction = onLoaded;
        
        Addressables.LoadAssetAsync<Texture2D>(networkData.frontTexturePath.Value).Completed += ao =>
        {
            frontTexture = ao.Result;
            OnLoaded();
        };
        
        Addressables.LoadAssetAsync<Texture2D>(networkData.backTexturePath.Value).Completed += ao =>
        {
            backTexture = ao.Result;
            OnLoaded();
        };
    }

    private void OnLoaded()
    {
        ++_loadedCount;

        if (_loadedCount != LoadTargetCount)
        {
            return;
        }
        
        _onLoadedAction?.Invoke(this);
    }
}