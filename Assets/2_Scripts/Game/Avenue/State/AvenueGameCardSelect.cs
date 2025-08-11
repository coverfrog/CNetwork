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
        Debug.Log("Focus");
        
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
        ulong id = _mFocusCard.NetworkObjectId;
        context.fieldGroup.MyField.Add_Card_Rpc(id);
    }
}