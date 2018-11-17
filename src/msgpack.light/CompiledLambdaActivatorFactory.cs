using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ProGaudi.MsgPack
{
    public static class CompiledLambdaActivatorFactory
    {
        private static readonly Dictionary<Type, Func<object>> DefaultConstructorCache = new Dictionary<Type, Func<object>>();

        private static readonly Dictionary<Type, Func<MsgPackContext, object>> SingleConstructorCache = new Dictionary<Type, Func<MsgPackContext, object>>();

        public static Func<object> GetDefaultActivator(this Type type)
        {
            if (DefaultConstructorCache.TryGetValue(type, out var value))
                return value;

            return DefaultConstructorCache[type] = CreateActivator();

            Func<object> CreateActivator()
            {
                var ctor = type
                    .GetTypeInfo()
                    .DeclaredConstructors
                    .FirstOrDefault(x => x.GetParameters().Length == 0 && !x.IsStatic);

                if (ctor == null)
                {
                    return default;
                }

                var newExp = Expression.New(ctor);

                var lambda = Expression.Lambda(typeof(Func<object>), newExp);

                return (Func<object>) lambda.Compile();
            }
        }

        public static Func<MsgPackContext, object> GetContextActivator(this Type type)
        {
            if (SingleConstructorCache.TryGetValue(type, out var value))
                return value;

            return SingleConstructorCache[type] = CreateActivator();

            Func<MsgPackContext, object> CreateActivator()
            {
                var ctor = type
                    .GetTypeInfo()
                    .DeclaredConstructors
                    .FirstOrDefault(x => x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(MsgPackContext) && !x.IsStatic);

                if (ctor == null)
                {
                    return default;
                }

                var parameter = Expression.Parameter(typeof(MsgPackContext), "context");
                var newExp = Expression.New(ctor, parameter);

                var lambda = Expression.Lambda(typeof(Func<MsgPackContext, object>), newExp, parameter);

                return (Func<MsgPackContext, object>) lambda.Compile();
            }
        }
    }
}
