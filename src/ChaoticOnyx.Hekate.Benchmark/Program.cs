using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace ChaoticOnyx.Hekate.Benchmark
{
    public static class Program
    {
        public static void Main()
        {
            Summary? _ = BenchmarkRunner.Run<ParsingEnvironmentBenchmark>();
        }
    }
}
