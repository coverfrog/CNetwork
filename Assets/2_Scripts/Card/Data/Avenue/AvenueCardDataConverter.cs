using System;
using Unity.Collections;
using UnityEngine;

public static class AvenueCardDataConverter
{
    public static AvenueCardNetworkData ToNetworkData(AvenueCardDataSo so)
    {
        var result = new AvenueCardNetworkData()
        {
            // text
            codeName = new FixedString128Bytes(so.CodeName),
            displayName = new FixedString128Bytes(so.DisplayName),
            description = new FixedString128Bytes(so.Description),
            
            //texture
            frontTexturePath = so.FrontTexture.RuntimeKey.ToString()
        };
        
        return result;
    }

    public static AvenueCardData ToData(AvenueCardNetworkData networkData, Action<AvenueCardData> onLoaded)
    {
        var result = new AvenueCardData()
        {
            // text
            codeName = networkData.codeName.Value,
            displayName = networkData.displayName.Value,
            description = networkData.description.Value,
            
            // id
            deckCursor = networkData.deckCursor,
        };
        
        result.Load(networkData, onLoaded);
        
        return result;
    }
}