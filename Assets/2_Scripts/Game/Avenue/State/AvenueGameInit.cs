using UnityEngine;

public class AvenueGameInit : MonoBehaviour, IAvenueGameState
{
    public void OnEnter(AvenueGameHandler handler, AvenueGameContext context)
    {
        // - deck ins init
        context.deck = Instantiate(context.deckOrigin);
        context.deck.Spawn();
        context.deck.Init_Request(context.deckOriginTr.position);
        
        context.handGroup = Instantiate(context.handGroupOrigin);
        context.handGroup.Spawn();
        context.handGroup.Init_Request(context);

        context.selected = Instantiate(context.cardSelectedOrigin);
        context.selected.Spawn();
        
        context.fieldGroup = Instantiate(context.fieldGroupOrigin);
        context.fieldGroup.Spawn();
        context.fieldGroup.Init_Request(context);
        
        context.isHandCardSelect = MultiManager.Instance.IsServer;
        
        handler.StateChange(AvenueGameState.HandCardSelect);
    }

    public void OnUpdate(AvenueGameHandler handler, AvenueGameContext context)
    {
        
    }

    public void OnExit(AvenueGameHandler handler, AvenueGameContext context)
    {
       
    }
}