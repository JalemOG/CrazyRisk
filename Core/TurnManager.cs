using CrazyRisk.DataStructures;

namespace CrazyRisk.Core
{
    public class TurnManager
    {
        private Queue<Player> turnOrder;
        public Player CurrentPlayer { get; private set; }
        
        public TurnManager(LinkedList<Player> players)
        {
            turnOrder = new Queue<Player>();
            
            IIterator<Player> iterator = players.GetIterator();
            while (iterator.HasNext())
            {
                turnOrder.Enqueue(iterator.Next());
            }
            
            if (!turnOrder.IsEmpty)
                CurrentPlayer = turnOrder.Peek();
        }
        
        public void StartTurns()
        {
            // Lógica inicial de turnos
        }
        
        public Player NextTurn()
        {
            if (turnOrder.IsEmpty)
                return null;
                
            Player previous = turnOrder.Dequeue();
            turnOrder.Enqueue(previous);
            CurrentPlayer = turnOrder.Peek();
            
            return CurrentPlayer;
        }
    }
}