namespace CrazyRisk.Networking
{
    public class Message
    {
        public MessageAction Action { get; set; }
        public object Payload { get; set; } = string.Empty;
        
        public string Serialize()
        {
            return $"{Action}|{Payload}";
        }
        
        public static Message Deserialize(string data)
        {
            string[] parts = data.Split('|');
            return new Message
            {
                Action = (MessageAction)System.Enum.Parse(typeof(MessageAction), parts[0]),
                Payload = parts[1]
            };
        }
    }
}