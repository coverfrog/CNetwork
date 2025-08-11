using System;
using Unity.Netcode;
using UnityEngine;

public class AvenueGameHandCardSelect : MonoBehaviour, IAvenueGameState
{
    private AvenueCard _mFocusCard;
    private AvenueCard _mHandSelectedCard, _mDeckSelectedCard;

    public void OnEnter(AvenueGameHandler handler, AvenueGameContext context)
    {
        _mFocusCard = null;
        _mHandSelectedCard = null;
    }

    public void OnUpdate(AvenueGameHandler handler, AvenueGameContext context)
    {
        if (!context.isHandCardSelect || _mHandSelectedCard)
        {
            return;
        }

        Focus(context);
        Select(handler, context);
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

    private void Select(AvenueGameHandler handler, AvenueGameContext context)
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
        _mHandSelectedCard = _mFocusCard;

        _mHandSelectedCard.Set_Select_Rpc(true);
        _mHandSelectedCard.Set_Position_Tween_Rpc(context.handSetOriginTr.position);
        _mHandSelectedCard.Set_Scale_Tween_Rpc(Vector3.one * 2.0f);

        context.handGroup.MyHand.Spread_Rpc();

        // - deck
        _mDeckSelectedCard = context.deck.Draw_Client();
        context.deck.Draw_Rpc();

        _mDeckSelectedCard.Set_Select_Rpc(true);
        _mDeckSelectedCard.Set_Position_Tween_Rpc(context.deckSetOriginTr.position);
        _mDeckSelectedCard.Set_Scale_Tween_Rpc(Vector3.one * 2.0f);
        
        // - report
        context.selected.Set_Deck_Selected_Rpc(_mDeckSelectedCard.NetworkObjectId);
        context.selected.Set_Hand_Selected_Rpc(_mHandSelectedCard.NetworkObjectId);
        
        // - change
        handler.StateChange(AvenueGameState.CardSelect);
    }
}