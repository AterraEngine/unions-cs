// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using AterraEngine.Unions.Generator;
using JetBrains.Annotations;
using System;
using Xunit;

namespace Tests.AterraEngine.Unions.Generator;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnionGeneratorTests : IncrementalGeneratorTest<UnionGeneratorTests, UnionGenerator> {
    protected override Type[] ReferenceTypes { get; } = [
        typeof(object),
        typeof(IUnion<>),
        typeof(UnionAliasesAttribute),
        typeof(ValueTuple) // For tuples
    ];

    [Theory]
    [InlineData(TrueOrFalseInput, TrueOrFalseOutput)]
    // [InlineData(TupleOrFalseInput, TupleOrFalseOutput)]
    // [InlineData(SucceededOrFalseInput, SucceededOrFalseOutput)]
    // [InlineData(NothingOrSomethingInput, NothingOrSomethingOutput)]
    // [InlineData(TrueFalseOrAliasInput, TrueFalseOrAliasOutput)]
    public void TestText(string inputText, string expectedOutput) {
        TestGenerator(inputText, expectedOutput, predicate: result => result.HintName.EndsWith("_Union.g.cs"));
    }

    #region Original Test
    [LanguageInjection("csharp")] private const string TrueOrFalseInput = """
        namespace TestNamespace {
            public struct True;
            public struct False;
            
            public readonly partial struct TrueOrFalse() : AterraEngine.Unions.IUnion<True, False> {
                public static implicit operator TrueOrFalse(bool value) => new() {
                    Value = value,
                    IsTrue = value,
                    IsFalse = !value
                };
            }
        }
        """;

    [LanguageInjection("csharp")] private const string TrueOrFalseOutput = """
        // <auto-generated />
        using System;
        using TestNamespace;
        namespace TestNamespace {
            public readonly partial struct TrueOrFalse {
                public object Value { get; init; } = default!;
                public bool IsTrue { get; init; } = false;
                public TestNamespace.True AsTrue => (TestNamespace.True)Value;
                public bool TryGetAsTrue(out TestNamespace.True value) {
                    if (IsTrue) {
                        value = AsTrue;
                        return true;
                    }
                    value = default;
                    return false;
                }
                public static implicit operator TrueOrFalse(TestNamespace.True value) => new TrueOrFalse() {
                    Value = value,
                    IsTrue = true
                };
                public bool IsFalse { get; init; } = false;
                public TestNamespace.False AsFalse => (TestNamespace.False)Value;
                public bool TryGetAsFalse(out TestNamespace.False value) {
                    if (IsFalse) {
                        value = AsFalse;
                        return true;
                    }
                    value = default;
                    return false;
                }
                public static implicit operator TrueOrFalse(TestNamespace.False value) => new TrueOrFalse() {
                    Value = value,
                    IsFalse = true
                };
            }
        }
        """;
    #endregion

    #region Tuple Test
    [LanguageInjection("csharp")] private const string TupleOrFalseInput = """
        using System;
        namespace TestNamespace;

        public readonly struct Success<T> {
            public T Value { get; init; }
        }
        public struct None;
        public struct False;
                
        public readonly partial struct TupleOrFalse() : AterraEngine.Unions.IUnion<(Success<string>, None), False> { }
        """;

    [LanguageInjection("csharp")] private const string TupleOrFalseOutput = """
        // <auto-generated />
        using System;
        using TestNamespace;
        namespace TestNamespace {
            public readonly partial struct TupleOrFalse {
                public object Value { get; init; } = default!;
                public bool IsTupleOfSuccessAndNone { get; init; } = false;
                public (TestNamespace.Success<string>, TestNamespace.None) AsTupleOfSuccessAndNone => ((TestNamespace.Success<string>, TestNamespace.None))Value;
                public bool TryGetAsTupleOfSuccessAndNone(out (TestNamespace.Success<string>, TestNamespace.None) value) {
                    if (IsTupleOfSuccessAndNone) {
                        value = AsTupleOfSuccessAndNone;
                        return true;
                    }
                    value = default;
                    return false;
                }
                public static implicit operator TupleOrFalse((TestNamespace.Success<string>, TestNamespace.None) value) => new TupleOrFalse() {
                    Value = value,
                    IsTupleOfSuccessAndNone = true
                };
                public bool IsFalse { get; init; } = false;
                public TestNamespace.False AsFalse => (TestNamespace.False)Value;
                public bool TryGetAsFalse(out TestNamespace.False value) {
                    if (IsFalse) {
                        value = AsFalse;
                        return true;
                    }
                    value = default;
                    return false;
                }
                public static implicit operator TupleOrFalse(TestNamespace.False value) => new TupleOrFalse() {
                    Value = value,
                    IsFalse = true
                };
            }
        }
        """;
    #endregion

    #region Alias Test
    [LanguageInjection("csharp")] private const string SucceededOrFalseInput = """
        namespace TestNamespace;

        public readonly struct Success<T> {
            public T Value { get; init; }
        }
        public struct None;
        public struct False;

        [AterraEngine.Unions.UnionAliases("Succeeded")]
        public readonly partial struct SucceededOrFalse() : AterraEngine.Unions.IUnion<(Success<string>, None), False> { }
        """;

