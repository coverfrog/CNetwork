using Unity.Netcode;
using UnityEngine;

public class AvenueGameCardSelect : MonoBehaviour, IAvenueGameState
{
    private AvenueCard _mFocusCard;
    
    public void OnEnter(AvenueGameHandler handler, AvenueGameContext context)
    {
        
    }

    public void OnUpdate(AvenueGameHandler handler, AvenueGameContext context)
    {
        if (context.isHandCardSelect)
        {
            return;
        }
    }

    public void OnExit(AvenueGameHandler handler, AvenueGameContext context)
    {
        
    }
}