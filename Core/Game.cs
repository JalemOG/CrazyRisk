using CrazyRisk.DataStructures;

namespace CrazyRisk.Core
{
    public class Game
    {
        public Map Map { get; set; }
        public LinkedList<Player> Players { get; set; }
        public Deck Deck { get; set; }
        public TurnManager TurnManager { get; set; }
        public int GlobalTradeCounter { get; set; }
        public GameState State { get; set; }
        
        public Game()
        {
            Map = new Map();
            Players = new LinkedList<Player>();
            Deck = new Deck();
            GlobalTradeCounter = 2;
            State = GameState.Setup;
        }
        
        public void StartGame()
        {
            InitializeMap();
            DistributeTerritories();
            State = GameState.Reinforce;
        }
        
        private void InitializeMap()
        {
            // Implementar inicialización del mapa
        }
        
        private void DistributeTerritories()
        {
            // Implementar distribución de territorios
        }
        
        public void NextTurn()
        {
            Player nextPlayer = TurnManager.NextTurn();
            if (nextPlayer != null)
            {
                HandleReinforcements(nextPlayer);
                State = GameState.Reinforce;
            }
        }
        
        public Player CheckVictory()
        {
            IIterator<Player> iterator = Players.GetIterator();
            while (iterator.HasNext())
            {
                Player player = iterator.Next();
                if (player.Territories.Count == Map.Territories.Count)
                    return player;
            }
            return null;
        }
        
        public void HandleAttack(Battle battle)
        {
            CombatResult result = battle.Execute();
            
            battle.FromTerritory.RemoveTroops(result.AttackerLosses);
            battle.ToTerritory.RemoveTroops(result.DefenderLosses);
            
            if (result.TerritoryConquered)
            {
                battle.ToTerritory.ChangeOwner(battle.Attacker);
                Card newCard = Deck.DrawCard();
                battle.Attacker.AddCard(newCard);
            }
        }
        
        public void HandleReinforcements(Player player)
        {
            player.AvailableTroops += player.CalculateReinforcements(this);
        }
        
        public void HandleMovement(Player player, Territory from, Territory to, int units)
        {
            if (from.Owner != player || to.Owner != player || from.Troops <= units)
                return;
                
            from.RemoveTroops(units);
            to.AddTroops(units);
        }
    }
}