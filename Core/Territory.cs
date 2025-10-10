using CrazyRisk.DataStructures;

namespace CrazyRisk.Core
{
    // Core/Territory.cs (fragmento esperado)
    public class Territory
    {
        public string Name { get; set; }
        public Player? Owner { get; set; }
        public int Troops { get; set; }
        public CrazyRisk.DataStructures.LinkedList<Territory> AdjacentTerritories { get; set; }

        public Territory(string name)
        {
            Name = name;
            Troops = 0;
            AdjacentTerritories = new CrazyRisk.DataStructures.LinkedList<Territory>();
        }

        public void AddTroops(int n) => Troops += n;
        public void RemoveTroops(int n) => Troops -= n;
        
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