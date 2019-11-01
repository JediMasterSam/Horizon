using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Horizon.Reflection
{
    public sealed class Instruction
    {
        private readonly Lazy<TypeData> _type;

        private readonly Lazy<MethodBaseData> _method;

        internal Instruction(OpCode operationCode, int offset, object operand)
        {
            _type = new Lazy<TypeData>(() => GetType(Operand));
         //   _method = new Lazy<MethodBaseData>(() => GetMethod(Operand));

            OperationCode = operationCode;
            Offset = offset;
            Operand = operand;
        }

        public OpCode OperationCode { get; }

        public int Offset { get; }

        public object Operand { get; }

        private static TypeData GetType(object operand)
        {
            return operand is Type type ? type.GetTypeData() : null;
        }

//        private static MethodBaseData GetMethod(object operand)
//        {
//            if (!(operand is MethodBase methodBase)) return null;
//
//            var query = new MethodBaseDataQuery
//            {
//                Name = methodBase.IsConstructor ? ".ctor" : methodBase.Name,
//                Parameters = methodBase.GetParameters().Select(parameter => parameter.ParameterType.GetTypeData()).ToArray()
//            };
//
//            return methodBase.DeclaringType.GetTypeData().TryGetMethod(query, out var method) ? method : null;
//        }

        public bool TryGetType(out TypeData type)
        {
            type = _type.Value;
            return type != null;
        }

        public bool TryGetMethod(out MethodBaseData method)
        {
            method = _method.Value;
            return method != null;
        }
    }
}