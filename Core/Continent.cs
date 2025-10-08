using CrazyRisk.DataStructures;

namespace CrazyRisk.Core
{
    public class Continent
    {
        public string Name { get; set; }
        public int Bonus { get; set; }
        public CrazyRisk.DataStructures.LinkedList<Territory> Territories { get; set; }
        
        public Continent(string name, int bonus)
        {
            Name = name;
            Bonus = bonus;
            Territories = new CrazyRisk.DataStructures.LinkedList<Territory>();
        }
        
        public bool IsControlledBy(Player player)
        {
            IIterator<Territory> iterator = Territories.GetIterator();
            while (iterator.HasNext())
            {
                if (iterator.Next().Owner != player)
                    return false;
            }
            return true;
        }
    }
}