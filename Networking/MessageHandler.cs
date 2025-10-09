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
                case MessageAction.StartGame:
                    game.StartGame();
                    break;

                case MessageAction.Attack:
                    // TODO
                    break;

                case MessageAction.Reinforce:
                    // TODO
                    break;

                case MessageAction.Move:
                    // TODO
                    break;

                case MessageAction.EndTurn:
                    game.NextTurn();
                    break;
            }
        
        }
    }
}