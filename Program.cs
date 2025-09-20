using CrazyRisk.Core;
using CrazyRisk.DataStructures;

namespace CrazyRisk
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando Crazy Risk...");
            
            // Ejemplo de uso
            LinkedList<string> testList = new LinkedList<string>();
            testList.Add("Jugador1");
            testList.Add("Jugador2");
            
            IIterator<string> iterator = testList.GetIterator();
            while (iterator.HasNext())
            {
                Console.WriteLine(iterator.Next());
            }
            
            Console.WriteLine("Juego terminado.");
        }
    }
}