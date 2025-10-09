using CrazyRisk.DataStructures;

namespace CrazyRisk.Core
{
    public class Game
    {
        // Usa tu LinkedList propia para evitar ambigüedad con System.Collections.Generic
        public CrazyRisk.DataStructures.LinkedList<Player> Players { get; }
            = new CrazyRisk.DataStructures.LinkedList<Player>();

        public Deck Deck { get; } = new Deck();
        public Map  Map  { get; } = new Map();

        public TurnManager? TurnManager { get; private set; }
        public GameState    State       { get; private set; } = GameState.Setup;

        public Player? CurrentPlayer => TurnManager?.CurrentPlayer;

        public void StartGame()
        {
            // Si tienes inicialización propia del mapa, llámala aquí
            // (por ejemplo: Map.InitializeMap(); )
            Map.DistributeTerritories(Players);
            TurnManager = new TurnManager(Players);
            State = GameState.Reinforce;
        }

        // Punto único para cambiar la fase desde la UI
        public void SetPhase(GameState newState)
        {
            State = newState;
        }

        public Player? NextTurn()
        {
            if (TurnManager == null) return null;

            var next = TurnManager.NextTurn();
            if (next != null)
            {
                State = GameState.Reinforce;
            }
            return next;
        }

        public Player? CheckVictory()
        {
            var it = Players.GetIterator();
            while (it.HasNext())
            {
                var p = it.Next();
                if (p.Territories.Count == Map.Territories.Count)
                    return p;
            }
            return null;
        }

        public void HandleReinforcements(Player player)
        {
            player.AvailableTroops += player.CalculateReinforcements(this);
        }

        public void HandleMovement(Player player, Territory from, Territory to, int units)
        {
            if (from.Owner != player || to.Owner != player) return;
            if (units <= 0 || from.Troops <= units) return;

            from.RemoveTroops(units);
            to.AddTroops(units);
        }
    }
}