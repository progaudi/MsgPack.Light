using System.Linq;

using ProGaudi.MsgPack.Light.Converters.Generation;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Generators.Discovery
{
    public class Properties
    {
        [Fact]
        public void SimpleTest()
        {
            var provider = new PropertyProvider();
            provider.GetProperties(typeof(IC)).Select(x => x.Name).ShouldBe(new [] { "D" });
            provider.GetProperties(typeof(IA)).Select(x => x.Name).ShouldBe(new [] { "D" });
            provider.GetProperties(typeof(IB)).Select(x => x.Name).ShouldBe(new [] { "D" });

            provider.GetProperties(typeof(A)).Select(x => x.Name).ShouldBe(new [] { "D" });
            provider.GetProperties(typeof(B)).Select(x => x.Name).ShouldBe(new [] { "D" });
            provider.GetProperties(typeof(C)).Select(x => x.Name).ShouldBe(new [] { "D" });
        }

        [Fact]
        public void NewShouldBeReturned()
        {
            var provider = new PropertyProvider();
            var properties = provider.GetProperties(typeof(ID));
            properties.Select(x => x.Name).ShouldBe(new[] { "D" });
            properties.Select(x => x.DeclaringType).ShouldBe(new[] {typeof(ID)});
        }

        [Fact]
        public void Regression71()
        {
            var context = new MsgPackContext();
            Should.NotThrow(() => context.GenerateAndRegisterMapConverter<IC, C>());
        }

        [Fact]
        public void Regression70()
        {
            var context = new MsgPackContext();
            Should.NotThrow(() => context.GenerateAndRegisterMapConverter<IC>());
        }

        public class C : B, IC
        {
        }

        public class B : A, IB
        {
        }

        public class A : IA
        {
            public string D { get; set; }
        }

        public interface IC : IB
        {
        }

        public interface IB : IA
        {
        }

        public interface IA
        {
            [MsgPackMapElement("T")]
            string D { get; }
        }

        public interface ID : IA
        {
            new string D { get; }
        }
    }
}
