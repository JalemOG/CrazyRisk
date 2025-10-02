using CrazyRisk.DataStructures;
using System;

namespace CrazyRisk.Core
{
    public class Map
    {
        public LinkedList<Continent> Continents { get; set; }
        public LinkedList<Territory> Territories { get; set; }
        
        public Map()
        {
            Continents = new LinkedList<Continent>();
            Territories = new LinkedList<Territory>();
        }
        
        // Agregar territorio al mapa
        public void AddTerritory(Territory territory)
        {
            Territories.Add(territory);
        }
        
        // Agregar continente al mapa
        public void AddContinent(Continent continent)
        {
            Continents.Add(continent);
        }
        
        // Agregar adyacencia bidireccional entre territorios
        public void AddAdjacency(Territory t1, Territory t2)
        {
            if (!t1.AdjacentTerritories.Contains(t2))
                t1.AdjacentTerritories.Add(t2);
                
            if (!t2.AdjacentTerritories.Contains(t1))
                t2.AdjacentTerritories.Add(t1);
        }
        
        // Obtener territorios adyacentes
        public LinkedList<Territory> GetAdjacent(Territory territory)
        {
            return territory.AdjacentTerritories;
        }
        
        // Buscar continente por nombre
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
        
        // Buscar territorio por nombre
        public Territory GetTerritory(string name)
        {
            IIterator<Territory> iterator = Territories.GetIterator();
            while (iterator.HasNext())
            {
                Territory territory = iterator.Next();
                if (territory.Name == name)
                    return territory;
            }
            return null;
        }
        
        // Verificar si dos territorios son adyacentes
        public bool AreAdjacent(Territory t1, Territory t2)
        {
            return t1.AdjacentTerritories.Contains(t2);
        }
        
        // Verificar si existe un camino entre dos territorios pasando solo por territorios del mismo jugador
        // Útil para la fase de planeación (movimiento de tropas)
        public bool ExistsPath(Territory origin, Territory destination, Player player)
        {
            if (origin == destination)
                return true;
                
            // BFS para encontrar camino
            HashMap<Territory, bool> visited = new HashMap<Territory, bool>();
            Queue<Territory> queue = new Queue<Territory>();
            
            queue.Enqueue(origin);
            visited.Put(origin, true);
            
            while (!queue.IsEmpty())
            {
                Territory current = queue.Dequeue();
                
                if (current == destination)
                    return true;
                
                IIterator<Territory> iterator = current.AdjacentTerritories.GetIterator();
                
                while (iterator.HasNext())
                {
                    Territory neighbor = iterator.Next();
                    
                    // Solo explorar territorios del mismo jugador
                    if (neighbor.Owner == player && !visited.ContainsKey(neighbor))
                    {
                        visited.Put(neighbor, true);
                        queue.Enqueue(neighbor);
                    }
                }
            }
            
            return false;
        }
        
        // Obtener todos los territorios de un jugador
        public LinkedList<Territory> GetPlayerTerritories(Player player)
        {
            LinkedList<Territory> playerTerritories = new LinkedList<Territory>();
            IIterator<Territory> iterator = Territories.GetIterator();
            
            while (iterator.HasNext())
            {
                Territory territory = iterator.Next();
                if (territory.Owner == player)
                    playerTerritories.Add(territory);
            }
            
            return playerTerritories;
        }
        
        // Calcular bonificación de continentes para un jugador
        public int CalculateContinentBonus(Player player)
        {
            int bonus = 0;
            IIterator<Continent> iterator = Continents.GetIterator();
            
            while (iterator.HasNext())
            {
                Continent continent = iterator.Next();
                if (continent.IsControlledBy(player))
                    bonus += continent.Bonus;
            }
            
            return bonus;
        }
        
        // Contar territorios de un jugador
        public int CountPlayerTerritories(Player player)
        {
            int count = 0;
            IIterator<Territory> iterator = Territories.GetIterator();
            
            while (iterator.HasNext())
            {
                Territory territory = iterator.Next();
                if (territory.Owner == player)
                    count++;
            }
            
            return count;
        }
        
        // Verificar si un jugador ha ganado (controla todos los territorios)
        public bool HasPlayerWon(Player player)
        {
            IIterator<Territory> iterator = Territories.GetIterator();
            
            while (iterator.HasNext())
            {
                Territory territory = iterator.Next();
                if (territory.Owner != player)
                    return false;
            }
            
            return true;
        }
        
