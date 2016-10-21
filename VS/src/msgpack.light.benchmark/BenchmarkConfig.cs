using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;

namespace ProGaudi.MsgPack.Light.benchmark
{
    internal class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(CsvMeasurementsExporter.Default);
        }
    }
}