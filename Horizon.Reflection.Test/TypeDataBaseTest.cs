﻿using Horizon.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test
{
    [TestClass]
    public class TypeDataBaseTest : Diagnostics.BaseTest
    {
        [TestMethod]
        public void FieldsTest()
        {
            var temp = typeof(Foobar).GetTypeData().Interfaces;
        }
    }

    public class Foobar : IFoo
    {
        
    }

    public interface IFoo : IBar
    {
        
    }

    public interface IBar : IBase
    {
        
    }

    public interface IBase
    {
        
    }
}