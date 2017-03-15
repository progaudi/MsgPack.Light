using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using ProGaudi.MsgPack.Light.Converters;

namespace ProGaudi.MsgPack.Light
{
    public class MsgPackContext
    {
        private static readonly IMsgPackTokenConverter<object> SharedNullTokenConverter = new NullTokenConverter();

        private readonly Dictionary<Type, IMsgPackTokenConverter> _converters;

        private readonly Dictionary<Type, Type> _genericConverters = new Dictionary<Type, Type>();

        private readonly Dictionary<Type, Func<object>> _objectActivators = new Dictionary<Type, Func<object>>();

        public MsgPackContext(bool strictParseOfFloat = false)
        {
            var dateTimeConverter = new DateTimeTokenConverter();
            var tokenConverter = new MsgPackTokenTokenConverter(strictParseOfFloat);
            _converters = new Dictionary<Type, IMsgPackTokenConverter>
            {
                {typeof (bool), tokenConverter},
                {typeof (string), tokenConverter},
                {typeof (byte[]), tokenConverter},
                {typeof (float), tokenConverter},
                {typeof (double), tokenConverter},
                {typeof (byte), tokenConverter},
                {typeof (sbyte), tokenConverter},
                {typeof (short), tokenConverter},
                {typeof (ushort), tokenConverter},
                {typeof (int), tokenConverter},
                {typeof (uint), tokenConverter},
                {typeof (long), tokenConverter},
                {typeof (ulong), tokenConverter},
                {typeof (DateTime), dateTimeConverter},
                {typeof (DateTimeOffset), dateTimeConverter},
                {typeof (TimeSpan), new TimeSpanTokenConverter() },

                {typeof (bool?), new NullableTokenConverter<bool>()},
                {typeof (float?), new NullableTokenConverter<float>()},
                {typeof (double?), new NullableTokenConverter<double>()},
                {typeof (byte?), new NullableTokenConverter<byte>()},
                {typeof (sbyte?), new NullableTokenConverter<sbyte>()},
                {typeof (short?), new NullableTokenConverter<short>()},
                {typeof (ushort?), new NullableTokenConverter<ushort>()},
                {typeof (int?), new NullableTokenConverter<int>()},
                {typeof (uint?), new NullableTokenConverter<uint>()},
                {typeof (long?), new NullableTokenConverter<long>()},
                {typeof (ulong?), new NullableTokenConverter<ulong>()},
                {typeof (DateTime?), new NullableTokenConverter<DateTime>()},
                {typeof (DateTimeOffset?), new NullableTokenConverter<DateTimeOffset>()}
            };

            foreach (var converter in _converters)
            {
                converter.Value.Initialize(this);
            }
        }

        public IMsgPackTokenConverter<object> NullTokenConverter => SharedNullTokenConverter;

        public void RegisterConverter<T>(IMsgPackTokenConverter<T> tokenConverter)
        {
            tokenConverter.Initialize(this);
            _converters[typeof(T)] = tokenConverter;
        }

        public void RegisterGenericConverter(Type type)
        {
            var converterType = GetGenericInterface(type, typeof(IMsgPackTokenConverter<>));
            if (converterType == null)
            {
                throw new ArgumentException($"Error registering generic tokenConverter. Expected IMsgPackTokenConverter<> implementation, but got {type}");
            }

            var convertedType = converterType.GenericTypeArguments.Single().GetGenericTypeDefinition();
            _genericConverters.Add(convertedType, type);
        }

        public IMsgPackTokenConverter<T> GetConverter<T>()
        {
            var type = typeof(T);
            var result = (IMsgPackTokenConverter<T>)GetConverterFromCache<T>();
            if (result != null)
                return result;

            result = (IMsgPackTokenConverter<T>)(TryGenerateConverterFromGenericConverter(type)
                ?? TryGenerateArrayConverter(type)
                ?? TryGenerateMapConverter(type)
                ?? TryGenerateNullableConverter(type));

            if (result == null)
            {
                throw ExceptionUtils.ConverterNotFound(type);
            }

            return result;
        }

