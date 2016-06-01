using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;

namespace msgpack.light.benchmark
{
    internal class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(CsvMeasurementsExporter.Default);
        }
    }
}