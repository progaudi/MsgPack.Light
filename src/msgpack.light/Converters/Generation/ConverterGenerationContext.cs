using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;

namespace ProGaudi.MsgPack.Light.Converters.Generation
{
    internal class ConverterGenerationContext
    {
        private readonly AssemblyBuilder _asmBuilder;

        private readonly string _name;

        private readonly string _namespace = "ProGaudi.MsgPack.Light.Generated";

        private readonly MapConverterGenerator _mapGenerator;

        private readonly InterfaceStubGenerator _interfaceStubGenerator;

        private readonly ConcurrentDictionary<Type, Type> _implementationCaches = new ConcurrentDictionary<Type, Type>();

        private readonly ConcurrentDictionary<(Type, Type), Type> _converterCaches = new ConcurrentDictionary<(Type, Type), Type>();

        public ConverterGenerationContext()
        {
            _name = Guid.NewGuid().ToString("N");
#if NET46 || NET45
            _asmBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(_name), AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = _asmBuilder.DefineDynamicModule("main", $"{_name}.dll");
#else
            _asmBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(_name), AssemblyBuilderAccess.RunAndCollect);
            var moduleBuilder = _asmBuilder.DefineDynamicModule("main");
#endif
            _mapGenerator = new MapConverterGenerator(moduleBuilder, _namespace);
            _interfaceStubGenerator = new InterfaceStubGenerator(moduleBuilder, _namespace);
        }

        public IMsgPackConverter GenerateConverter(Type type)
        {
            return GenerateConverter(type, DetectImplementation(type));
        }

        public IMsgPackConverter GenerateConverter(Type @interface, Type implementation)
        {
            var generatorType = _converterCaches.GetOrAdd((@interface, implementation), x => _mapGenerator.Generate(x.Item1, x.Item2));
            return (IMsgPackConverter) generatorType.GetActivator()();
        }

        private Type DetectImplementation(Type @interface)
        {
            return _implementationCaches.GetOrAdd(
                @interface,
                type =>
                {
                    var typeInfo = type.GetTypeInfo();
                    var isInterface = typeInfo.IsInterface;
                    if (!isInterface)
                    {
                        if (!typeInfo.IsAbstract)
                        {
                            throw new NotImplementedException("Can't generate child type for abstract class");
                        }

                        if (typeInfo.GetConstructor() == null)
                        {
                            throw new NotImplementedException("Can't generate child type for type without default ctor");
                        }
                    }

                    if (typeInfo.IsGenericTypeDefinition)
                    {
                        throw new NotImplementedException("Can't generate generic implementors.");
                    }

                    return typeInfo.IsInterface
                        ? _interfaceStubGenerator.GenerateTypeToInstantinate(type, x => $"{x}Implementation")
                        : type;
                });
        }

        public void Dump()
        {
#if NET46 || NET45
            _asmBuilder.Save($"{_name}.dll");
#else
            throw new NotSupportedException();
#endif
        }
    }
}