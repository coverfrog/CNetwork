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
        if (_mSelectedCard)
        {
            return;
        }
        
        Focus();
        Select(context);
    }

    public void OnExit(AvenueGameHandler handler, AvenueGameContext context)
    {
        
    }

    private void Focus()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

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

        if (Input.GetMouseButtonDown(0))
        {
            _mSelectedCard = _mFocusCard;
            _mFocusCard.On_Select_Rpc(context.handSetOriginTr.position);
        }
    }
}