using CrazyRisk.DataStructures;

namespace CrazyRisk.Core
{
    public class TurnManager
    {
        private CrazyRisk.DataStructures.Queue<Player> turnOrder;
        public Player CurrentPlayer { get; private set; }
        
        public TurnManager(CrazyRisk.DataStructures.LinkedList<Player> players)
        {
            turnOrder = new CrazyRisk.DataStructures.Queue<Player>();
            
            IIterator<Player> iterator = players.GetIterator();
            while (iterator.HasNext())
            {
                var p = iterator.Next();
                turnOrder.Enqueue(p);
            }

            CurrentPlayer = turnOrder.Peek();
        }
        
        public void StartTurns()
        {
            // Lógica inicial de turnos
        }
        
        public Player? NextTurn()
        {
            if (turnOrder.IsEmpty)
                return null;
                
            var previous = turnOrder.Dequeue();
            turnOrder.Enqueue(previous);
            CurrentPlayer = turnOrder.Peek();
            
            return CurrentPlayer;
        }
    }
}