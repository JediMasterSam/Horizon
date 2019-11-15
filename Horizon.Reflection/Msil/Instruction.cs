using System.Reflection.Emit;

namespace Horizon.Reflection
{
    /// <summary>
    /// MSIL instruction.
    /// </summary>
    public sealed class Instruction
    {
        /// <summary>
        /// Creates a new instance of <see cref="Instruction"/>.
        /// </summary>
        /// <param name="operationCode">Operation code.</param>
        /// <param name="offset"></param>
        /// <param name="operand"></param>
        internal Instruction(OpCode operationCode, int offset, object operand)
        {
            OperationCode = operationCode;
            Offset = offset;
            Operand = operand;
        }

        /// <summary>
        /// The operation code for the current <see cref="Instruction"/>.
        /// </summary>
        public OpCode OperationCode { get; }

        /// <summary>
        /// Byte code offset for the current <see cref="Instruction"/>.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// Target of the operation for the current <see cref="Instruction"/>.
        /// </summary>
        public object Operand { get; }
    }
}