using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface ICardDataSo 
{
    
}

public abstract class CardDataSo : ScriptableObject, ICardDataSo
{
    
}

// --->

public interface ICardDeckDataSo
{
    
}

public abstract class CardDeckDataSo : ScriptableObject, ICardDeckDataSo
{
   
}

public abstract class CardDeckDataSo<T> : CardDeckDataSo where T : ICardDataSo
{
    [SerializeField] protected List<CountValue<T>> cardDataList;
    
    public IReadOnlyList<CountValue<T>> CardDataList => cardDataList;
}