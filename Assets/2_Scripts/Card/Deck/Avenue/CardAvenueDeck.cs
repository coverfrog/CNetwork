using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CardAvenueDeck : CardDeck
{
    [SerializeField] private AvenueCard mCardOrigin;
    
    [Header("So")]
    [SerializeField] private CardAvenueDeckDataSo mDeckDataSo;
    
    [Header("Debug")]
    [SerializeField] private List<CardDataSo> mCardDataSoList;
    
    public override ICardDeck Init()
    {
        Init_Deck_Server_Rpc();
        
        return this;
    }

    [Rpc(SendTo.Server)]
    private void Init_Deck_Server_Rpc()
    {
        // - init data
        mCardDataSoList = new List<CardDataSo>();
        foreach (CountValue<CardDataSo> countValue in mDeckDataSo.CardDataList)
        {
            for (int i = 0; i < countValue.count; i++)
            {
                mCardDataSoList.Add(countValue.value);
            }
        }
        
        // - shuffle
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < mCardDataSoList.Count; j++)
            {
                int rand = Random.Range(0, mCardDataSoList.Count);

                (mCardDataSoList[j], mCardDataSoList[rand]) = (mCardDataSoList[rand], mCardDataSoList[j]);
            }
        }
        
        // - ins
        for (int i = 0; i < mCardDataSoList.Count; i++)
        {
            CardDataSo cardDataSo = mCardDataSoList[i];

            AvenueCard card = Instantiate(mCardOrigin.Init(cardDataSo));
            card.GetComponent<NetworkObject>().Spawn();
        }
    }
}