        public Func<object> GetObjectActivator(Type type)
        {
            return _objectActivators.GetOrAdd(type, CompiledLambdaActivatorFactory.GetActivator);
        }

        private IMsgPackTokenConverter CreateAndInializeConverter(Func<object> converterActivator)
        {
            var converter = (IMsgPackTokenConverter)converterActivator();
            converter.Initialize(this);
            return converter;
        }

        private IMsgPackTokenConverter TryGenerateConverterFromGenericConverter(Type type)
        {
            if (!type.GetTypeInfo().IsGenericType)
            {
                return null;
            }
            var genericType = type.GetGenericTypeDefinition();
            Type genericConverterType;

            if (!_genericConverters.TryGetValue(genericType, out genericConverterType))
            {
                return null;
            }

            var converterType = genericConverterType.MakeGenericType(type.GenericTypeArguments);
            return _converters.GetOrAdd(type, x => CreateAndInializeConverter(GetObjectActivator(converterType)));
        }

        private IMsgPackTokenConverter TryGenerateMapConverter(Type type)
        {
            var mapInterface = GetGenericInterface(type, typeof(IDictionary<,>));
            if (mapInterface != null)
            {
                return _converters.GetOrAdd(type, x => CreateAndInializeConverter(GetObjectActivator(typeof(MapTokenConverter<,,>).MakeGenericType(
                    x,
                    mapInterface.GenericTypeArguments[0],
                    mapInterface.GenericTypeArguments[1]))));
            }

            mapInterface = GetGenericInterface(type, typeof(IReadOnlyDictionary<,>));
            if (mapInterface != null)
            {
                return _converters.GetOrAdd(type, x => CreateAndInializeConverter(GetObjectActivator(typeof(ReadOnlyMapTokenConverter<,,>).MakeGenericType(
                    x,
                    mapInterface.GenericTypeArguments[0],
                    mapInterface.GenericTypeArguments[1]))));
            }

            return null;
        }

        private IMsgPackTokenConverter TryGenerateNullableConverter(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (!typeInfo.IsGenericType || typeInfo.GetGenericTypeDefinition() != typeof(Nullable<>))
            {
                return null;
            }

            return _converters.GetOrAdd(type, x => CreateAndInializeConverter(GetObjectActivator(typeof(NullableTokenConverter<>).MakeGenericType(x.GetTypeInfo().GenericTypeArguments[0]))));
        }

        private IMsgPackTokenConverter TryGenerateArrayConverter(Type type)
        {
            var arrayInterface = GetGenericInterface(type, typeof(IList<>));
            if (arrayInterface != null)
            {
                return _converters.GetOrAdd(type, x => CreateAndInializeConverter(GetObjectActivator(typeof(ArrayTokenConverter<,>).MakeGenericType(x, arrayInterface.GenericTypeArguments[0]))));
            }

            arrayInterface = GetGenericInterface(type, typeof(IReadOnlyList<>));
            return arrayInterface != null
                ? _converters.GetOrAdd(type, x => CreateAndInializeConverter(GetObjectActivator(typeof(ReadOnlyListTokenConverter<,>).MakeGenericType(x, arrayInterface.GenericTypeArguments[0]))))
                : null;
        }

        private IMsgPackTokenConverter GetConverterFromCache<T>()
        {
            IMsgPackTokenConverter temp;

            if (_converters.TryGetValue(typeof(T), out temp))
            {
                return temp;
            }

            return null;
        }

        private static TypeInfo GetGenericInterface(Type type, Type genericInterfaceType)
        {
            var info = type.GetTypeInfo();
            if (info.IsInterface && info.IsGenericType && info.GetGenericTypeDefinition() == genericInterfaceType)
            {
                return info;
            }

            return info
                .ImplementedInterfaces
                .Select(x => x.GetTypeInfo())
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterfaceType);
        }
    }
}