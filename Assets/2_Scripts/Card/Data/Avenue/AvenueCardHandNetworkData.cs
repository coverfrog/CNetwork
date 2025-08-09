using System;
using Netcode;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class AvenueCardHandNetworkData : INetworkSerializable
{
    public ulong id;
    public bool isMe;
    public Vector3 positionOrigin;
    public FixedString128Bytes objName;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref id);
        serializer.SerializeValue(ref isMe);
        serializer.SerializeValue(ref positionOrigin);
        serializer.SerializeValue(ref objName);
    }
}