using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

#pragma warning disable 618

namespace Horizon.Reflection
{
    internal static class InstructionReader
    {
        private static TValue Read<TValue>(byte[] byteArray, ref int index) where TValue : struct
        {
            object value;

            if (typeof(byte) == typeof(TValue))
            {
                value = byteArray[index];
                index += 1;
            }
            else if (typeof(sbyte) == typeof(TValue))
            {
                value = (sbyte) byteArray[index];
                index += 1;
            }
            else if (typeof(ushort) == typeof(TValue))
            {
                value = BitConverter.ToUInt16(byteArray, index);
                index += 2;
            }
            else if (typeof(int) == typeof(TValue))
            {
                value = BitConverter.ToInt32(byteArray, index);
                index += 4;
            }
            else if (typeof(ulong) == typeof(TValue))
            {
                value = BitConverter.ToUInt64(byteArray, index);
                index += 8;
            }
            else if (typeof(float) == typeof(TValue))
            {
                value = BitConverter.ToSingle(byteArray, index);
                index += 4;
            }
            else if (typeof(double) == typeof(TValue))
            {
                value = BitConverter.ToDouble(byteArray, index);
                index += 8;
            }
            else
            {
                return default;
            }

            return (TValue) value;
        }

        private static IReadOnlyList<Instruction> GetInstructions(MethodBase methodInfo)
        {
            const int min = 254;
            const int max = 65024;

            var methodBody = methodInfo.GetMethodBody();
            var module = methodInfo.Module;
            var declaringType = methodInfo.DeclaringType;

            if (methodBody == null || declaringType == null) return null;

            var index = 0;
            var byteArray = methodBody.GetILAsByteArray();
            var instructions = new List<Instruction>();

            while (index < byteArray.Length)
            {
                var value = (ushort) byteArray[index++];
                OpCode operationCode;

                if (value != min)
                {
                    operationCode = OperationCodes.Instance.SingleByte[value];
                }
                else
                {
                    operationCode = OperationCodes.Instance.MultiByte[value = byteArray[index++]];
                    value |= max;
                }

                var offset = index - 1;
                object operand;

                switch (operationCode.OperandType)
                {
                    case OperandType.InlineBrTarget:
                    {
                        operand = Read<int>(byteArray, ref index) + index;
                        break;
                    }
                    case OperandType.InlineField:
                    {
                        operand = module.ResolveField(Read<int>(byteArray, ref index));
                        break;
                    }
                    case OperandType.InlineMethod:
                    {
                        var metadataToken = Read<int>(byteArray, ref index);

                        try
                        {
                            operand = module.ResolveMethod(metadataToken);
                        }
                        catch
                        {
                            operand = module.ResolveMember(metadataToken);
                        }

                        break;
                    }
                    case OperandType.InlineSig:
                    {
                        operand = module.ResolveSignature(Read<int>(byteArray, ref index));
                        break;
                    }
                    case OperandType.InlineTok:
                    {
                        try
                        {
                            operand = module.ResolveType(Read<int>(byteArray, ref index));
                        }
                        catch
                        {
                            operand = null;
                        }

                        break;
                    }
                    case OperandType.InlineType:
                    {
                        operand = module.ResolveType(Read<int>(byteArray, ref index), declaringType.GetGenericArguments(), methodInfo.GetGenericArguments());
                        break;
                    }
                    case OperandType.InlineI:
                    {
                        operand = Read<int>(byteArray, ref index);
                        break;
                    }
                    case OperandType.InlineI8:
                    {
                        operand = Read<ulong>(byteArray, ref index);
                        break;
                    }
                    case OperandType.InlineNone:
                    {
                        operand = null;
                        break;
                    }
                    case OperandType.InlineR:
                    {
                        operand = Read<double>(byteArray, ref index);
                        break;
                    }
                    case OperandType.InlineString:
                    {
                        operand = module.ResolveString(Read<int>(byteArray, ref index));
                        break;
                    }
                    case OperandType.InlineSwitch:
                    {
                        var addresses = new int[Read<int>(byteArray, ref index)];
                        var cases = new int[addresses.Length];

                        for (var addressIndex = 0; addressIndex < addresses.Length; addressIndex++)
                        {
                            addresses[addressIndex] = Read<int>(byteArray, ref index);
                        }

                        for (var caseIndex = 0; caseIndex < cases.Length; caseIndex++)
                        {
                            cases[caseIndex] = index + addresses[caseIndex];
                        }

                        operand = null;
                        break;
                    }
                    case OperandType.InlineVar:
                    {
                        operand = Read<ushort>(byteArray, ref index);
                        break;
                    }
                    case OperandType.ShortInlineBrTarget:
                    case OperandType.ShortInlineI:
                    {
                        operand = Read<sbyte>(byteArray, ref index);
                        break;
                    }
                    case OperandType.ShortInlineR:
                    {
                        operand = Read<float>(byteArray, ref index);
                        break;
                    }
                    case OperandType.ShortInlineVar:
                    {
                        operand = Read<byte>(byteArray, ref index);
                        break;
                    }
                    case OperandType.InlinePhi:
                    {
                        operand = null;
                        break;
                    }
                    default:
                    {
                        operand = null;
                        break;
                    }
                }

                instructions.Add(new Instruction(operationCode, offset, operand));
            }

            return instructions;
        }

        public static IReadOnlyList<Instruction> GetInstructions(MethodBaseData method)
        {
            try
            {
                return GetInstructions((MethodBase) method);
            }
            catch
            {
                return new List<Instruction>();
            }
        }
    }
}