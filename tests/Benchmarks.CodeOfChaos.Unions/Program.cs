// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using BenchmarkDotNet.Running;

namespace Benchmarks.CodeOfChaos.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static void Main(string[] args) {
        // BenchmarkRunner.Run<DiscriminatedUnionsBenchmark>();
        BenchmarkRunner.Run<DiscriminatedUnionsBenchmarkEnhanced>();
    }
}