        // Distribuir territorios aleatoriamente entre jugadores
        public void DistributeTerritories(LinkedList<Player> players)
        {
            // Convertir territorios a array para facilitar el shuffle
            Territory[] territoryArray = new Territory[Territories.Size()];
            IIterator<Territory> territoryIterator = Territories.GetIterator();
            int index = 0;
            
            while (territoryIterator.HasNext())
            {
                territoryArray[index++] = territoryIterator.Next();
            }
            
            // Mezclar territorios (algoritmo Fisher-Yates)
            Random random = new Random();
            for (int i = territoryArray.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Territory temp = territoryArray[i];
                territoryArray[i] = territoryArray[j];
                territoryArray[j] = temp;
            }
            
            // Distribuir territorios equitativamente
            int playerIndex = 0;
            Player[] playerArray = new Player[players.Size()];
            IIterator<Player> playerIterator = players.GetIterator();
            int pIndex = 0;
            
            while (playerIterator.HasNext())
            {
                playerArray[pIndex++] = playerIterator.Next();
            }
            
            for (int i = 0; i < territoryArray.Length; i++)
            {
                Territory territory = territoryArray[i];
                Player player = playerArray[playerIndex];
                
                territory.ChangeOwner(player);
                territory.AddTroops(1); // Colocar una tropa inicial
                
                playerIndex = (playerIndex + 1) % playerArray.Length;
            }
        }
        
        // Obtener territorios desde los cuales un jugador puede atacar
        public LinkedList<Territory> GetAttackableTerritories(Player player)
        {
            LinkedList<Territory> attackable = new LinkedList<Territory>();
            IIterator<Territory> iterator = Territories.GetIterator();
            
            while (iterator.HasNext())
            {
                Territory territory = iterator.Next();
                if (territory.Owner == player && territory.Troops >= 2)
                    attackable.Add(territory);
            }
            
            return attackable;
        }
        
        // Obtener territorios enemigos adyacentes a un territorio dado
        public LinkedList<Territory> GetAdjacentEnemyTerritories(Territory territory)
        {
            LinkedList<Territory> enemies = new LinkedList<Territory>();
            IIterator<Territory> iterator = territory.AdjacentTerritories.GetIterator();
            
            while (iterator.HasNext())
            {
                Territory adjacent = iterator.Next();
                if (adjacent.Owner != territory.Owner)
                    enemies.Add(adjacent);
            }
            
            return enemies;
        }
        
        // Obtener territorios amigos adyacentes a un territorio dado
        public LinkedList<Territory> GetAdjacentFriendlyTerritories(Territory territory)
        {
            LinkedList<Territory> friends = new LinkedList<Territory>();
            IIterator<Territory> iterator = territory.AdjacentTerritories.GetIterator();
            
            while (iterator.HasNext())
            {
                Territory adjacent = iterator.Next();
                if (adjacent.Owner == territory.Owner)
                    friends.Add(adjacent);
            }
            
            return friends;
        }
        
