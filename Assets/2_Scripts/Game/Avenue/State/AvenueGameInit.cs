using UnityEngine;

public class AvenueGameInit : MonoBehaviour, IAvenueGameState
{
    public void OnEnter(AvenueGameHandler handler, AvenueGameContext context)
    {
        // - deck ins init
        context.isHandCardSelect = MultiManager.Instance.IsServer;

        if (!handler.IsServer)
        {
            return;
        }
        
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
        
        handler.StateChange(AvenueGameState.HandCardSelect);
    }

    public void OnUpdate(AvenueGameHandler handler, AvenueGameContext context)
    {
        
    }

    public void OnExit(AvenueGameHandler handler, AvenueGameContext context)
    {
        if (handler.IsServer)
        {
            return;
        }

        context.deck       = FindAnyObjectByType<AvenueCardDeck>();
        context.handGroup  = FindAnyObjectByType<AvenueCardHandGroup>();
        context.selected   = FindAnyObjectByType<AvenueCardSelected>();
        context.fieldGroup = FindAnyObjectByType<AvenueCardFieldGroup>();
    }
}