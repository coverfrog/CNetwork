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
    public AvenueCardField fieldOrigin;
    public AvenueCardFieldGroup fieldGroupOrigin;
    public AvenueCardSelected cardSelectedOrigin;

    [Header("-- Origin Tr")]
    public Transform deckOriginTr;
    [Space]
    public Transform handMeOriginTr;
    public Transform handOtherOriginTr;
    [Space]
    public Transform fieldMeOriginTr;
    public Transform fieldOtherOriginTr;
    [Space]
    public Transform handSetOriginTr;
    public Transform deckSetOriginTr;
    
    [Header("-- Runtime")]
    public AvenueCardDeck deck;
    public AvenueCardHandGroup handGroup;
    public AvenueCardFieldGroup fieldGroup;
    public AvenueCardSelected selected;
    [Space]
    public bool isHandCardSelect;
}