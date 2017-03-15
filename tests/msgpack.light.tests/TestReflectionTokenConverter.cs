using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

using JetBrains.Annotations;

// ReSharper disable once RedundantUsingDirective

namespace ProGaudi.MsgPack.Light.Tests
{
    public class TestReflectionTokenConverter : IMsgPackTokenConverter<object>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public MsgPackToken ConvertFrom(object value)
        {
            if (value == null)
            {
                return null;
            }

            var converter = GetConverter(_context, value.GetType());

            var methodDefinition = typeof(IMsgPackTokenConverter<>).MakeGenericType(value.GetType()).GetMethod(
                "ConvertFrom",
                new[] { value.GetType() });

            return (MsgPackToken) methodDefinition.Invoke(converter, new[] { value });
        }

        public object ConvertTo(MsgPackToken token)
        {
            var msgPackType = token.DataType;

            Type type;
            switch (msgPackType)
            {
                case DataTypes.Null:
                    return null;

                case DataTypes.False:
                    return false;

                case DataTypes.True:
                    return true;

                case DataTypes.Single:
                    type = typeof(float);
                    break;

                case DataTypes.Double:
                    type = typeof(double);
                    break;

                case DataTypes.UInt8:
                    type = typeof(byte);
                    break;

                case DataTypes.UInt16:
                    type = typeof(ushort);
                    break;

                case DataTypes.UInt32:
                    type = typeof(uint);
                    break;

                case DataTypes.UInt64:
                    type = typeof(ulong);
                    break;

                case DataTypes.Int8:
                    type = typeof(sbyte);
                    break;

                case DataTypes.Int16:
                    type = typeof(short);
                    break;

                case DataTypes.Int32:
                    type = typeof(int);
                    break;

                case DataTypes.Int64:
                    type = typeof(long);
                    break;

                case DataTypes.Array16:
                    type = typeof(object[]);
                    break;

                case DataTypes.Array32:
                    type = typeof(object[]);
                    break;

                case DataTypes.Map16:
                    type = typeof(Dictionary<object, object>);
                    break;

                case DataTypes.Map32:
                    type = typeof(Dictionary<object, object>);
                    break;

                case DataTypes.Str8:
                    type = typeof(string);
                    break;

                case DataTypes.Str16:
                    type = typeof(string);
                    break;

                case DataTypes.Str32:
                    type = typeof(string);
                    break;

                case DataTypes.Bin8:
                    type = typeof(byte[]);
                    break;

                case DataTypes.Bin16:
                    type = typeof(byte[]);
                    break;

                case DataTypes.Bin32:
                    type = typeof(byte[]);
                    break;

                default:
                    type = TryInferFromFixedLength(msgPackType);
                    break;
            }

            var converter = GetConverter(_context, type);
            var methodDefinition = typeof(IMsgPackTokenConverter<>).MakeGenericType(type).GetMethod(
                "ConvertTo",
                new[] { typeof(MsgPackToken) });

            return methodDefinition.Invoke(converter, new object[] { token });
        }

        private Type TryInferFromFixedLength(DataTypes msgPackType)
        {
            if (msgPackType.GetHighBits(1) == DataTypes.PositiveFixNum.GetHighBits(1))
                return typeof(byte);

            if (msgPackType.GetHighBits(3) == DataTypes.NegativeFixNum.GetHighBits(3))
                return typeof(sbyte);

            if (msgPackType.GetHighBits(4) == DataTypes.FixArray.GetHighBits(4))
                return typeof(object[]);

            if (msgPackType.GetHighBits(3) == DataTypes.FixStr.GetHighBits(3))
                return typeof(string);

            if (msgPackType.GetHighBits(4) == DataTypes.FixMap.GetHighBits(4))
                return typeof(Dictionary<object, object>);

            throw new SerializationException($"Can't infer type for msgpack type: {msgPackType:G} (0x{msgPackType:X})");
        }

        [NotNull]
        private static object GetConverter(MsgPackContext context, Type type)
        {
            var methodDefinition = typeof(MsgPackContext).GetMethod(nameof(MsgPackContext.GetConverter), new Type[0]);
            var concreteMethod = methodDefinition.MakeGenericMethod(type);
            var converter = concreteMethod.Invoke(context, null);
            if (converter == null)
                throw new SerializationException($"Please, provide convertor for {type.Name}");
            return converter;
        }
    }
}