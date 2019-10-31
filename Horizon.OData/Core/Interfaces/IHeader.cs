namespace Horizon.OData
{
    internal interface IHeader : IParameter
    {
        void SetRequired(bool required);

        void SetDefaultValue(string defaultValue);
    }
}