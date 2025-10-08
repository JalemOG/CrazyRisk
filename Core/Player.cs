using CrazyRisk.DataStructures;

namespace CrazyRisk.Core
{
    public class Player
    {
        public string Alias { get; set; }
        public System.ConsoleColor Color { get; set; }
        public CrazyRisk.DataStructures.LinkedList<Territory> Territories { get; set; }
        public CrazyRisk.DataStructures.LinkedList<Card>      Cards       { get; set; }
        public int AvailableTroops { get; set; }
        
        public Player(string alias, System.ConsoleColor color)
        {
            Alias = alias;
            Color = color;
            Territories = new CrazyRisk.DataStructures.LinkedList<Territory>();
            Cards = new CrazyRisk.DataStructures.LinkedList<Card>();
            AvailableTroops = 0;
        }
        
        public void AddTerritory(Territory territory)
        {
            Territories.Add(territory);
        }
        
        public void RemoveTerritory(Territory territory)
        {
            Territories.Remove(territory);
        }
        
        public void AddCard(Card card)
        {
            Cards.Add(card);
        }
        
        public int TradeCards(Card card1, Card card2, Card card3)
        {
            if (!card1.CanTradeWith(card2, card3))
                return 0;
                
            // Remover las cartas
            Cards.Remove(card1);
            Cards.Remove(card2);
            Cards.Remove(card3);
            
            // Calcular tropas según la serie de Fibonacci
            return 5; // Valor temporal
        }
        
        public int CalculateReinforcements(Game game)
        {
            int reinforcements = System.Math.Max(3, Territories.Count / 3);
            
            // Bonificación por continentes
            IIterator<Continent> continents = game.Map.Continents.GetIterator();
            while (continents.HasNext())
            {
                Continent continent = continents.Next();
                if (continent.IsControlledBy(this))
                {
                    reinforcements += continent.Bonus;
                }
            }
            
            return reinforcements;
        }
        
        public override string ToString()
        {
            return $"{Alias} (Territorios: {Territories.Count}, Tropas: {AvailableTroops})";
        }
    }
}