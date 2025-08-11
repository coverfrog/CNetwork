using System;
using UnityEngine;

public class AvenueGameHandCardSelect : MonoBehaviour, IAvenueGameState
{
    private AvenueCard _mFocusCard, _mSelectedCard;

    public void OnEnter(AvenueGameHandler handler, AvenueGameContext context)
    {
        _mFocusCard = null;
        _mSelectedCard = null;
    }

    public void OnUpdate(AvenueGameHandler handler, AvenueGameContext context)
    {
        if (!context.isMyTurn || _mSelectedCard)
        {
            return;
        }
        
        Focus(context);
        Select(context);
    }

    public void OnExit(AvenueGameHandler handler, AvenueGameContext context)
    {
        
    }

    private void Focus(AvenueGameContext context)
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = context.mainCamera.ScreenPointToRay(mousePosition);
        
        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            if (!_mFocusCard)
            {
                return;
            }
            
            _mFocusCard.On_UnFocus_Rpc();
            _mFocusCard = null;
            
            
            return;
        }

        if (!hit.collider.transform.parent.TryGetComponent<AvenueCard>(out AvenueCard card))
        {
            return;
        }
        
        _mFocusCard = card;
        
        card.On_Focus_Rpc();
    }

    private void Select(AvenueGameContext context)
    {
        if (!_mFocusCard)
        {
            return;
        }

        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
        
        // - hand
        _mSelectedCard = _mFocusCard;
        
        _mSelectedCard.Set_Select_Rpc(true);
        _mSelectedCard.Set_Position_Tween_Rpc(context.handSetOriginTr.position);
        _mSelectedCard.Set_Scale_Tween_Rpc(Vector3.one * 2.0f);
        
        context.handGroup.MyHand.Spread_Rpc();
        
        // - deck
        AvenueCard deckCard = context.deck.Draw_Client();
        context.deck.Draw_Rpc();
        
        deckCard.Set_Position_Tween_Rpc(context.deckSetOriginTr.position);
        deckCard.Set_Scale_Tween_Rpc(Vector3.one * 2.0f);
    }
}