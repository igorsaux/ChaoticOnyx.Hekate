using System;
using BenchmarkDotNet.Running;

namespace ChaoticOnyx.Hekate.Benchmark
{
    public static class Program
    {
        public static void Main()
        {
            var summary = BenchmarkRunner.Run<ParsingEnvironmentBenchmark>();
        }
    }
}
