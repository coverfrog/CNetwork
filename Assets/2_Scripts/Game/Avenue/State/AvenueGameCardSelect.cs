using Unity.Netcode;
using UnityEngine;

public class AvenueGameCardSelect : MonoBehaviour, IAvenueGameState
{
    [SerializeField] private AvenueCard _mFocusCard;
    
    public void OnEnter(AvenueGameHandler handler, AvenueGameContext context)
    {
    }

    public void OnUpdate(AvenueGameHandler handler, AvenueGameContext context)
    {
        if (context.isHandCardSelect)
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

            _mFocusCard = null;

            return;
        }
        
        if (!hit.collider.transform.parent.TryGetComponent<AvenueCard>(out AvenueCard card))
        {
            return;
        }
        
        _mFocusCard = card;
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
        
        // - get
        ulong selectId = _mFocusCard.NetworkObjectId;
        context.fieldGroup.On_Select_Me_Rpc(selectId);
        
        ulong remainId = context.selected.Get_Remain_Card(selectId);
        context.fieldGroup.On_Select_NotMe_Rpc(remainId);
        
        
    }
}