// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Benchmarks.AterraEngine.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[MemoryDiagnoser]// Adds memory allocation info
[Orderer(SummaryOrderPolicy.FastestToSlowest)]// Orders results by speed
public class DiscriminatedUnionsBenchmarkRecursiveReturns {

    [Params(10, 100, 1000)]
    public int RecursiveDepth { get; set; }

    [Params( // Short string
        "value",

        // A string longer than 64 bytes
        "This is a sample string that contains more than 64 bytes of text data.",

        // An even longer string (significantly larger than 64 bytes)
        "This is an even longer string intended to test performance when working with values " +
        "of significant length that exceed normal expected sizes.",

        // A string with whitespace and special characters
        "This string includes whitespace, numbers (12345), and special characters !@#$%^&*().",

        // A very large string for stress testing (e.g., more than 256 bytes)
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor " +
        "incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud " +
        "exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute " +
        "irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla " +
        "pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia " +
        "deserunt mollit anim id est laborum."
    )]
    public string Value { get; set; } = string.Empty;


    [Benchmark(OperationsPerInvoke = 1000)]
    public Union_T8 Union_T8() {
        Union_T8 union = Value;
        return RecursiveMethod(union, RecursiveDepth);
    }

    [Benchmark(OperationsPerInvoke = 1000)]
    public RefUnion_T8 RefUnion_T8() {
        RefUnion_T8 union = Value;
        return RecursiveMethod(union, RecursiveDepth);
    }
    

    private T RecursiveMethod<T>(T union, int depth) {
        if (depth <= 0) return union;
        return RecursiveMethod(union, depth - 1);
    }

}

// BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476)
// Unknown processor
// .NET SDK 9.0.201
//   [Host]     : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
//   DefaultJob : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
// 
// 
// | Method      | RecursiveDepth | Value                | Mean      | Error     | StdDev    | Median    | Gen0   | Allocated |
// |------------ |--------------- |--------------------- |----------:|----------:|----------:|----------:|-------:|----------:|
// | RefUnion_T8 | 10             | This(...)zes. [140]  | 0.0088 ns | 0.0002 ns | 0.0005 ns | 0.0089 ns | 0.0000 |         - |
// | RefUnion_T8 | 10             | This (...)&*(). [84] | 0.0093 ns | 0.0002 ns | 0.0003 ns | 0.0093 ns | 0.0000 |         - |
// | RefUnion_T8 | 10             | value                | 0.0096 ns | 0.0002 ns | 0.0002 ns | 0.0097 ns | 0.0000 |         - |
// | RefUnion_T8 | 10             | This (...)data. [70] | 0.0100 ns | 0.0002 ns | 0.0004 ns | 0.0099 ns | 0.0000 |         - |
// | RefUnion_T8 | 10             | Lore(...)rum. [445]  | 0.0106 ns | 0.0005 ns | 0.0014 ns | 0.0099 ns | 0.0000 |         - |
// | Union_T8    | 10             | value                | 0.0112 ns | 0.0001 ns | 0.0001 ns | 0.0112 ns |      - |         - |
// | Union_T8    | 10             | Lore(...)rum. [445]  | 0.0112 ns | 0.0002 ns | 0.0001 ns | 0.0112 ns |      - |         - |
// | Union_T8    | 10             | This (...)&*(). [84] | 0.0112 ns | 0.0000 ns | 0.0000 ns | 0.0112 ns |      - |         - |
// | Union_T8    | 10             | This(...)zes. [140]  | 0.0117 ns | 0.0001 ns | 0.0001 ns | 0.0116 ns |      - |         - |
// | Union_T8    | 10             | This (...)data. [70] | 0.0123 ns | 0.0002 ns | 0.0002 ns | 0.0123 ns |      - |         - |
// | Union_T8    | 100            | value                | 0.0274 ns | 0.0001 ns | 0.0001 ns | 0.0274 ns |      - |         - |
// | Union_T8    | 100            | This (...)&*(). [84] | 0.0280 ns | 0.0002 ns | 0.0001 ns | 0.0279 ns |      - |         - |
// | Union_T8    | 100            | This (...)data. [70] | 0.0280 ns | 0.0001 ns | 0.0001 ns | 0.0280 ns |      - |         - |
// | Union_T8    | 100            | This(...)zes. [140]  | 0.0280 ns | 0.0002 ns | 0.0001 ns | 0.0280 ns |      - |         - |
// | Union_T8    | 100            | Lore(...)rum. [445]  | 0.0284 ns | 0.0002 ns | 0.0001 ns | 0.0284 ns |      - |         - |
// | RefUnion_T8 | 100            | Lore(...)rum. [445]  | 0.0594 ns | 0.0023 ns | 0.0067 ns | 0.0620 ns | 0.0000 |         - |
// | RefUnion_T8 | 100            | value                | 0.0599 ns | 0.0026 ns | 0.0077 ns | 0.0640 ns | 0.0000 |         - |
// | RefUnion_T8 | 100            | This(...)zes. [140]  | 0.0636 ns | 0.0006 ns | 0.0006 ns | 0.0637 ns | 0.0000 |         - |
// | RefUnion_T8 | 100            | This (...)data. [70] | 0.0640 ns | 0.0013 ns | 0.0023 ns | 0.0644 ns | 0.0000 |         - |
// | RefUnion_T8 | 100            | This (...)&*(). [84] | 0.0649 ns | 0.0002 ns | 0.0001 ns | 0.0649 ns | 0.0000 |         - |
// | Union_T8    | 1000           | value                | 0.2146 ns | 0.0005 ns | 0.0004 ns | 0.2146 ns |      - |         - |
// | Union_T8    | 1000           | This (...)&*(). [84] | 0.2149 ns | 0.0002 ns | 0.0002 ns | 0.2149 ns |      - |         - |
// | Union_T8    | 1000           | This(...)zes. [140]  | 0.2151 ns | 0.0003 ns | 0.0003 ns | 0.2151 ns |      - |         - |
// | Union_T8    | 1000           | This (...)data. [70] | 0.2154 ns | 0.0005 ns | 0.0004 ns | 0.2153 ns |      - |         - |
// | Union_T8    | 1000           | Lore(...)rum. [445]  | 0.2161 ns | 0.0005 ns | 0.0005 ns | 0.2160 ns |      - |         - |
// | RefUnion_T8 | 1000           | This(...)zes. [140]  | 0.4070 ns | 0.0006 ns | 0.0005 ns | 0.4070 ns | 0.0000 |         - |
// | RefUnion_T8 | 1000           | This (...)data. [70] | 0.4727 ns | 0.0247 ns | 0.0728 ns | 0.4183 ns | 0.0000 |         - |
// | RefUnion_T8 | 1000           | Lore(...)rum. [445]  | 0.5147 ns | 0.0244 ns | 0.0720 ns | 0.5277 ns | 0.0000 |         - |
// | RefUnion_T8 | 1000           | This (...)&*(). [84] | 0.5275 ns | 0.0197 ns | 0.0575 ns | 0.5408 ns | 0.0000 |         - |
// | RefUnion_T8 | 1000           | value                | 0.5535 ns | 0.0111 ns | 0.0236 ns | 0.5605 ns | 0.0000 |         - |
