using System;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class MarkedClassesTest
    {
        [Fact]
        public void DiscoverTest()
        {
            var context = new MsgPackContext();
            context.DiscoverConverters<MarkedClass>();
            context.GetConverter<MarkedClass>().ShouldBeAssignableTo<IMsgPackConverter<MarkedClass>>();
            context.GetConverter<InnerClass>().ShouldBeAssignableTo<IMsgPackConverter<InnerClass>>();

            var emptyContext = new MsgPackContext();

            var discoveredConvertersCount = context.DumpConvertersCache().Count;
            var standardConvertersCount = emptyContext.DumpConvertersCache().Count;
            discoveredConvertersCount.ShouldBeGreaterThan(standardConvertersCount);
            (discoveredConvertersCount - standardConvertersCount).ShouldBe(2);
        }

        [Fact]
        public void SmokeSerialization()
        {
            var oldContext = new MsgPackContext();
            oldContext.RegisterConverter(new InnerClassConverter());

            var newContext = new MsgPackContext();
            newContext.DiscoverConverters<InnerClass>();

            var s = Guid.NewGuid().ToString("B");
            var expected = new InnerClass {B = s};
            MsgPackSerializer.Serialize(expected, newContext).ShouldBe(MsgPackSerializer.Serialize(expected, oldContext));
            MsgPackSerializer.Deserialize<InnerClass>(MsgPackSerializer.Serialize(expected, oldContext), newContext).B.ShouldBe(s);
            MsgPackSerializer.Deserialize<InnerClass>(MsgPackSerializer.Serialize(expected, newContext), oldContext).B.ShouldBe(s);
        }

        [MsgPackMap]
        public class InnerClass
        {
            [MsgPackMapElement("Qwer")]
            public string B { get; set; }
        }

        public class InnerClassConverter : IMsgPackConverter<InnerClass>
        {
            private Lazy<IMsgPackConverter<string>> _stringConverter;

            public void Initialize(MsgPackContext context)
            {
                _stringConverter = new Lazy<IMsgPackConverter<string>>(context.GetConverter<string>);
            }

            public void Write(InnerClass value, IMsgPackWriter writer)
            {
                if (value == null)
                {
                    writer.Write(DataTypes.Null);
                }
                else
                {
                    writer.WriteMapHeader(1U);
                    _stringConverter.Value.Write("Qwer", writer);
                    _stringConverter.Value.Write(value.B, writer);
                }
            }

            public InnerClass Read(IMsgPackReader reader)
            {
                var instance = new InnerClass();
                var nullable = reader.ReadMapLength();
                if (!nullable.HasValue)
                    return null;
                for (var index = 0; index < nullable.Value; ++index)
                {
                    var str = _stringConverter.Value.Read(reader);
                    switch (str)
                    {
                        case "Qwer":
                            instance.B = _stringConverter.Value.Read(reader);
                            break;
                        default:
                            reader.SkipToken();
                            break;
                    }
                }
                return instance;
            }
        }
    }
}
