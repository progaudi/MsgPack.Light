using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters.Csv;

namespace ProGaudi.MsgPack.Light.benchmark
{
    internal class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(CsvMeasurementsExporter.Default);
            Add(MemoryDiagnoser.Default);
        }
    }
}