using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace ChaoticOnyx.Hekate.Benchmark
{
    [MarkdownExporterAttribute.GitHub]
    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.Monitoring, 1, 0, 5)]
    public class ParsingEnvironmentBenchmark
    {
        public string DmeFile { get; }

        public ParsingEnvironmentBenchmark()
        {
            DmeFile = Environment.GetEnvironmentVariable("HEKATE_BENCH_DME") ?? throw new InvalidOperationException("Переменная среды 'HEKATE_BENCH_DME' не установлена.");
        }

        [Benchmark]
        public void Parse()
        {
            Linter.Environment env = new(new FileInfo(DmeFile));
            env.Load();
        }
    }
}
