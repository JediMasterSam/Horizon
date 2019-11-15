using System;

[assembly: Global]

namespace Horizon.Reflection.Test.Models
{
    [Global]
    [Local]
    public class BaseAttributes
    {
        [Global] [Local] public virtual int Property { get; set; }

        [Global]
        [Local]
        public virtual void Method([Global] [Local] int a)
        {
        }
    }


    public class Attributes : BaseAttributes
    {
        public override int Property { get; set; }

        public override void Method(int a)
        {
        }
    }
}


[AttributeUsage(AttributeTargets.All)]
public class GlobalAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.All, Inherited = false)]
public class LocalAttribute : Attribute
{
}