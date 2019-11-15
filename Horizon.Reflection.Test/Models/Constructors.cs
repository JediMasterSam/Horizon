using System;
using System.Reflection;

namespace Horizon.Reflection.Test.Models
{
    public class Constructors
    {
        public ConstructorInfo DefaultConstructor { get; }
        
        public ConstructorInfo NotDefaultConstructor { get; }

        public Constructors()
        {
            DefaultConstructor = typeof(Message).GetConstructor(new Type[0]);
            NotDefaultConstructor = typeof(Message).GetConstructor(new[] {typeof(string)});
        }
    }

    
}