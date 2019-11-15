namespace Horizon.Reflection.Test.Models
{
    public class Message
    {
        public Message()
        {
            Value = string.Empty;
        }

        public Message(string value)
        {
            Value = value;
        }
        
        public string Value { get; private set; }

        public void Append(string value)
        {
            Value += value;
        }

        public TMessage AppendMessage<TMessage>(TMessage message) where TMessage : Message
        {
            message.Append(Value);
            return message;
        }

        public char[] GetCharacters()
        {
            return Value.ToCharArray();
        }
    }
}