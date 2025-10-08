namespace CrazyRisk.Core
{
    public class Card
    {
        public CardType Type { get; set; }
        public Territory Territory { get; set; } = null!;
        
        public bool CanTradeWith(Card c2, Card c3)
        {
            // Tres del mismo tipo o uno de cada tipo
            return (Type == c2.Type && Type == c3.Type) || 
                   (Type != c2.Type && Type != c3.Type && c2.Type != c3.Type);
        }
        
        public override string ToString()
        {
            return $"{Type} - {Territory.Name}";
        }
    }
}