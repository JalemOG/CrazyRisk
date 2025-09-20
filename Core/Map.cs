using CrazyRisk.DataStructures;

namespace CrazyRisk.Core
{
    public class Map
    {
        public LinkedList<Continent> Continents { get; set; }
        public LinkedList<Territory> Territories { get; set; }
        private HashMap<Territory, LinkedList<Territory>> adjacencyList;
        
        public Map()
        {
            Continents = new LinkedList<Continent>();
            Territories = new LinkedList<Territory>();
            adjacencyList = new HashMap<Territory, LinkedList<Territory>>();
        }
        
        public void AddTerritory(Territory territory)
        {
            Territories.Add(territory);
            adjacencyList.Put(territory, new LinkedList<Territory>());
        }
        
        public void AddAdjacency(Territory t1, Territory t2)
        {
            if (!adjacencyList.ContainsKey(t1))
                adjacencyList.Put(t1, new LinkedList<Territory>());
                
            if (!adjacencyList.ContainsKey(t2))
                adjacencyList.Put(t2, new LinkedList<Territory>());
                
            adjacencyList.Get(t1).Add(t2);
            adjacencyList.Get(t2).Add(t1);
        }
        
        public LinkedList<Territory> GetAdjacent(Territory territory)
        {
            return adjacencyList.Get(territory) ?? new LinkedList<Territory>();
        }
        
        public Continent GetContinent(string name)
        {
            IIterator<Continent> iterator = Continents.GetIterator();
            while (iterator.HasNext())
            {
                Continent continent = iterator.Next();
                if (continent.Name == name)
                    return continent;
            }
            return null;
        }
    }
}