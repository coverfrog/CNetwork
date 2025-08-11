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
        ulong remainId = context.selected.Get_Remain_Card(selectId);
        
        // - 결국 여기에서 타겟 id를 제대로 선택해줘야함
        ulong myTargetId = context.isHandCardSelect ? remainId : selectId;
        ulong otherTargetId = !context.isHandCardSelect ? remainId : selectId;
        
        context.fieldGroup.MyField.On_Select_Rpc(myTargetId);
        context.fieldGroup.OtherField.On_Select_Rpc(otherTargetId);
    }
}