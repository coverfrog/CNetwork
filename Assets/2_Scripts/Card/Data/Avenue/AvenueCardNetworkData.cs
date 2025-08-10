using System;
using Unity.Collections;
using Unity.Netcode;

[Serializable]
public struct AvenueCardNetworkData : INetworkSerializable, IEquatable<AvenueCardNetworkData>
{
    #region Text

    public FixedString128Bytes codeName;
    public FixedString128Bytes displayName;
    public FixedString128Bytes description;

    #endregion

    #region Texture

    public FixedString512Bytes frontTexturePath;
    public FixedString512Bytes backTexturePath;

    #endregion

    #region Id

    public ulong netId;

    #endregion

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        // text
        serializer.SerializeValue(ref codeName);
        serializer.SerializeValue(ref displayName);
        serializer.SerializeValue(ref description);
        
        // texture
        serializer.SerializeValue(ref frontTexturePath);
        serializer.SerializeValue(ref backTexturePath);
        
        // id
        serializer.SerializeValue(ref netId);
    }

    public bool Equals(AvenueCardNetworkData other)
    {
        return codeName.Equals(other.codeName);
    }
}