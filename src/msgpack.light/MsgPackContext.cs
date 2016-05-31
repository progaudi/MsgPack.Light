using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using MsgPack.Light.Converters;

namespace MsgPack.Light
{
    public class MsgPackContext
    {
        private bool _isInitialized = false;

        private static readonly IMsgPackConverter<object> SharedNullConverter = new NullConverter();

        private readonly Dictionary<Type, IMsgPackConverter> _converters = new Dictionary<Type, IMsgPackConverter>
        {
            {typeof (bool), new BoolConverter()},
            {typeof (string), new StringConverter()},
            {typeof (byte[]), new BinaryConverter()},
            {typeof (float), new FloatConverter()},
            {typeof (double), new FloatConverter()},
            {typeof (byte), new IntConverter()},
            {typeof (sbyte), new IntConverter()},
            {typeof (short), new IntConverter()},
            {typeof (ushort), new IntConverter()},
            {typeof (int), new IntConverter()},
            {typeof (uint), new IntConverter()},
            {typeof (long), new IntConverter()},
            {typeof (ulong), new IntConverter()},
            {typeof (DateTime), new DateTimeConverter()},
            {typeof (DateTimeOffset), new DateTimeConverter()},

            {typeof (bool?), new NullableConverter<bool>()},
            {typeof (float?), new NullableConverter<float>()},
            {typeof (double?), new NullableConverter<double>()},
            {typeof (byte?), new NullableConverter<byte>()},
            {typeof (sbyte?), new NullableConverter<sbyte>()},
            {typeof (short?), new NullableConverter<short>()},
            {typeof (ushort?), new NullableConverter<ushort>()},
            {typeof (int?), new NullableConverter<int>()},
            {typeof (uint?), new NullableConverter<uint>()},
            {typeof (long?), new NullableConverter<long>()},
            {typeof (ulong?), new NullableConverter<ulong>()},
            {typeof (DateTime?), new NullableConverter<DateTime>()},
            {typeof (DateTimeOffset?), new NullableConverter<DateTimeOffset>()}
        };

        private readonly Dictionary<Type, Type> _genericConverters = new Dictionary<Type, Type>();

        private readonly Dictionary<Type, Func<object>> _objectActivators = new Dictionary<Type, Func<object>>();

        public void Initialize()
        {
            foreach (var converter in _converters)
            {
                converter.Value.Initialize(this);
            }

            _isInitialized = true;
        }

        public IMsgPackConverter<object> NullConverter => SharedNullConverter;

        public void RegisterConverter<T>(IMsgPackConverter<T> converter)
        {
            _converters[typeof(T)] = converter;
        }

        public void RegisterGenericConverter(Type type)
        {
            var converterType= GetGenericInterface(type, typeof(IMsgPackConverter<>));
            if (converterType == null)
            {
                throw new ArgumentException($"Error registering generic converter. Expected IMsgPackConverter<> implementation, but got {type}");
            }

            var convertedType = converterType.GenericTypeArguments.Single().GetGenericTypeDefinition();
            _genericConverters.Add(convertedType, type);
        }

        public IMsgPackConverter<T> GetConverter<T>()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("MsgPackContext is not initialized yet!");
            }

            var type = typeof(T);
            var result = (IMsgPackConverter<T>)GetConverterFromCache<T>();
            if (result != null)
                return result;

            result = (IMsgPackConverter<T>) (TryGenerateConverterFromGenericConverter(type)
                ?? TryGenerateArrayConverter(type)
                ?? TryGenerateMapConverter(type)
                ?? TryGenerateNullableConverter(type));

            result.Initialize(this);

            return result;
        }

        public Func<object> GetObjectActivator(Type type)
        {
            return _objectActivators.GetOrAdd(type, CompiledLambdaActivatorFactory.GetActivator);
        }

        private IMsgPackConverter TryGenerateConverterFromGenericConverter(Type type)
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
            return _converters.GetOrAdd(converterType, x => (IMsgPackConverter)GetObjectActivator(x)());
        }

        private IMsgPackConverter TryGenerateMapConverter(Type type)
        {
            var mapInterface = GetGenericInterface(type, typeof(IDictionary<,>));
            if (mapInterface != null)
            {
                return _converters.GetOrAdd(type, x => (IMsgPackConverter)GetObjectActivator(typeof(MapConverter<,,>).MakeGenericType(
                    x,
                    mapInterface.GenericTypeArguments[0],
                    mapInterface.GenericTypeArguments[1]))());
            }

            mapInterface = GetGenericInterface(type, typeof(IReadOnlyDictionary<,>));
            if (mapInterface != null)
            {
                return _converters.GetOrAdd(type, x => (IMsgPackConverter)GetObjectActivator(typeof(ReadOnlyMapConverter<,,>).MakeGenericType(
                    x,
                    mapInterface.GenericTypeArguments[0],
                    mapInterface.GenericTypeArguments[1]))());
            }

            return null;
        }

        private IMsgPackConverter TryGenerateNullableConverter(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (!typeInfo.IsGenericType || typeInfo.GetGenericTypeDefinition() != typeof (Nullable<>))
            {
                return null;
            }

            return _converters.GetOrAdd(type, x => (IMsgPackConverter)GetObjectActivator(typeof(NullableConverter<>).MakeGenericType(x.GetTypeInfo().GenericTypeArguments[0]))());
        }

        private IMsgPackConverter TryGenerateArrayConverter(Type type)
        {
            var arrayInterface = GetGenericInterface(type, typeof(IList<>));
            if (arrayInterface != null)
            {
                return _converters.GetOrAdd(type, x => (IMsgPackConverter)GetObjectActivator(typeof(ArrayConverter<,>).MakeGenericType(x, arrayInterface.GenericTypeArguments[0]))());
            }

            arrayInterface = GetGenericInterface(type, typeof(IReadOnlyList<>));
            return arrayInterface != null
                ? _converters.GetOrAdd(type, x => (IMsgPackConverter)GetObjectActivator(typeof(ReadOnlyListConverter<,>).MakeGenericType(x, arrayInterface.GenericTypeArguments[0]))())
                : null;
        }

        private IMsgPackConverter GetConverterFromCache<T>()
        {
            IMsgPackConverter temp;

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