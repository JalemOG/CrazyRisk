using System;

namespace CrazyRisk.Core
{
    public class NeutralArmy : Player
    {
        public NeutralArmy() : base("Neutral", ConsoleColor.Gray)
        {
        }
        
        public void PlaceTroopsRandomly()
        {
            if (Territories.IsEmpty) return;
            
            Random random = new Random();
            Territory[] territoryArray = Territories.ToArray();
            
            while (AvailableTroops > 0)
            {
                Territory randomTerritory = territoryArray[random.Next(territoryArray.Length)];
                randomTerritory.AddTroops(1);
                AvailableTroops--;
            }
        }
    }
}