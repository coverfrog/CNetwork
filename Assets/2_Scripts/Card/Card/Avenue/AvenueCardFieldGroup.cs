using System.Collections.Generic;
using System.Linq;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using UnityEngine;

public class AvenueCardFieldGroup : NetworkBehaviour
{
    [SerializeField] private NetworkObject mNetworkObject;
    [Space]
    [SerializeField] private List<AvenueCardField> mFieldList = new List<AvenueCardField>();

    public AvenueCardField MyField => mFieldList.FirstOrDefault(f => f.IsMe);
    
    public void Spawn()
    {
        mNetworkObject.Spawn();
    }

    public void Init_Request(AvenueGameContext context)
    {
        Lobby? current = MultiManager.Instance.Current;
        
        if (!current.HasValue)
        {
            return;
        }

        // - add hand
        foreach (Friend friend in current.Value.Members)
        {
            // - spawn
            AvenueCardField field = Instantiate(context.fieldOrigin);
            ulong fieldId = field.Spawn();

            Add_Field_Rpc(fieldId);
            Set_IsMe_Rpc(fieldId, friend.Id);
        }

        Set_Origin_Rpc(context.fieldMeOriginTr.position,context.fieldOtherOriginTr.position);
    }
    
    [Rpc(SendTo.Everyone)]
    private void Add_Field_Rpc(ulong fieldId)
    {
        if (!NetCustomUtil.FindSpawned(fieldId, out AvenueCardField field))
        {
            return;
        }
        
        mFieldList.Add(field);
    }
    
    [Rpc(SendTo.Everyone)]
    private void Set_IsMe_Rpc(ulong fieldId, ulong steamId)
    {
        if (!NetCustomUtil.FindSpawned(fieldId, out AvenueCardField field))
        {
            return;
        }
        
        bool isMe = steamId == SteamClient.SteamId;
        field.Set_IsMe(isMe);
    }
    
    [Rpc(SendTo.Everyone)]
    private void Set_Origin_Rpc(Vector3 me, Vector3 other)
    {
        foreach (AvenueCardField field in mFieldList)
        {
            // - set
            field.Set_Origin(field.IsMe ? me : other);
        }
    }
}
