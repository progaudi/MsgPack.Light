// <copyright file="PropertyProvider.cs" company="eVote">
//   Copyright © eVote
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace ProGaudi.MsgPack.Light.Converters.Generation
{
    public class PropertyProvider
    {
        private readonly ConcurrentDictionary<Type, ImmutableArray<PropertyInfo>> _cache = new ConcurrentDictionary<Type, ImmutableArray<PropertyInfo>>();

        public ImmutableArray<PropertyInfo> GetProperties(Type type)
        {
            return _cache.GetOrAdd(type, Discover);
        }

        private ImmutableArray<PropertyInfo> Discover(Type type)
        {
            var result = new Dictionary<string, PropertyInfo>();
            foreach (var info in DiscoverProperties(type))
            {
                if (!result.ContainsKey(info.Name))
                    result[info.Name] = info;
            }

            return result.Values.ToImmutableArray();
        }

        private IEnumerable<PropertyInfo> DiscoverProperties(Type type)
        {
            if (type == null)
                yield break;

            var typeInfo = type.GetTypeInfo();
            foreach (var property in typeInfo.DeclaredProperties)
            {
                yield return property;
            }

            if (typeInfo.IsInterface)
            {
                foreach (var property in typeInfo.ImplementedInterfaces.SelectMany(DiscoverProperties))
                {
                    yield return property;
                }
            }
            else
            {
                foreach (var property in DiscoverProperties(typeInfo.BaseType))
                {
                    yield return property;
                }
            }
        }
    }
}