using CrazyRisk.DataStructures;

namespace CrazyRisk.Core
{
    public class Game
    {
        // Usa tu LinkedList explícita para evitar ambigüedades
        public CrazyRisk.DataStructures.LinkedList<Player> Players { get; }
            = new CrazyRisk.DataStructures.LinkedList<Player>();

        public Deck Deck { get; } = new Deck();
        public Map  Map  { get; } = new Map();

        public TurnManager? TurnManager { get; private set; }
        public GameState    State       { get; private set; } = GameState.Setup;

        public Player? CurrentPlayer => TurnManager?.CurrentPlayer;

        public void StartGame()
        {
            // ⚠️ SI tu Map tiene un método de inicialización propio, úsalo aquí.
            // Ejemplo (solo si existe): Map.InitializeMap();

            // Reparte territorios entre jugadores
            Map.DistributeTerritories(Players);

            // Crea gestor de turnos
            TurnManager = new TurnManager(Players);

            // Estado inicial del ciclo de turnos
            State = GameState.Reinforce;
        }

        public Player? NextTurn()
        {
            if (TurnManager is null) return null;
            var next = TurnManager.NextTurn();
            if (next is not null) State = GameState.Reinforce;
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
            if (from.Owner != player || to.Owner != player || from.Troops <= units) return;
            from.RemoveTroops(units);
            to.AddTroops(units);
        }
    }
}
