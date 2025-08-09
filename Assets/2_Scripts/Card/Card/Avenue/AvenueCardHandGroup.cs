using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Readonly = Sirenix.OdinInspector.ReadOnlyAttribute;

public class AvenueCardHandGroup : MonoBehaviour
{
    [SerializeField] private uint mInitDrawCount = 4;
    [Space]
    [SerializeField] private AvenueCardHand mCardHandOrigin;
    [Space] 
    [SerializeField] private Transform mCardHandSpawnPointMe;
    [SerializeField] private Transform mCardHandSpawnPointOther;
    [Space]
    [ShowInInspector, Readonly] private Dictionary<bool, AvenueCardHand> _handDict = new Dictionary<bool, AvenueCardHand>();

    public void Init_Request()
    {
        foreach (ulong id in NetworkManager.Singleton.ConnectedClientsIds)
        {
            bool isMine = id == NetworkManager.Singleton.LocalClientId;
            
            Debug.Log(isMine);
        }
    }
}