        // Inicializar el mapa con los datos del juego Risk estándar (42 territorios)
        public void InitializeStandardMap()
        {
            // Crear continentes
            Continent asia = new Continent("Asia", 7);
            Continent northAmerica = new Continent("América del Norte", 5);
            Continent europe = new Continent("Europa", 5);
            Continent africa = new Continent("África", 3);
            Continent southAmerica = new Continent("América del Sur", 2);
            Continent oceania = new Continent("Oceanía", 2);
            
            AddContinent(asia);
            AddContinent(northAmerica);
            AddContinent(europe);
            AddContinent(africa);
            AddContinent(southAmerica);
            AddContinent(oceania);
            
            // ============ ASIA (12 territorios) ============
            Territory kamchatka = new Territory("Kamchatka");
            Territory japan = new Territory("Japón");
            Territory mongolia = new Territory("Mongolia");
            Territory china = new Territory("China");
            Territory siam = new Territory("Siam");
            Territory india = new Territory("India");
            Territory middleEast = new Territory("Medio Oriente");
            Territory afghanistan = new Territory("Afganistán");
            Territory ural = new Territory("Ural");
            Territory siberia = new Territory("Siberia");
            Territory yakutsk = new Territory("Yakutsk");
            Territory irkutsk = new Territory("Irkutsk");
            
            asia.Territories.Add(kamchatka);
            asia.Territories.Add(japan);
            asia.Territories.Add(mongolia);
            asia.Territories.Add(china);
            asia.Territories.Add(siam);
            asia.Territories.Add(india);
            asia.Territories.Add(middleEast);
            asia.Territories.Add(afghanistan);
            asia.Territories.Add(ural);
            asia.Territories.Add(siberia);
            asia.Territories.Add(yakutsk);
            asia.Territories.Add(irkutsk);
            
            AddTerritory(kamchatka);
            AddTerritory(japan);
            AddTerritory(mongolia);
            AddTerritory(china);
            AddTerritory(siam);
            AddTerritory(india);
            AddTerritory(middleEast);
            AddTerritory(afghanistan);
            AddTerritory(ural);
            AddTerritory(siberia);
            AddTerritory(yakutsk);
            AddTerritory(irkutsk);
            
            // ============ AMÉRICA DEL NORTE (9 territorios) ============
            Territory alaska = new Territory("Alaska");
            Territory alberta = new Territory("Alberta");
            Territory ontario = new Territory("Ontario");
            Territory quebec = new Territory("Quebec");
            Territory greenland = new Territory("Groenlandia");
            Territory westernUS = new Territory("Estados Unidos Occidental");
            Territory easternUS = new Territory("Estados Unidos Oriental");
            Territory centralAmerica = new Territory("América Central");
            Territory northwest = new Territory("Territorio del Noroeste");
            
            northAmerica.Territories.Add(alaska);
            northAmerica.Territories.Add(alberta);
            northAmerica.Territories.Add(ontario);
            northAmerica.Territories.Add(quebec);
            northAmerica.Territories.Add(greenland);
            northAmerica.Territories.Add(westernUS);
            northAmerica.Territories.Add(easternUS);
            northAmerica.Territories.Add(centralAmerica);
            northAmerica.Territories.Add(northwest);
            
            AddTerritory(alaska);
            AddTerritory(alberta);
            AddTerritory(ontario);
            AddTerritory(quebec);
            AddTerritory(greenland);
            AddTerritory(westernUS);
            AddTerritory(easternUS);
            AddTerritory(centralAmerica);
            AddTerritory(northwest);
            
            // ============ EUROPA (7 territorios) ============
            Territory iceland = new Territory("Islandia");
            Territory scandinavia = new Territory("Escandinavia");
            Territory ukraine = new Territory("Ucrania");
            Territory northernEurope = new Territory("Europa del Norte");
            Territory westernEurope = new Territory("Europa Occidental");
            Territory southernEurope = new Territory("Europa del Sur");
            Territory greatBritain = new Territory("Gran Bretaña");
            
            europe.Territories.Add(iceland);
            europe.Territories.Add(scandinavia);
            europe.Territories.Add(ukraine);
            europe.Territories.Add(northernEurope);
            europe.Territories.Add(westernEurope);
            europe.Territories.Add(southernEurope);
            europe.Territories.Add(greatBritain);
            
            AddTerritory(iceland);
            AddTerritory(scandinavia);
            AddTerritory(ukraine);
            AddTerritory(northernEurope);
            AddTerritory(westernEurope);
            AddTerritory(southernEurope);
            AddTerritory(greatBritain);
            
            // ============ ÁFRICA (6 territorios) ============
            Territory northAfrica = new Territory("África del Norte");
            Territory egypt = new Territory("Egipto");
            Territory eastAfrica = new Territory("África Oriental");
            Territory congo = new Territory("Congo");
            Territory southAfrica = new Territory("Sudáfrica");
            Territory madagascar = new Territory("Madagascar");
            
            africa.Territories.Add(northAfrica);
            africa.Territories.Add(egypt);
            africa.Territories.Add(eastAfrica);
            africa.Territories.Add(congo);
            africa.Territories.Add(southAfrica);
            africa.Territories.Add(madagascar);
            
            AddTerritory(northAfrica);
            AddTerritory(egypt);
            AddTerritory(eastAfrica);
            AddTerritory(congo);
            AddTerritory(southAfrica);
            AddTerritory(madagascar);
            
            // ============ AMÉRICA DEL SUR (4 territorios) ============
            Territory venezuela = new Territory("Venezuela");
            Territory peru = new Territory("Perú");
            Territory brazil = new Territory("Brasil");
            Territory argentina = new Territory("Argentina");
            
            southAmerica.Territories.Add(venezuela);
            southAmerica.Territories.Add(peru);
            southAmerica.Territories.Add(brazil);
            southAmerica.Territories.Add(argentina);
            
            AddTerritory(venezuela);
            AddTerritory(peru);
            AddTerritory(brazil);
            AddTerritory(argentina);
            
            // ============ OCEANÍA (4 territorios) ============
            Territory indonesia = new Territory("Indonesia");
            Territory newGuinea = new Territory("Nueva Guinea");
            Territory westernAustralia = new Territory("Australia Occidental");
            Territory easternAustralia = new Territory("Australia Oriental");
            
            oceania.Territories.Add(indonesia);
            oceania.Territories.Add(newGuinea);
            oceania.Territories.Add(westernAustralia);
            oceania.Territories.Add(easternAustralia);
            
            AddTerritory(indonesia);
            AddTerritory(newGuinea);
            AddTerritory(westernAustralia);
            AddTerritory(easternAustralia);
            
            // ============ DEFINIR ADYACENCIAS ============
            
            // === ADYACENCIAS DE ASIA ===
            AddAdjacency(kamchatka, yakutsk);
            AddAdjacency(kamchatka, irkutsk);
            AddAdjacency(kamchatka, mongolia);
            AddAdjacency(kamchatka, japan);
            AddAdjacency(kamchatka, alaska); // Ruta marítima
            
            AddAdjacency(yakutsk, siberia);
            AddAdjacency(yakutsk, irkutsk);
            
            AddAdjacency(irkutsk, siberia);
            AddAdjacency(irkutsk, mongolia);
            
            AddAdjacency(siberia, ural);
            AddAdjacency(siberia, china);
            AddAdjacency(siberia, mongolia);
            
            AddAdjacency(mongolia, china);
            AddAdjacency(mongolia, japan);
            
            AddAdjacency(china, ural);
            AddAdjacency(china, afghanistan);
            AddAdjacency(china, india);
            AddAdjacency(china, siam);
            
            AddAdjacency(ural, ukraine);
            AddAdjacency(ural, afghanistan);
            
            AddAdjacency(afghanistan, ukraine);
            AddAdjacency(afghanistan, middleEast);
            AddAdjacency(afghanistan, india);
            
            AddAdjacency(middleEast, ukraine);
            AddAdjacency(middleEast, southernEurope);
            AddAdjacency(middleEast, egypt);
            AddAdjacency(middleEast, eastAfrica);
            AddAdjacency(middleEast, india);
            
            AddAdjacency(india, siam);
            
            AddAdjacency(siam, indonesia);
            
            // === ADYACENCIAS DE AMÉRICA DEL NORTE ===
            AddAdjacency(alaska, northwest);
            AddAdjacency(alaska, alberta);
            
            AddAdjacency(northwest, alberta);
            AddAdjacency(northwest, ontario);
            AddAdjacency(northwest, greenland);
            
            AddAdjacency(greenland, ontario);
            AddAdjacency(greenland, quebec);
            AddAdjacency(greenland, iceland); // Ruta marítima
            
            AddAdjacency(alberta, ontario);
            AddAdjacency(alberta, westernUS);
            
            AddAdjacency(ontario, quebec);
            AddAdjacency(ontario, westernUS);
            AddAdjacency(ontario, easternUS);
            
            AddAdjacency(quebec, easternUS);
            
            AddAdjacency(westernUS, easternUS);
            AddAdjacency(westernUS, centralAmerica);
            
            AddAdjacency(easternUS, centralAmerica);
            
            AddAdjacency(centralAmerica, venezuela);
            
            // === ADYACENCIAS DE EUROPA ===
            AddAdjacency(iceland, greatBritain);
            AddAdjacency(iceland, scandinavia);
            
            AddAdjacency(greatBritain, scandinavia);
            AddAdjacency(greatBritain, northernEurope);
            AddAdjacency(greatBritain, westernEurope);
            
            AddAdjacency(scandinavia, ukraine);
            AddAdjacency(scandinavia, northernEurope);
            
            AddAdjacency(ukraine, northernEurope);
            AddAdjacency(ukraine, southernEurope);
            
            AddAdjacency(northernEurope, westernEurope);
            AddAdjacency(northernEurope, southernEurope);
            
            AddAdjacency(westernEurope, southernEurope);
            AddAdjacency(westernEurope, northAfrica);
            
            AddAdjacency(southernEurope, northAfrica);
            AddAdjacency(southernEurope, egypt);
            
            // === ADYACENCIAS DE ÁFRICA ===
            AddAdjacency(northAfrica, egypt);
            AddAdjacency(northAfrica, eastAfrica);
            AddAdjacency(northAfrica, congo);
            
            AddAdjacency(egypt, eastAfrica);
            
            AddAdjacency(eastAfrica, congo);
            AddAdjacency(eastAfrica, southAfrica);
            AddAdjacency(eastAfrica, madagascar);
            
            AddAdjacency(congo, southAfrica);
            
            AddAdjacency(southAfrica, madagascar);
            
            // === ADYACENCIAS DE AMÉRICA DEL SUR ===
            AddAdjacency(venezuela, peru);
            AddAdjacency(venezuela, brazil);
            
            AddAdjacency(peru, brazil);
            AddAdjacency(peru, argentina);
            
            AddAdjacency(brazil, argentina);
            AddAdjacency(brazil, northAfrica); // Ruta marítima
            
            // === ADYACENCIAS DE OCEANÍA ===
            AddAdjacency(indonesia, newGuinea);
            AddAdjacency(indonesia, westernAustralia);
            
            AddAdjacency(newGuinea, westernAustralia);
            AddAdjacency(newGuinea, easternAustralia);
            
            AddAdjacency(westernAustralia, easternAustralia);
        }
    }
}