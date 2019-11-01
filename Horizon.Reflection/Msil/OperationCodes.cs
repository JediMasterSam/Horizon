using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Horizon.Reflection
{
    internal sealed class OperationCodes
    {
        internal static readonly OperationCodes Instance = new OperationCodes();
        
        private OperationCodes()
        {
            const ushort bufferSize = 256;

            var singleByte = new OpCode[bufferSize];
            var multiByte = new OpCode[bufferSize];

            var operationCodes = typeof(OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(field => field.FieldType == typeof(OpCode))
                .Select(field => (OpCode) field.GetValue(null));

            foreach (var operationCode in operationCodes)
            {
                var value = (ushort) operationCode.Value;

                if (value < bufferSize)
                {
                    singleByte[value] = operationCode;
                }
                else
                {
                    multiByte[value & (bufferSize - 1)] = operationCode;
                }
            }

            SingleByte = singleByte;
            MultiByte = multiByte;
        }

        internal IReadOnlyList<OpCode> SingleByte { get; }
        
        internal IReadOnlyList<OpCode> MultiByte { get; }
    }
}