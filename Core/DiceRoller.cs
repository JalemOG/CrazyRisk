using System;

namespace CrazyRisk.Core
{
    public class DiceRoller
    {
        private Random random;
        
        public DiceRoller()
        {
            random = new Random();
        }
        
        public int[] RollDice(int numberOfDice)
        {
            int[] results = new int[numberOfDice];
            for (int i = 0; i < numberOfDice; i++)
            {
                results[i] = random.Next(1, 7);
            }
            Array.Sort(results);
            Array.Reverse(results);
            return results;
        }
        
        public CombatResult ResolveCombat(int attackerDice, int defenderDice)
        {
            int attDice = Math.Min(3, Math.Max(1, attackerDice));
            int defDice = Math.Min(2, Math.Max(1, defenderDice));
            
            int[] attResults = RollDice(attDice);
            int[] defResults = RollDice(defDice);
            
            int attackerLosses = 0;
            int defenderLosses = 0;
            
            int comparisons = Math.Min(attDice, defDice);
            for (int i = 0; i < comparisons; i++)
            {
                if (attResults[i] > defResults[i])
                    defenderLosses++;
                else
                    attackerLosses++;
            }
            
            return new CombatResult
            {
                AttackerLosses = attackerLosses,
                DefenderLosses = defenderLosses,
                TerritoryConquered = defenderLosses >= defenderDice
            };
        }
    }
}