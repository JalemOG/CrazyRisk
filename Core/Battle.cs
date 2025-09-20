namespace CrazyRisk.Core
{
    public class Battle
    {
        public Player Attacker { get; set; }
        public Player Defender { get; set; }
        public Territory FromTerritory { get; set; }
        public Territory ToTerritory { get; set; }
        public DiceRoller DiceRoller { get; set; }
        
        public Battle(Player attacker, Player defender, Territory from, Territory to)
        {
            Attacker = attacker;
            Defender = defender;
            FromTerritory = from;
            ToTerritory = to;
            DiceRoller = new DiceRoller();
        }
        
        public CombatResult Execute()
        {
            int attackerDice = System.Math.Min(3, FromTerritory.Troops - 1);
            int defenderDice = System.Math.Min(2, ToTerritory.Troops);
            
            return DiceRoller.ResolveCombat(attackerDice, defenderDice);
        }
    }
}