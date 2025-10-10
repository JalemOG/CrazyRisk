using CrazyRisk.DataStructures;

namespace CrazyRisk.Core
{
    public class Game
    {
        // Jugadores activos
        public CrazyRisk.DataStructures.LinkedList<Player> Players { get; }
            = new CrazyRisk.DataStructures.LinkedList<Player>();

        // Neutral (no rota turnos)
        public NeutralArmy Neutral { get; } = new NeutralArmy();

        // Activa/Desactiva ejército neutral en el reparto
        public bool UseNeutral { get; set; } = true;  // ← cámbialo a false si no quieres neutral

        public Deck Deck { get; } = new Deck();
        public Map Map { get; } = new Map();

        public TurnManager? TurnManager { get; private set; }
        public GameState State { get; private set; } = GameState.Setup;

        public Player? CurrentPlayer => TurnManager?.CurrentPlayer;

        public void StartGame()
        {
            // Si no hay mapa, usa demo
            if (Map.Territories.Count == 0)
                Map.BuildDemoMap();

            // Reparto de territorios: con o sin neutral
            if (UseNeutral)
            {
                // cada 3 territorios uno para Neutral, con 2 tropas iniciales
                Map.DistributeWithNeutral(Players, Neutral, neutralEvery: 3, neutralTroops: 2);
            }
            else
            {
                Map.DistributeTerritories(Players);
            }

            TurnManager = new TurnManager(Players); // ← NO incluye al neutral
            State = GameState.Reinforce;
        }

        public void SetPhase(GameState newState) => State = newState;

        public Player? NextTurn()
        {
            if (TurnManager == null) return null;
            var next = TurnManager.NextTurn();
            if (next != null) State = GameState.Reinforce;
            return next;
        }

        public Player? CheckVictory()
        {
            // Gana si posee TODOS los territorios (incluidos los que tenía el neutral)
            var it = Players.GetIterator();
            while (it.HasNext())
            {
                var p = it.Next();
                if (p.Territories.Count == Map.Territories.Count) return p;
            }
            return null;
        }

        public void HandleReinforcements(Player player)
        {
            if (player is NeutralArmy) return; // ← neutral no recibe refuerzos
            player.AvailableTroops += player.CalculateReinforcements(this);
        }
    }
}
