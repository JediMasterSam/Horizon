namespace Horizon.OData
{
    public interface IParameter
    {
        string Name { get; }

        string DefaultValue { get; }

        bool Required { get; }
    }
}