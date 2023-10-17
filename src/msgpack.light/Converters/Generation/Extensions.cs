using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ProGaudi.MsgPack.Light.Converters.Generation
{
    public static class Extensions
    {
        public static Type ToType(this TypeBuilder typeBuilder)
        {
#if NET462
            return typeBuilder.CreateType();
#else
            return typeBuilder.CreateTypeInfo().AsType();
#endif
        }

        public static string GetNormalizedName(this Type type)
        {
            return type.GetTypeInfo().IsInterface && type.Name.StartsWith("I") ? type.Name.Substring(1) : type.Name;
        }

        public static IEnumerable<T> GetMembersFromInterface<T>(this Type typeToWrap, Func<Type, IEnumerable<T>> getter)
        {
            foreach (var member in getter(typeToWrap))
            {
                yield return member;
            }

            foreach (var @interface in typeToWrap.GetTypeInfo().ImplementedInterfaces)
            {
                foreach (var member in GetMembersFromInterface(@interface, getter))
                {
                    yield return member;
                }
            }
        }

        public static ConstructorInfo GetDefaultConstructor(this TypeInfo type)
        {
            return type.GetConstructor(new Type[0]);
        }

        public static string GetMapElementName(this PropertyInfo info)
        {
            return info.GetCustomAttribute<MsgPackMapElementAttribute>().Name;
        }

        public static int GetArrayElementOrder(this PropertyInfo info)
        {
            return info.GetCustomAttribute<MsgPackArrayElementAttribute>().Order;
        }

        public static MethodInfo GetGenericMethod(this TypeInfo type, string name, byte number)
        {
            return type.DeclaredMethods.SingleOrDefault(x => x.Name == name && x.GetGenericArguments().Length == number);
        }
    }
}