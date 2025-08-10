public enum AvenueGameState
{
    Init,
    TurnBegin
}

public interface IAvenueGameState
{
    void OnEnter(AvenueGameHandler handler, AvenueGameContext context);
    
    void OnExit(AvenueGameHandler handler, AvenueGameContext context);
}