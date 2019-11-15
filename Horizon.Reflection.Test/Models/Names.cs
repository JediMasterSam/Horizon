namespace Horizon.Reflection.Test.Models
{
    public class Names
    {
        public readonly int Field;

        public int Property { get; }

        public Names()
        {
        }

        public Names(int a, int b)
        {
        }

        public void Method()
        {
        }

        public void Method(int a, int b)
        {
        }

        public void Method<T>(T t)
        {
        }
    }
}