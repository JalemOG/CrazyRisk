using System;

namespace CrazyRisk.UI
{
    public class GameConfiguration
    {
        public string PlayerName { get; set; } = string.Empty;
        public ConsoleColor PlayerColor { get; set; } 
        public string ServerIP { get; set; } = string.Empty;
        public int Port { get; set; } = 7777;
        public bool IncludeNeutral { get; set; }
        public bool IsServer { get; set; }
    }
}
