using System;
using System.Reflection;

namespace Horizon.Reflection.Test.Models
{
    public class MessageHelper
    {
        public MessageHelper()
        {
            DefaultConstructor = typeof(Message).GetConstructor(new Type[0]);
            NotDefaultConstructor = typeof(Message).GetConstructor(new[] {typeof(string)});
            Value = typeof(Message).GetProperty(nameof(Message.Value));
            Append = typeof(Message).GetMethod(nameof(Message.Append), 0, new[] {typeof(string)});
            AppendMessage = typeof(Message).GetMethod(nameof(Message.AppendMessage), 1, new[] {Type.MakeGenericMethodParameter(0)});
            GetCharacters = typeof(Message).GetMethod(nameof(Message.GetCharacters), 0, new Type[0]);
            MessageType = typeof(Message).GetTypeData();
        }

        public ConstructorInfo DefaultConstructor { get; }

        public ConstructorInfo NotDefaultConstructor { get; }

        public PropertyInfo Value { get; }

        public MethodInfo Append { get; }

        public MethodInfo AppendMessage { get; }

        public MethodInfo GetCharacters { get; }
        
        public TypeData MessageType { get; }
    }
}