using System;

namespace CrazyRisk.Core
{
    /// Jugador “neutral” que puede poseer territorios pero no toma turnos.
    public class NeutralArmy : Player
    {
        public NeutralArmy() : base("Neutral", ConsoleColor.DarkGray) { }
        // No override: los refuerzos del neutral se bloquean desde Game.HandleReinforcements(...)
    }
}