using Unity.Netcode;

public interface ICardDeck
{
    ICardDeck Init();
}

public abstract class CardDeck : NetworkBehaviour, ICardDeck
{
    public abstract ICardDeck Init();
}