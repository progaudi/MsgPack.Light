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
            var msgPackType = token.DataTypeInternal;

            Type type;
            switch (msgPackType)
            {
                case DataTypeInternal.Null:
                    return null;

                case DataTypeInternal.False:
                    return false;

                case DataTypeInternal.True:
                    return true;

                case DataTypeInternal.Single:
                    type = typeof(float);
                    break;

                case DataTypeInternal.Double:
                    type = typeof(double);
                    break;

                case DataTypeInternal.UInt8:
                    type = typeof(byte);
                    break;

                case DataTypeInternal.UInt16:
                    type = typeof(ushort);
                    break;

                case DataTypeInternal.UInt32:
                    type = typeof(uint);
                    break;

                case DataTypeInternal.UInt64:
                    type = typeof(ulong);
                    break;

                case DataTypeInternal.Int8:
                    type = typeof(sbyte);
                    break;

                case DataTypeInternal.Int16:
                    type = typeof(short);
                    break;

                case DataTypeInternal.Int32:
                    type = typeof(int);
                    break;

                case DataTypeInternal.Int64:
                    type = typeof(long);
                    break;

                case DataTypeInternal.Array16:
                    type = typeof(object[]);
                    break;

                case DataTypeInternal.Array32:
                    type = typeof(object[]);
                    break;

                case DataTypeInternal.Map16:
                    type = typeof(Dictionary<object, object>);
                    break;

                case DataTypeInternal.Map32:
                    type = typeof(Dictionary<object, object>);
                    break;

                case DataTypeInternal.Str8:
                    type = typeof(string);
                    break;

                case DataTypeInternal.Str16:
                    type = typeof(string);
                    break;

                case DataTypeInternal.Str32:
                    type = typeof(string);
                    break;

                case DataTypeInternal.Bin8:
                    type = typeof(byte[]);
                    break;

                case DataTypeInternal.Bin16:
                    type = typeof(byte[]);
                    break;

                case DataTypeInternal.Bin32:
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

        private Type TryInferFromFixedLength(DataTypeInternal msgPackTypeInternal)
        {
            if (msgPackTypeInternal.GetHighBits(1) == DataTypeInternal.PositiveFixNum.GetHighBits(1))
                return typeof(byte);

            if (msgPackTypeInternal.GetHighBits(3) == DataTypeInternal.NegativeFixNum.GetHighBits(3))
                return typeof(sbyte);

            if (msgPackTypeInternal.GetHighBits(4) == DataTypeInternal.FixArray.GetHighBits(4))
                return typeof(object[]);

            if (msgPackTypeInternal.GetHighBits(3) == DataTypeInternal.FixStr.GetHighBits(3))
                return typeof(string);

            if (msgPackTypeInternal.GetHighBits(4) == DataTypeInternal.FixMap.GetHighBits(4))
                return typeof(Dictionary<object, object>);

            throw new SerializationException($"Can't infer typeInternal for msgpack typeInternal: {msgPackTypeInternal:G} (0x{msgPackTypeInternal:X})");
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