using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ProGaudi.MsgPack
{
    public static class CompiledLambdaActivatorFactory
    {
        private static readonly ConcurrentDictionary<Type, Func<object>> DefaultConstructorCache = new ConcurrentDictionary<Type, Func<object>>();

        private static readonly ConcurrentDictionary<Type, Func<MsgPackContext, object>> SingleConstructorCache = new ConcurrentDictionary<Type, Func<MsgPackContext, object>>();

        public static Func<object> GetDefaultActivator(this Type type)
        {
            return DefaultConstructorCache.GetOrAdd(type, x => CreateActivator());

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
            return SingleConstructorCache.GetOrAdd(type, x => CreateActivator());

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
