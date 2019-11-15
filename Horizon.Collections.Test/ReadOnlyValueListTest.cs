using Horizon.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Collections.Test
{
    [TestClass]
    public class ReadOnlyValueListTest : BaseTest
    {
        private ReadOnlyValueList Values { get; } = new ReadOnlyValueList(new[]
        {
            new ValueElement(1),
            new ValueElement(2),
            new ValueElement(true),
            new ValueElement("string"),
            new ValueElement(3)
        });

        [TestMethod]
        public void ContainsTest()
        {
            Run(Test);

            void Test()
            {
                IsTrue(Values.Contains<int>());
                IsTrue(Values.Contains<bool>());
                IsTrue(Values.Contains<string>());
                IsFalse(Values.Contains<float>());
            }
        }

        [TestMethod]
        public void TryGetTest()
        {
            Run(Test);

            void Test()
            {
                if (IsTrue(Values.TryGet<int>(out var number)))
                {
                    AreEqual(1, number);
                }

                if (IsTrue(Values.TryGet<bool>(out var boolean)))
                {
                    AreEqual(true, boolean);
                }

                if (IsTrue(Values.TryGet<string>(out var name)))
                {
                    AreEqual("string", name);
                }

                if (IsFalse(Values.TryGet<float>(out var value)))
                {
                    AreEqual(0.0f, value);
                }
            }
        }

        [TestMethod]
        public void GetTest()
        {
            Run(Test);

            void Test()
            {
                SequenceEquals(new[] {1, 2, 3}, Values.Get<int>());
                SequenceEquals(new[] {true}, Values.Get<bool>());
                SequenceEquals(new[] {"string"}, Values.Get<string>());

                IsEmpty(Values.Get<float>());
            }
        }
    }
}