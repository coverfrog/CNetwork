using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AvenueGameContext
{
    [Header("-- Option")] 
    public int initDrawCount = 4;
    
    [Header("-- References")]
    public Camera mainCamera;
    
    [Header("-- Origin")]
    public AvenueCardDeck deckOrigin;
    public AvenueCardHand handOrigin;
    public AvenueCardHandGroup handGroupOrigin;

    [Header("-- Origin Tr")]
    public Transform deckOriginTr;
    public Transform handMeOriginTr;
    public Transform handOtherOriginTr;
    public Transform handSetOriginTr;
    public Transform deckSetOriginTr;
    
    [Header("-- Runtime")]
    public AvenueCardDeck deck;
    public AvenueCardHandGroup handGroup;
    public bool isMyTurn;
}