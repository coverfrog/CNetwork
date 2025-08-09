using Unity.Netcode;
using UnityEngine;

public class AvenueCard : NetworkBehaviour
{
    [SerializeField] private CardDataSo mCardDataSo;
    
    public AvenueCard Init(CardDataSo cardDataSo)
    {
        mCardDataSo = cardDataSo;
        
        return this;
    }
}