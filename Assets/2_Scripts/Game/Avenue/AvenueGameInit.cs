using UnityEngine;

public class AvenueGameInit : MonoBehaviour, IAvenueGameState
{
    public void OnEnter(AvenueGameHandler handler, AvenueGameContext context)
    {
        // - deck ins init
        
        context.deck = Instantiate(context.deckOrigin);
        context.deck.Spawn();
        context.deck.Init_Request(context.deckOriginTr.position);
        
        handler.StateChange(AvenueGameState.TurnBegin);
    }

    public void OnExit(AvenueGameHandler handler, AvenueGameContext context)
    {
       
    }
}