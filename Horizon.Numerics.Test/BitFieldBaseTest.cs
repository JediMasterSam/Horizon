using Horizon.Diagnostics;
using Horizon.Numerics.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Numerics.Test
{
    [TestClass]
    public class BitFieldBaseTest : Diagnostics.BaseTest
    {
        [TestMethod]
        public void EqualityTest()
        {
            Run(Test);

            void Test()
            {
                IsTrue((BitField<LetterFlags>) LetterFlags.A == LetterFlags.A);
                IsTrue((BitField<LetterFlags>) (LetterFlags.A | LetterFlags.B) == (LetterFlags.A | LetterFlags.B));
                IsFalse((BitField<LetterFlags>) (LetterFlags.A | LetterFlags.B) == LetterFlags.B);
                IsFalse((BitField<LetterFlags>) LetterFlags.A == LetterFlags.B);
            }
        }

        [TestMethod]
        public void InequalityTest()
        {
            Run(Test);

            void Test()
            {
                IsFalse((BitField<LetterFlags>) LetterFlags.A != LetterFlags.A);
                IsFalse((BitField<LetterFlags>) (LetterFlags.A | LetterFlags.B) != (LetterFlags.A | LetterFlags.B));
                IsTrue((BitField<LetterFlags>) (LetterFlags.A | LetterFlags.B) != LetterFlags.B);
                IsTrue((BitField<LetterFlags>) LetterFlags.A != LetterFlags.B);
            }
        }

        [TestMethod]
        public void ContainsAnyTest()
        {
            Run(Test);

            void Test()
            {
                var a = (BitField<LetterFlags>) LetterFlags.A;
                var bcd = (BitField<LetterFlags>) (LetterFlags.B | LetterFlags.C | LetterFlags.D);
                var abc = (BitField<LetterFlags>) (LetterFlags.A | LetterFlags.B | LetterFlags.C);

                IsFalse(a | bcd);
                IsFalse(bcd | a);
                IsTrue(abc | a);
                IsTrue(a | abc);
            }
        }

        [TestMethod]
        public void ContainsAllTest()
        {
            Run(Test);

            void Test()
            {
                var a = (BitField<LetterFlags>) LetterFlags.A;
                var bcd = (BitField<LetterFlags>) (LetterFlags.B | LetterFlags.C | LetterFlags.D);
                var abc = (BitField<LetterFlags>) (LetterFlags.A | LetterFlags.B | LetterFlags.C);

                IsFalse(a & bcd);
                IsFalse(bcd & a);
                IsTrue(abc & a);
                IsFalse(a & abc);
            }
        }
    }
}