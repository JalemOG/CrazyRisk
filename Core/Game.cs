using System;

namespace CrazyRisk.Core
{
    /// <summary>
    /// Orquestador principal del juego.
    /// </summary>
    public class Game
    {
        // Usa tu LinkedList explícitamente para evitar ambigüedad
        public CrazyRisk.DataStructures.LinkedList<Player> Players { get; }
            = new CrazyRisk.DataStructures.LinkedList<Player>();

        public Map  Map  { get; } = new Map();
        public Deck Deck { get; } = new Deck();

        public TurnManager? TurnManager { get; private set; }

        public Player? CurrentPlayer => TurnManager?.CurrentPlayer;

        public int GlobalTradeCounter { get; private set; } = 0;

        public void StartGame()
        {
            if (Players.IsEmpty)
                throw new InvalidOperationException("Debe haber al menos un jugador para iniciar el juego.");

            Map.DistributeTerritories(Players);
            TurnManager = new TurnManager(Players);

            // Si tu Deck tiene barajado, descomenta:
            // Deck.Shuffle();
        }

        public Player? NextTurn()
        {
            if (TurnManager is null)
                throw new InvalidOperationException("El juego no ha sido iniciado. Llama a StartGame primero.");

            return TurnManager.NextTurn();
        }

        public void RegisterGlobalTrade()
        {
            checked { GlobalTradeCounter++; }
        }
    }
}