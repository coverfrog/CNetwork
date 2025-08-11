public enum AvenueGameState
{
    Init,
    HandCardSelect
}

public interface IAvenueGameState
{
    void OnEnter(AvenueGameHandler handler, AvenueGameContext context);
    
    void OnUpdate(AvenueGameHandler handler, AvenueGameContext context);
    
    void OnExit(AvenueGameHandler handler, AvenueGameContext context);
}