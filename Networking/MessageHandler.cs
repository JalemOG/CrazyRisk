using CrazyRisk.Core;

namespace CrazyRisk.Networking
{
    public class MessageHandler
    {
        private Game game;

        public MessageHandler(Game game)
        {
            this.game = game;
        }

        public void ProcessMessage(Message msg)
        {
            switch (msg.Action)
            {
                case MessageAction.Attack:
                    // Procesar ataque
                    break;
                case MessageAction.Reinforce:
                    // Procesar refuerzos
                    break;
                case MessageAction.Move:
                    // Procesar movimiento
                    break;
                case MessageAction.EndTurn:
                    game.NextTurn();
                    break;
            }
        }
    }
}