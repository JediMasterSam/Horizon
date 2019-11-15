using Horizon.OData.Attributes;
using Horizon.Reflection;
using Microsoft.AspNet.OData.Builder;

namespace Horizon.OData.Builders
{
    public sealed class EdmModelBuilder
    {
        private readonly ODataModelBuilder _oDataModelBuilder;
        
        public EdmModelBuilder(ODataModelBuilder oDataModelBuilder)
        {
            const string defaultNamespace = "Default";
            
            _oDataModelBuilder = new ODataModelBuilder();

            EdmTypeBuilder = new EdmTypeBuilder(oDataModelBuilder);
            Namespace = defaultNamespace;
        }

        public EdmTypeBuilder EdmTypeBuilder { get; }
        
        public string Namespace { get; set; }

        public void BuildEdmType(TypeData type)
        {
            if (!EdmTypeBuilder.TryBuild(type, out var edmType) || !(edmType is EntityTypeConfiguration entityType)) return;

            if (type.TryGetAttribute<EntitySetAttribute>(out var entitySetAttribute))
            {
                _oDataModelBuilder.AddEntitySet(entitySetAttribute.Name, entityType);
            }
            else if (type.TryGetAttribute<SingletonTypeAttribute>(out var singletonTypeAttribute))
            {
                _oDataModelBuilder.AddSingleton(singletonTypeAttribute.Name, entityType);
            }
        }
    }
}