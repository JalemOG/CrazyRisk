using System;

namespace CrazyRisk.UI
{
    public class GameConfiguration
    {
        public string PlayerName { get; set; }
        public ConsoleColor PlayerColor { get; set; }
        public string ServerIP { get; set; }
        public int Port { get; set; }
        public bool IncludeNeutral { get; set; }
        public bool IsServer { get; set; }
    }
}