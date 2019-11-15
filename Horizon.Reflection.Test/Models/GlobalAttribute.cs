using System;

[assembly: Global]

// ReSharper disable once EmptyNamespace
namespace Horizon.Reflection.Test
{
}


[AttributeUsage(AttributeTargets.Assembly)]
public class GlobalAttribute : Attribute
{
}