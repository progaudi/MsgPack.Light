using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    internal class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(MarkdownExporter.GitHub);
            Add(CsvMeasurementsExporter.Default);
            Add(MemoryDiagnoser.Default);

            Add(Job.Clr, Job.Core);
        }
    }
}