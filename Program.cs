using System;
using System.Windows.Forms;
using CrazyRisk.Core;
using CrazyRisk.DataStructures;

namespace CrazyRisk
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación Crazy Risk.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Si se pasan argumentos, ejecutar en modo consola (para testing)
            if (args.Length > 0 && args[0] == "--console")
            {
                RunConsoleMode();
                return;
            }
            
            // Modo GUI (por defecto)
            RunGUIMode();
        }
        
        /// <summary>
        /// Ejecutar en modo GUI con Windows Forms
        /// </summary>
        static void RunGUIMode()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                Application.Run(new UI.MainMenuForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al iniciar la aplicación:\n{ex.Message}",
                    "Error Crítico",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        
        /// <summary>
        /// Ejecutar en modo consola (para pruebas y debugging)
        /// </summary>
        static void RunConsoleMode()
        {
            Console.WriteLine("=================================");
            Console.WriteLine("    CRAZY RISK - MODO CONSOLA    ");
            Console.WriteLine("=================================\n");
            
            // Prueba de estructuras de datos
            TestDataStructures();
            
            // Prueba del mapa
            TestMap();
            
            // Prueba del juego básico
            TestGame();
            
            Console.WriteLine("\n=================================");
            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
        }
        
        /// <summary>
        /// Probar estructuras de datos básicas
        /// </summary>
        static void TestDataStructures()
        {
            Console.WriteLine("--- PRUEBA DE ESTRUCTURAS DE DATOS ---\n");
            
            // Prueba LinkedList
            Console.WriteLine("Probando LinkedList:");
            LinkedList<string> players = new LinkedList<string>();
            players.Add("Jugador1");
            players.Add("Jugador2");
            players.Add("Jugador3");
            
            IIterator<string> iterator = players.GetIterator();
            while (iterator.HasNext())
            {
                Console.WriteLine($"  - {iterator.Next()}");
            }
            Console.WriteLine($"Total: {players.Size()} jugadores\n");
            
            // Prueba Queue
            Console.WriteLine("Probando Queue:");
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            
            while (!queue.IsEmpty())
            {
                Console.WriteLine($"  Dequeue: {queue.Dequeue()}");
            }
            Console.WriteLine();
            
            // Prueba Stack
            Console.WriteLine("Probando Stack:");
            Stack<string> stack = new Stack<string>();
            stack.Push("Primero");
            stack.Push("Segundo");
            stack.Push("Tercero");
            
            while (!stack.IsEmpty())
            {
                Console.WriteLine($"  Pop: {stack.Pop()}");
            }
            Console.WriteLine();
            
            // Prueba HashMap
            Console.WriteLine("Probando HashMap:");
            HashMap<string, int> scores = new HashMap<string, int>();
            scores.Put("Alice", 100);
            scores.Put("Bob", 200);
            scores.Put("Charlie", 150);
            
            Console.WriteLine($"  Score de Alice: {scores.Get("Alice")}");
            Console.WriteLine($"  Score de Bob: {scores.Get("Bob")}");
            Console.WriteLine($"  ¿Contiene Charlie?: {scores.ContainsKey("Charlie")}");
            Console.WriteLine();
        }
        
        /// <summary>
        /// Probar inicialización del mapa
        /// </summary>
        static void TestMap()
        {
            Console.WriteLine("--- PRUEBA DEL MAPA ---\n");
            
            Map map = new Map();
            map.InitializeStandardMap();
            
            Console.WriteLine($"Territorios totales: {map.Territories.Size()}");
            Console.WriteLine($"Continentes totales: {map.Continents.Size()}\n");
            
            // Mostrar continentes
            Console.WriteLine("Continentes:");
            IIterator<Continent> contIterator = map.Continents.GetIterator();
            while (contIterator.HasNext())
            {
                Continent continent = contIterator.Next();
                Console.WriteLine($"  - {continent.Name}: {continent.Territories.Size()} territorios, Bonus: {continent.Bonus}");
            }
            Console.WriteLine();
            
            // Probar adyacencias
            Territory kamchatka = map.GetTerritory("Kamchatka");
            Territory alaska = map.GetTerritory("Alaska");
            
            if (kamchatka != null && alaska != null)
            {
                bool adjacent = map.AreAdjacent(kamchatka, alaska);
                Console.WriteLine($"¿Kamchatka y Alaska son adyacentes? {adjacent}");
            }
            Console.WriteLine();
        }
        
        /// <summary>
        /// Probar inicialización básica del juego
        /// </summary>
        static void TestGame()
        {
            Console.WriteLine("--- PRUEBA DEL JUEGO ---\n");
            
            try
            {
                Game game = new Game();
                game.Initialize("Alice", ConsoleColor.Red, "Bob", ConsoleColor.Blue, true);
                
                Console.WriteLine("Juego inicializado correctamente");
                Console.WriteLine($"Estado del juego: {game.State}");
                Console.WriteLine($"Jugadores: {game.Players.Size()}");
                Console.WriteLine($"Territorios en el mapa: {game.Map.Territories.Size()}");
                Console.WriteLine($"Cartas en el mazo: {game.Deck.RemainingCards()}\n");
                
                // Mostrar información de jugadores
                IIterator<Player> playerIterator = game.Players.GetIterator();
                int playerNum = 1;
                
                while (playerIterator.HasNext())
                {
                    Player player = playerIterator.Next();
                    int territories = game.Map.CountPlayerTerritories(player);
                    
                    Console.WriteLine($"Jugador {playerNum}: {player.Alias}");
                    Console.WriteLine($"  Color: {player.Color}");
                    Console.WriteLine($"  Territorios: {territories}");
                    Console.WriteLine($"  Tropas disponibles: {player.AvailableTroops}");
                    Console.WriteLine();
                    
                    playerNum++;
                }
                
                // Probar cálculo de Fibonacci
                Console.WriteLine("Serie de Fibonacci para intercambio de cartas:");
                for (int i = 0; i < 8; i++)
                {
                    int value = FibonacciCalculator.GetReinforcementValue(i);
                    Console.WriteLine($"  Intercambio {i + 1}: {value} tropas");
                }
                Console.WriteLine();
                
                // Probar batalla simulada
                Console.WriteLine("--- SIMULACIÓN DE BATALLA ---");
                Territory attacker = game.Map.Territories.GetFirst();
                Territory defender = null;
                
                // Buscar un territorio adyacente enemigo
                IIterator<Territory> adjIterator = attacker.AdjacentTerritories.GetIterator();
                while (adjIterator.HasNext())
                {
                    Territory adj = adjIterator.Next();
                    if (adj.Owner != attacker.Owner)
                    {
                        defender = adj;
                        break;
                    }
                }
                
                if (attacker != null && defender != null && attacker.Troops >= 2)
                {
                    attacker.AddTroops(5); // Agregar tropas para la prueba
                    
                    Console.WriteLine($"Atacante: {attacker.Name} ({attacker.Owner.Alias}) - {attacker.Troops} tropas");
                    Console.WriteLine($"Defensor: {defender.Name} ({defender.Owner.Alias}) - {defender.Troops} tropas");
                    
                    Battle battle = new Battle(attacker.Owner, attacker, defender, 3);
                    CombatResult result = battle.Execute();
                    
                    if (result != null)
                    {
                        Console.WriteLine($"\nResultado:");
                        Console.WriteLine($"  Dados atacante: [{string.Join(", ", result.AttackRolls)}]");
                        Console.WriteLine($"  Dados defensor: [{string.Join(", ", result.DefenseRolls)}]");
                        Console.WriteLine($"  Pérdidas atacante: {result.AttackerLosses}");
                        Console.WriteLine($"  Pérdidas defensor: {result.DefenderLosses}");
                        Console.WriteLine($"  Territorio conquistado: {result.TerritoryConquered}");
                    }
                }
                else
                {
                    Console.WriteLine("No se pudo simular batalla (configuración insuficiente)");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al probar el juego: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}