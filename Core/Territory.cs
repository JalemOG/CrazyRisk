using CrazyRisk.DataStructures;

namespace CrazyRisk.Core
{
    public class Territory
    {
        public string Name { get; set; }
        public Player? Owner { get; set; }
        public int Troops { get; set; }
        public CrazyRisk.DataStructures.LinkedList<Territory> AdjacentTerritories { get; set; }
        
        public Territory(string name)
        {
            Name = name;
            AdjacentTerritories = new CrazyRisk.DataStructures.LinkedList<Territory>();
            Troops = 0;
        }
        
        public void AddTroops(int count)
        {
            Troops += count;
        }
        
        public void RemoveTroops(int count)
        {
            Troops = System.Math.Max(0, Troops - count);
        }
        
        public void ChangeOwner(Player newOwner)
        {
            if (Owner != null)
                Owner.RemoveTerritory(this);
            
            Owner = newOwner;
            if (newOwner != null)
                newOwner.AddTerritory(this);
        }
        
        public override string ToString()
        {
            return $"{Name} (Tropas: {Troops})";
        }
    }
}