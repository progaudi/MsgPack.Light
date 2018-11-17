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

        public static Func<object> GetDefaultActivator(this Type type) => DefaultConstructorCache[type] ??
            (DefaultConstructorCache[type] = CreateActivatorFromCtor<Func<object>>(
                type,
                x => x.GetParameters().Length == 0 && !x.IsStatic));

        public static Func<MsgPackContext, object> GetContextActivator(this Type type) => SingleConstructorCache[type] ??
            (SingleConstructorCache[type] = CreateActivatorFromCtor<Func<MsgPackContext, object>>(
                type,
                x => x.GetParameters().Length == 0 && x.GetParameters()[0].ParameterType == typeof(MsgPackContext) && !x.IsStatic));

        private static T CreateActivatorFromCtor<T>(Type type, Func<ConstructorInfo, bool> predicate)
            where T : Delegate
        {
            var ctor = type
                .GetTypeInfo()
                .DeclaredConstructors
                .FirstOrDefault(predicate);

            if (ctor == null)
                return default;

            var newExp = Expression.New(ctor);

            var lambda = Expression.Lambda(typeof(T), newExp);

            return (T)lambda.Compile();
        }
    }
}
