using System;
using UnityEngine;

[Serializable]
public class AvenueGameContext
{
    [Header("-- Option")] 
    public int initDrawCount = 4;
    
    [Header("-- Origin")]
    public AvenueCardDeck deckOrigin;
    public AvenueCardHand handOrigin;

    [Header("-- Origin Tr")]
    public Transform deckOriginTr;
    public Transform handMeOriginTr;
    public Transform handOtherOriginTr;
    
    [Header("-- References")]
    public AvenueCardHandGroup handGroup;
    
    [Header("-- Runtime")]
    public AvenueCardDeck deck;
}