    [LanguageInjection("csharp")] private const string SucceededOrFalseOutput = """
        // <auto-generated />
        using System;
        using TestNamespace;
        namespace TestNamespace {
            public readonly partial struct SucceededOrFalse {
                public object Value { get; init; } = default!;
                public bool IsSucceeded { get; init; } = false;
                public (TestNamespace.Success<string>, TestNamespace.None) AsSucceeded => ((TestNamespace.Success<string>, TestNamespace.None))Value;
                public bool TryGetAsSucceeded(out (TestNamespace.Success<string>, TestNamespace.None) value) {
                    if (IsSucceeded) {
                        value = AsSucceeded;
                        return true;
                    }
                    value = default;
                    return false;
                }
                public static implicit operator SucceededOrFalse((TestNamespace.Success<string>, TestNamespace.None) value) => new SucceededOrFalse() {
                    Value = value,
                    IsSucceeded = true
                };
                public bool IsFalse { get; init; } = false;
                public TestNamespace.False AsFalse => (TestNamespace.False)Value;
                public bool TryGetAsFalse(out TestNamespace.False value) {
                    if (IsFalse) {
                        value = AsFalse;
                        return true;
                    }
                    value = default;
                    return false;
                }
                public static implicit operator SucceededOrFalse(TestNamespace.False value) => new SucceededOrFalse() {
                    Value = value,
                    IsFalse = true
                };
            }
        }
        """;
    #endregion
    
    #region Alias All Test
    [LanguageInjection("csharp")] private const string NothingOrSomethingInput = """
        namespace TestNamespace;

        public struct True;
        public struct False;

        [AterraEngine.Unions.UnionAliases("Nothing", "Something")]
        public readonly partial struct NothingOrSomething() : AterraEngine.Unions.IUnion<True, False> { }
        """;

    [LanguageInjection("csharp")] private const string NothingOrSomethingOutput = """
        // <auto-generated />
        using System;
        using TestNamespace;
        namespace TestNamespace {
            public readonly partial struct NothingOrSomething {
                public object Value { get; init; } = default!;
                public bool IsNothing { get; init; } = false;
                public TestNamespace.True AsNothing => (TestNamespace.True)Value;
                public bool TryGetAsNothing(out TestNamespace.True value) {
                    if (IsNothing) {
                        value = AsNothing;
                        return true;
                    }
                    value = default;
                    return false;
                }
                public static implicit operator NothingOrSomething(TestNamespace.True value) => new NothingOrSomething() {
                    Value = value,
                    IsNothing = true
                };
                public bool IsSomething { get; init; } = false;
                public TestNamespace.False AsSomething => (TestNamespace.False)Value;
                public bool TryGetAsSomething(out TestNamespace.False value) {
                    if (IsSomething) {
                        value = AsSomething;
                        return true;
                    }
                    value = default;
                    return false;
                }
                public static implicit operator NothingOrSomething(TestNamespace.False value) => new NothingOrSomething() {
                    Value = value,
                    IsSomething = true
                };
            }
        }
        """;
    #endregion
    
    #region Alias Skip Test
    [LanguageInjection("csharp")] private const string TrueFalseOrAliasInput = """
        namespace TestNamespace;

        public struct True;
        public struct False;
        public struct Done;

        [AterraEngine.Unions.UnionAliases(aliasT2: "Alias")]
        public readonly partial struct TrueFalseOrAlias() : AterraEngine.Unions.IUnion<True, False, Done> { }
        """;

    [LanguageInjection("csharp")] private const string TrueFalseOrAliasOutput = """
        // <auto-generated />
        using System;
        using TestNamespace;
        namespace TestNamespace {
            public readonly partial struct TrueFalseOrAlias {
                public object Value { get; init; } = default!;
                public bool IsTrue { get; init; } = false;
                public TestNamespace.True AsTrue => (TestNamespace.True)Value;
                public bool TryGetAsTrue(out TestNamespace.True value) {
                    if (IsTrue) {
                        value = AsTrue;
                        return true;
                    }
                    value = default;
                    return false;
                }
                public static implicit operator TrueFalseOrAlias(TestNamespace.True value) => new TrueFalseOrAlias() {
                    Value = value,
                    IsTrue = true
                };
                public bool IsFalse { get; init; } = false;
                public TestNamespace.False AsFalse => (TestNamespace.False)Value;
                public bool TryGetAsFalse(out TestNamespace.False value) {
                    if (IsFalse) {
                        value = AsFalse;
                        return true;
                    }
                    value = default;
                    return false;
                }
                public static implicit operator TrueFalseOrAlias(TestNamespace.False value) => new TrueFalseOrAlias() {
                    Value = value,
                    IsFalse = true
                };
                public bool IsAlias { get; init; } = false;
                public TestNamespace.Done AsAlias => (TestNamespace.Done)Value;
                public bool TryGetAsAlias(out TestNamespace.Done value) {
                    if (IsAlias) {
                        value = AsAlias;
                        return true;
                    }
                    value = default;
                    return false;
                }
                public static implicit operator TrueFalseOrAlias(TestNamespace.Done value) => new TrueFalseOrAlias() {
                    Value = value,
                    IsAlias = true
                };
            }
        }
        """;
    #endregion
}