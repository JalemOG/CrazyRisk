using CrazyRisk.DataStructures;

namespace CrazyRisk.Core
{
    public class Map
    {
        public CrazyRisk.DataStructures.LinkedList<Continent> Continents { get; set; }
            = new CrazyRisk.DataStructures.LinkedList<Continent>();

        public CrazyRisk.DataStructures.LinkedList<Territory> Territories { get; set; }
            = new CrazyRisk.DataStructures.LinkedList<Territory>();

        public Map() { }

        public CrazyRisk.DataStructures.LinkedList<Territory> GetAdjacent(Territory territory)
        {
            var result = new CrazyRisk.DataStructures.LinkedList<Territory>();
            var it = territory.AdjacentTerritories.GetIterator();
            while (it.HasNext()) result.Add(it.Next());
            return result;
        }

        /// Reparto *sin* neutral (round-robin entre jugadores)
        public void DistributeTerritories(CrazyRisk.DataStructures.LinkedList<Player> players)
        {
            int pcount = players.Count;
            if (pcount == 0 || Territories.Count == 0) return;

            int idx = 0;
            var tit = Territories.GetIterator();
            while (tit.HasNext())
            {
                var terr = tit.Next();

                var pit = players.GetIterator();
                Player chosen = null!;
                int k = 0;
                while (pit.HasNext())
                {
                    var p = pit.Next();
                    if (k == (idx % pcount)) { chosen = p; break; }
                    k++;
                }

                terr.Owner = chosen;
                if (terr.Troops <= 0) terr.Troops = 1;
                chosen.Territories.Add(terr);

                idx++;
            }
        }

        /// Reparto *con* neutral: cada `neutralEvery` territorios para `neutral`,
        /// el resto se asigna round-robin a los jugadores activos.
        public void DistributeWithNeutral(
            CrazyRisk.DataStructures.LinkedList<Player> players,
            Player neutral,
            int neutralEvery = 3,
            int neutralTroops = 2)
        {
            int pcount = players.Count;
            if (pcount == 0 || Territories.Count == 0) return;
            if (neutralEvery < 2) neutralEvery = 2; // evita dar demasiados al neutral

            int idx = 0;
            var tit = Territories.GetIterator();
            while (tit.HasNext())
            {
                var terr = tit.Next();

                if ((idx + 1) % neutralEvery == 0)
                {
                    terr.Owner = neutral;
                    terr.Troops = neutralTroops;
                    neutral.Territories.Add(terr);
                }
                else
                {
                    var pit = players.GetIterator();
                    Player chosen = null!;
                    int k = 0;
                    while (pit.HasNext())
                    {
                        var p = pit.Next();
                        if (k == (idx % pcount)) { chosen = p; break; }
                        k++;
                    }

                    terr.Owner = chosen;
                    if (terr.Troops <= 0) terr.Troops = 1;
                    chosen.Territories.Add(terr);
                }

                idx++;
            }
        }

        // ------- Fallback de mapa (ya lo tenías/te lo pasé) -------
        public void BuildDemoMap()
        {
            this.Territories = new CrazyRisk.DataStructures.LinkedList<Territory>();
            this.Continents  = new CrazyRisk.DataStructures.LinkedList<Continent>();

            var demo = new Continent("Demo", 5);
            demo.Territories = new CrazyRisk.DataStructures.LinkedList<Territory>();
            this.Continents.Add(demo);

            var nodes = new Territory[12];
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new Territory($"T{i + 1}");
                if (nodes[i].AdjacentTerritories == null)
                    nodes[i].AdjacentTerritories = new CrazyRisk.DataStructures.LinkedList<Territory>();
                this.Territories.Add(nodes[i]);
                demo.Territories.Add(nodes[i]);
            }

            void Link(Territory a, Territory b)
            {
                if (!a.AdjacentTerritories.Contains(b)) a.AdjacentTerritories.Add(b);
                if (!b.AdjacentTerritories.Contains(a)) b.AdjacentTerritories.Add(a);
            }

            int cols = 4, rows = 3;
            for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
            {
                int i = r * cols + c;
                if (c + 1 < cols) Link(nodes[i], nodes[i + 1]);     // derecha
                if (r + 1 < rows) Link(nodes[i], nodes[i + cols]);  // abajo
            }

            Link(nodes[1], nodes[6]);
            Link(nodes[3], nodes[8]);
            Link(nodes[5], nodes[10]);
        }
    }
}
