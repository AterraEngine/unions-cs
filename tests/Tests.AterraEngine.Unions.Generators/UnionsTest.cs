// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using AterraEngine.Unions.Generators;
using CodeOfChaos.GeneratorTools;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Assembly=System.Reflection.Assembly;

namespace Tests.AterraEngine.Unions.Generators;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnionGeneratorTests : IncrementalGeneratorTest<UnionGenerator> {
    protected override Assembly[] ReferenceAssemblies { get; } = [
        typeof(object).Assembly,
        typeof(ValueTuple).Assembly,
        typeof(Attribute).Assembly,
        typeof(Console).Assembly,
        Assembly.Load("System.Runtime"),
        Assembly.Load("System.Threading.Tasks"),

        typeof(IUnion<>).Assembly,
        typeof(UnionAliasesAttribute).Assembly,
        typeof(IValue<>).Assembly,
        typeof(GeneratorStringBuilder).Assembly
    ];

    // I hae no Clue why this is not working.
    // It is working in production, but in these tests, it just breaks
    // I might be something related to the IncrementalGeneratorTest<> configuration, but I have no clue.
    [Test]
    [Arguments(TrueOrFalseInput, TrueOrFalseOutput)]
    [Arguments(TupleOrFalseInput, TupleOrFalseOutput)]
    [Arguments(SucceededOrFalseInput, SucceededOrFalseOutput)]
    [Arguments(NothingOrSomethingInput, NothingOrSomethingOutput)]
    [Arguments(TrueFalseOrAliasInput, TrueFalseOrAliasOutput)]
    [Arguments(UnionExtraGenerateFromInput, UnionExtraGenerateFromOutput)]
    [Arguments(UnionExtraGenerateAsValueInput, UnionExtraGenerateAsValueOutput)]
    public async Task TestText(string inputText, string expectedOutput) {

        GeneratorDriverRunResult runResult = await RunGeneratorAsync(inputText);

        GeneratedSourceResult? generatedSource = runResult.Results
            .SelectMany(result => result.GeneratedSources)
            .SingleOrDefault(result => result.HintName.EndsWith("_Union.g.cs"));

        await Assert.That(generatedSource?.SourceText).IsNotNull();
        await Assert.That(generatedSource?.SourceText.ToString())
            .IsEqualTo(expectedOutput).IgnoringWhitespace().WithTrimming();
    }

    #region Original Test
    [LanguageInjection("csharp")] private const string TrueOrFalseInput = """
        namespace TestNamespace {
            public struct True;
            public struct False;
                    
            public readonly partial struct TrueOrFalse() : AterraEngine.Unions.IUnion<True, False> {
                public static implicit operator TrueOrFalse(bool value) => value ? new True() : new False();
            }
        }
        """;

    [LanguageInjection("csharp")] private const string TrueOrFalseOutput = """
        // <auto-generated />
        using System;
        using System.Diagnostics.CodeAnalysis;
        using System.Threading.Tasks;
        namespace TestNamespace;
        #nullable enable
        public readonly partial struct TrueOrFalse {
        
            #region True
            public bool IsTrue { get; private init; } = false;
            public TestNamespace.True AsTrue {get; private init;} = default!;
            public bool TryGetAsTrue(out TestNamespace.True value) {
                if (IsTrue) {
                    value = AsTrue;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TrueOrFalse(TestNamespace.True value) => new TrueOrFalse() {
                IsTrue = true,
                AsTrue = value
            };
            #endregion
        
            #region False
            public bool IsFalse { get; private init; } = false;
            public TestNamespace.False AsFalse {get; private init;} = default!;
            public bool TryGetAsFalse(out TestNamespace.False value) {
                if (IsFalse) {
                    value = AsFalse;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TrueOrFalse(TestNamespace.False value) => new TrueOrFalse() {
                IsFalse = true,
                AsFalse = value
            };
            #endregion
        
            public object? Value { get {
                if (IsTrue) return AsTrue;
                if (IsFalse) return AsFalse;
                throw new ArgumentException("Union does not contain a value");
            }}
        
            #region Match and MatchAsync
            public TOutput Match<TOutput>(Func<TestNamespace.True, TOutput> trueCase,Func<TestNamespace.False, TOutput> falseCase){
                switch (this) {
                    case {IsTrue: true, AsTrue: var value} : return trueCase(value); 
                    case {IsFalse: true, AsFalse: var value} : return falseCase(value); 
                }
                throw new ArgumentException("Union does not contain a value");
            }
        
            public async Task<TOutput> MatchAsync<TOutput>(Func<TestNamespace.True, Task<TOutput>> trueCase,Func<TestNamespace.False, Task<TOutput>> falseCase){
                switch (this) {
                    case {IsTrue: true, AsTrue: var value} : return await trueCase(value); 
                    case {IsFalse: true, AsFalse: var value} : return await falseCase(value); 
                }
                throw new ArgumentException("Union does not contain a value");
            }
            #endregion
        
            #region Switch and SwitchAsync
            public void Switch(Action<TestNamespace.True> trueCase,Action<TestNamespace.False> falseCase){
                switch (this) {
                    case {IsTrue: true, AsTrue: var value} : trueCase(value); return;
                    case {IsFalse: true, AsFalse: var value} : falseCase(value); return;
                }
                throw new ArgumentException("Union does not contain a value");
            }
        
            public async Task SwitchAsync(Func<TestNamespace.True, Task> trueCase,Func<TestNamespace.False, Task> falseCase){
                switch (this) {
                    case {IsTrue: true, AsTrue: var value} : await trueCase(value); return;
                    case {IsFalse: true, AsFalse: var value} : await falseCase(value); return;
                }
                throw new ArgumentException("Union does not contain a value");
            }
            #endregion

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
        using System.Diagnostics.CodeAnalysis;
        using System.Threading.Tasks;
        namespace TestNamespace;
        #nullable enable
        public readonly partial struct TupleOrFalse {
        
            #region SuccessOfStringAndNoneTuple
            public bool IsSuccessOfStringAndNoneTuple { get; private init; } = false;
            public (TestNamespace.Success<string>, TestNamespace.None) AsSuccessOfStringAndNoneTuple {get; private init;} = default!;
            public bool TryGetAsSuccessOfStringAndNoneTuple(out (TestNamespace.Success<string>, TestNamespace.None) value) {
                if (IsSuccessOfStringAndNoneTuple) {
                    value = AsSuccessOfStringAndNoneTuple;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TupleOrFalse((TestNamespace.Success<string>, TestNamespace.None) value) => new TupleOrFalse() {
                IsSuccessOfStringAndNoneTuple = true,
                AsSuccessOfStringAndNoneTuple = value
            };
            #endregion
        
            #region False
            public bool IsFalse { get; private init; } = false;
            public TestNamespace.False AsFalse {get; private init;} = default!;
            public bool TryGetAsFalse(out TestNamespace.False value) {
                if (IsFalse) {
                    value = AsFalse;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TupleOrFalse(TestNamespace.False value) => new TupleOrFalse() {
                IsFalse = true,
                AsFalse = value
            };
            #endregion
        
            public object? Value { get {
                if (IsSuccessOfStringAndNoneTuple) return AsSuccessOfStringAndNoneTuple;
                if (IsFalse) return AsFalse;
                throw new ArgumentException("Union does not contain a value");
            }}
        
            #region Match and MatchAsync
            public TOutput Match<TOutput>(Func<(TestNamespace.Success<string>, TestNamespace.None), TOutput> successofstringandnonetupleCase,Func<TestNamespace.False, TOutput> falseCase){
                switch (this) {
                    case {IsSuccessOfStringAndNoneTuple: true, AsSuccessOfStringAndNoneTuple: var value} : return successofstringandnonetupleCase(value); 
                    case {IsFalse: true, AsFalse: var value} : return falseCase(value); 
                }
                throw new ArgumentException("Union does not contain a value");
            }
        
            public async Task<TOutput> MatchAsync<TOutput>(Func<(TestNamespace.Success<string>, TestNamespace.None), Task<TOutput>> successofstringandnonetupleCase,Func<TestNamespace.False, Task<TOutput>> falseCase){
                switch (this) {
                    case {IsSuccessOfStringAndNoneTuple: true, AsSuccessOfStringAndNoneTuple: var value} : return await successofstringandnonetupleCase(value); 
                    case {IsFalse: true, AsFalse: var value} : return await falseCase(value); 
                }
                throw new ArgumentException("Union does not contain a value");
            }
            #endregion
        
            #region Switch and SwitchAsync
            public void Switch(Action<(TestNamespace.Success<string>, TestNamespace.None)> successofstringandnonetupleCase,Action<TestNamespace.False> falseCase){
                switch (this) {
                    case {IsSuccessOfStringAndNoneTuple: true, AsSuccessOfStringAndNoneTuple: var value} : successofstringandnonetupleCase(value); return;
                    case {IsFalse: true, AsFalse: var value} : falseCase(value); return;
                }
                throw new ArgumentException("Union does not contain a value");
            }
        
            public async Task SwitchAsync(Func<(TestNamespace.Success<string>, TestNamespace.None), Task> successofstringandnonetupleCase,Func<TestNamespace.False, Task> falseCase){
                switch (this) {
                    case {IsSuccessOfStringAndNoneTuple: true, AsSuccessOfStringAndNoneTuple: var value} : await successofstringandnonetupleCase(value); return;
                    case {IsFalse: true, AsFalse: var value} : await falseCase(value); return;
                }
                throw new ArgumentException("Union does not contain a value");
            }
            #endregion

        }
        """;
    #endregion

    #region SucceededOrFalse Test (Alias)
    [LanguageInjection("csharp")] private const string SucceededOrFalseInput = """
        namespace TestNamespace;

        public readonly struct Success<T> {
            public T Value { get; init; }
        }
        public struct None;
        public struct False;

        [AterraEngine.Unions.UnionAliases(aliasT0:"Succeeded")]
        public readonly partial struct SucceededOrFalse() : AterraEngine.Unions.IUnion<(Success<string>, None), False> { }
        """;

    [LanguageInjection("csharp")] private const string SucceededOrFalseOutput = """
        // <auto-generated />
        using System;
        using System.Diagnostics.CodeAnalysis;
        using System.Threading.Tasks;
        namespace TestNamespace;
        #nullable enable
        public readonly partial struct SucceededOrFalse {
        
            #region Succeeded
            public bool IsSucceeded { get; private init; } = false;
            public (TestNamespace.Success<string>, TestNamespace.None) AsSucceeded {get; private init;} = default!;
            public bool TryGetAsSucceeded(out (TestNamespace.Success<string>, TestNamespace.None) value) {
                if (IsSucceeded) {
                    value = AsSucceeded;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator SucceededOrFalse((TestNamespace.Success<string>, TestNamespace.None) value) => new SucceededOrFalse() {
                IsSucceeded = true,
                AsSucceeded = value
            };
            #endregion
        
            #region False
            public bool IsFalse { get; private init; } = false;
            public TestNamespace.False AsFalse {get; private init;} = default!;
            public bool TryGetAsFalse(out TestNamespace.False value) {
                if (IsFalse) {
                    value = AsFalse;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator SucceededOrFalse(TestNamespace.False value) => new SucceededOrFalse() {
                IsFalse = true,
                AsFalse = value
            };
            #endregion
        
            public object? Value { get {
                if (IsSucceeded) return AsSucceeded;
                if (IsFalse) return AsFalse;
                throw new ArgumentException("Union does not contain a value");
            }}
        
            #region Match and MatchAsync
            public TOutput Match<TOutput>(Func<(TestNamespace.Success<string>, TestNamespace.None), TOutput> succeededCase,Func<TestNamespace.False, TOutput> falseCase){
                switch (this) {
                    case {IsSucceeded: true, AsSucceeded: var value} : return succeededCase(value); 
                    case {IsFalse: true, AsFalse: var value} : return falseCase(value); 
                }
                throw new ArgumentException("Union does not contain a value");
            }
        
            public async Task<TOutput> MatchAsync<TOutput>(Func<(TestNamespace.Success<string>, TestNamespace.None), Task<TOutput>> succeededCase,Func<TestNamespace.False, Task<TOutput>> falseCase){
                switch (this) {
                    case {IsSucceeded: true, AsSucceeded: var value} : return await succeededCase(value); 
                    case {IsFalse: true, AsFalse: var value} : return await falseCase(value); 
                }
                throw new ArgumentException("Union does not contain a value");
            }
            #endregion
        
            #region Switch and SwitchAsync
            public void Switch(Action<(TestNamespace.Success<string>, TestNamespace.None)> succeededCase,Action<TestNamespace.False> falseCase){
                switch (this) {
                    case {IsSucceeded: true, AsSucceeded: var value} : succeededCase(value); return;
                    case {IsFalse: true, AsFalse: var value} : falseCase(value); return;
                }
                throw new ArgumentException("Union does not contain a value");
            }
        
            public async Task SwitchAsync(Func<(TestNamespace.Success<string>, TestNamespace.None), Task> succeededCase,Func<TestNamespace.False, Task> falseCase){
                switch (this) {
                    case {IsSucceeded: true, AsSucceeded: var value} : await succeededCase(value); return;
                    case {IsFalse: true, AsFalse: var value} : await falseCase(value); return;
                }
                throw new ArgumentException("Union does not contain a value");
            }
            #endregion

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
        using System.Diagnostics.CodeAnalysis;
        using System.Threading.Tasks;
        namespace TestNamespace;
        #nullable enable
        public readonly partial struct NothingOrSomething {
        
            #region Nothing
            public bool IsNothing { get; private init; } = false;
            public TestNamespace.True AsNothing {get; private init;} = default!;
            public bool TryGetAsNothing(out TestNamespace.True value) {
                if (IsNothing) {
                    value = AsNothing;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator NothingOrSomething(TestNamespace.True value) => new NothingOrSomething() {
                IsNothing = true,
                AsNothing = value
            };
            #endregion
        
            #region Something
            public bool IsSomething { get; private init; } = false;
            public TestNamespace.False AsSomething {get; private init;} = default!;
            public bool TryGetAsSomething(out TestNamespace.False value) {
                if (IsSomething) {
                    value = AsSomething;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator NothingOrSomething(TestNamespace.False value) => new NothingOrSomething() {
                IsSomething = true,
                AsSomething = value
            };
            #endregion
        
            public object? Value { get {
                if (IsNothing) return AsNothing;
                if (IsSomething) return AsSomething;
                throw new ArgumentException("Union does not contain a value");
            }}
        
            #region Match and MatchAsync
            public TOutput Match<TOutput>(Func<TestNamespace.True, TOutput> nothingCase,Func<TestNamespace.False, TOutput> somethingCase){
                switch (this) {
                    case {IsNothing: true, AsNothing: var value} : return nothingCase(value); 
                    case {IsSomething: true, AsSomething: var value} : return somethingCase(value); 
                }
                throw new ArgumentException("Union does not contain a value");
            }
        
            public async Task<TOutput> MatchAsync<TOutput>(Func<TestNamespace.True, Task<TOutput>> nothingCase,Func<TestNamespace.False, Task<TOutput>> somethingCase){
                switch (this) {
                    case {IsNothing: true, AsNothing: var value} : return await nothingCase(value); 
                    case {IsSomething: true, AsSomething: var value} : return await somethingCase(value); 
                }
                throw new ArgumentException("Union does not contain a value");
            }
            #endregion
        
            #region Switch and SwitchAsync
            public void Switch(Action<TestNamespace.True> nothingCase,Action<TestNamespace.False> somethingCase){
                switch (this) {
                    case {IsNothing: true, AsNothing: var value} : nothingCase(value); return;
                    case {IsSomething: true, AsSomething: var value} : somethingCase(value); return;
                }
                throw new ArgumentException("Union does not contain a value");
            }
        
            public async Task SwitchAsync(Func<TestNamespace.True, Task> nothingCase,Func<TestNamespace.False, Task> somethingCase){
                switch (this) {
                    case {IsNothing: true, AsNothing: var value} : await nothingCase(value); return;
                    case {IsSomething: true, AsSomething: var value} : await somethingCase(value); return;
                }
                throw new ArgumentException("Union does not contain a value");
            }
            #endregion

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
        using System.Diagnostics.CodeAnalysis;
        using System.Threading.Tasks;
        namespace TestNamespace;
        #nullable enable
        public readonly partial struct TrueFalseOrAlias {
        
            #region True
            public bool IsTrue { get; private init; } = false;
            public TestNamespace.True AsTrue {get; private init;} = default!;
            public bool TryGetAsTrue(out TestNamespace.True value) {
                if (IsTrue) {
                    value = AsTrue;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TrueFalseOrAlias(TestNamespace.True value) => new TrueFalseOrAlias() {
                IsTrue = true,
                AsTrue = value
            };
            #endregion
        
            #region False
            public bool IsFalse { get; private init; } = false;
            public TestNamespace.False AsFalse {get; private init;} = default!;
            public bool TryGetAsFalse(out TestNamespace.False value) {
                if (IsFalse) {
                    value = AsFalse;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TrueFalseOrAlias(TestNamespace.False value) => new TrueFalseOrAlias() {
                IsFalse = true,
                AsFalse = value
            };
            #endregion
        
            #region Alias
            public bool IsAlias { get; private init; } = false;
            public TestNamespace.Done AsAlias {get; private init;} = default!;
            public bool TryGetAsAlias(out TestNamespace.Done value) {
                if (IsAlias) {
                    value = AsAlias;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TrueFalseOrAlias(TestNamespace.Done value) => new TrueFalseOrAlias() {
                IsAlias = true,
                AsAlias = value
            };
            #endregion
        
            public object? Value { get {
                if (IsTrue) return AsTrue;
                if (IsFalse) return AsFalse;
                if (IsAlias) return AsAlias;
                throw new ArgumentException("Union does not contain a value");
            }}
        
            #region Match and MatchAsync
            public TOutput Match<TOutput>(Func<TestNamespace.True, TOutput> trueCase,Func<TestNamespace.False, TOutput> falseCase,Func<TestNamespace.Done, TOutput> aliasCase){
                switch (this) {
                    case {IsTrue: true, AsTrue: var value} : return trueCase(value); 
                    case {IsFalse: true, AsFalse: var value} : return falseCase(value); 
                    case {IsAlias: true, AsAlias: var value} : return aliasCase(value); 
                }
                throw new ArgumentException("Union does not contain a value");
            }
        
            public async Task<TOutput> MatchAsync<TOutput>(Func<TestNamespace.True, Task<TOutput>> trueCase,Func<TestNamespace.False, Task<TOutput>> falseCase,Func<TestNamespace.Done, Task<TOutput>> aliasCase){
                switch (this) {
                    case {IsTrue: true, AsTrue: var value} : return await trueCase(value); 
                    case {IsFalse: true, AsFalse: var value} : return await falseCase(value); 
                    case {IsAlias: true, AsAlias: var value} : return await aliasCase(value); 
                }
                throw new ArgumentException("Union does not contain a value");
            }
            #endregion
        
            #region Switch and SwitchAsync
            public void Switch(Action<TestNamespace.True> trueCase,Action<TestNamespace.False> falseCase,Action<TestNamespace.Done> aliasCase){
                switch (this) {
                    case {IsTrue: true, AsTrue: var value} : trueCase(value); return;
                    case {IsFalse: true, AsFalse: var value} : falseCase(value); return;
                    case {IsAlias: true, AsAlias: var value} : aliasCase(value); return;
                }
                throw new ArgumentException("Union does not contain a value");
            }
        
            public async Task SwitchAsync(Func<TestNamespace.True, Task> trueCase,Func<TestNamespace.False, Task> falseCase,Func<TestNamespace.Done, Task> aliasCase){
                switch (this) {
                    case {IsTrue: true, AsTrue: var value} : await trueCase(value); return;
                    case {IsFalse: true, AsFalse: var value} : await falseCase(value); return;
                    case {IsAlias: true, AsAlias: var value} : await aliasCase(value); return;
                }
                throw new ArgumentException("Union does not contain a value");
            }
            #endregion

        }
        """;
    #endregion

    #region UnionExtra.GenerateFrom Test
    [LanguageInjection("csharp")] private const string UnionExtraGenerateFromInput = """
        using AterraEngine.Unions;

        namespace TestNamespace;
        public readonly struct Success<T> {
            public T Value { get; init; }
        }

        public struct None;

        public struct False;

        [AterraEngine.Unions.UnionExtra(UnionExtra.GenerateFrom)]
        public readonly partial struct TupleOrFalse() : AterraEngine.Unions.IUnion<(Success<string>, None), False> {}
        """;

    [LanguageInjection("csharp")] private const string UnionExtraGenerateFromOutput = """
        // <auto-generated />
        using System;
        using System.Diagnostics.CodeAnalysis;
        using System.Threading.Tasks;
        namespace TestNamespace;
        #nullable enable
        public readonly partial struct TupleOrFalse {
        
            #region SuccessOfStringAndNoneTuple
            public bool IsSuccessOfStringAndNoneTuple { get; private init; } = false;
            public (TestNamespace.Success<string>, TestNamespace.None) AsSuccessOfStringAndNoneTuple {get; private init;} = default!;
            public bool TryGetAsSuccessOfStringAndNoneTuple(out (TestNamespace.Success<string>, TestNamespace.None) value) {
                if (IsSuccessOfStringAndNoneTuple) {
                    value = AsSuccessOfStringAndNoneTuple;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TupleOrFalse((TestNamespace.Success<string>, TestNamespace.None) value) => new TupleOrFalse() {
                IsSuccessOfStringAndNoneTuple = true,
                AsSuccessOfStringAndNoneTuple = value
            };
            public static TupleOrFalse FromSuccessOfStringAndNoneTuple((TestNamespace.Success<string>, TestNamespace.None) value) => new TupleOrFalse() {
                IsSuccessOfStringAndNoneTuple = true,
                AsSuccessOfStringAndNoneTuple = value
            };
            #endregion
        
            #region False
            public bool IsFalse { get; private init; } = false;
            public TestNamespace.False AsFalse {get; private init;} = default!;
            public bool TryGetAsFalse(out TestNamespace.False value) {
                if (IsFalse) {
                    value = AsFalse;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TupleOrFalse(TestNamespace.False value) => new TupleOrFalse() {
                IsFalse = true,
                AsFalse = value
            };
            public static TupleOrFalse FromFalse(TestNamespace.False value) => new TupleOrFalse() {
                IsFalse = true,
                AsFalse = value
            };
            #endregion
        
            public object? Value { get {
                if (IsSuccessOfStringAndNoneTuple) return AsSuccessOfStringAndNoneTuple;
                if (IsFalse) return AsFalse;
                throw new ArgumentException("Union does not contain a value");
            }}
        
            #region Match and MatchAsync
            public TOutput Match<TOutput>(Func<(TestNamespace.Success<string>, TestNamespace.None), TOutput> successofstringandnonetupleCase,Func<TestNamespace.False, TOutput> falseCase){
                switch (this) {
                    case {IsSuccessOfStringAndNoneTuple: true, AsSuccessOfStringAndNoneTuple: var value} : return successofstringandnonetupleCase(value); 
                    case {IsFalse: true, AsFalse: var value} : return falseCase(value); 
                }
                throw new ArgumentException("Union does not contain a value");
            }
        
            public async Task<TOutput> MatchAsync<TOutput>(Func<(TestNamespace.Success<string>, TestNamespace.None), Task<TOutput>> successofstringandnonetupleCase,Func<TestNamespace.False, Task<TOutput>> falseCase){
                switch (this) {
                    case {IsSuccessOfStringAndNoneTuple: true, AsSuccessOfStringAndNoneTuple: var value} : return await successofstringandnonetupleCase(value); 
                    case {IsFalse: true, AsFalse: var value} : return await falseCase(value); 
                }
                throw new ArgumentException("Union does not contain a value");
            }
            #endregion
        
            #region Switch and SwitchAsync
            public void Switch(Action<(TestNamespace.Success<string>, TestNamespace.None)> successofstringandnonetupleCase,Action<TestNamespace.False> falseCase){
                switch (this) {
                    case {IsSuccessOfStringAndNoneTuple: true, AsSuccessOfStringAndNoneTuple: var value} : successofstringandnonetupleCase(value); return;
                    case {IsFalse: true, AsFalse: var value} : falseCase(value); return;
                }
                throw new ArgumentException("Union does not contain a value");
            }
        
            public async Task SwitchAsync(Func<(TestNamespace.Success<string>, TestNamespace.None), Task> successofstringandnonetupleCase,Func<TestNamespace.False, Task> falseCase){
                switch (this) {
                    case {IsSuccessOfStringAndNoneTuple: true, AsSuccessOfStringAndNoneTuple: var value} : await successofstringandnonetupleCase(value); return;
                    case {IsFalse: true, AsFalse: var value} : await falseCase(value); return;
                }
                throw new ArgumentException("Union does not contain a value");
            }
            #endregion

        }
        """;
    #endregion

    #region UnionExtra.GenerateAsValue Test
    [LanguageInjection("csharp")] private const string UnionExtraGenerateAsValueInput = """
        namespace TestNamespace;

        public readonly struct Success<T> : AterraEngine.Unions.IValue<T> {
            public T Value { get; init; }
        }
        public readonly struct SuccessMany<T> : AterraEngine.Unions.IValues<T> {
            public T Values { get; init; }
        }

        [AterraEngine.Unions.UnionExtra(AterraEngine.Unions.UnionExtra.GenerateAsValue)]
        public readonly partial struct TupleOrFalse() : AterraEngine.Unions.IUnion<Success<string>, SuccessMany<int[]>>;
        """;

    [LanguageInjection("csharp")] private const string UnionExtraGenerateAsValueOutput = """
        // <auto-generated />
        using System;
        using System.Diagnostics.CodeAnalysis;
        using System.Threading.Tasks;
        
        namespace TestNamespace;
        #nullable enable
        public readonly partial struct TupleOrFalse {
        
            #region SuccessOfString
            public bool IsSuccessOfString { get; private init; } = false;
            public TestNamespace.Success<string> AsSuccessOfString {get; private init;} = default!;
            public bool TryGetAsSuccessOfString(out TestNamespace.Success<string> value) {
                if (IsSuccessOfString) {
                    value = AsSuccessOfString;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TupleOrFalse(TestNamespace.Success<string> value) => new TupleOrFalse() {
                IsSuccessOfString = true,
                AsSuccessOfString = value
            };
            public bool TryGetAsSuccessOfStringValue([NotNullWhen(true)] out string? value) {
                if (IsSuccessOfString) {
                    value = AsSuccessOfString.Value;
                    return true;
                }
                value = default;
                return false;
            }
            #endregion
            #region SuccessManyOfInt32Array
            public bool IsSuccessManyOfInt32Array { get; private init; } = false;
            public TestNamespace.SuccessMany<int[]> AsSuccessManyOfInt32Array {get; private init;} = default!;
            public bool TryGetAsSuccessManyOfInt32Array(out TestNamespace.SuccessMany<int[]> value) {
                if (IsSuccessManyOfInt32Array) {
                    value = AsSuccessManyOfInt32Array;
                    return true;
                }
                value = default;
                return false;
            }
            public static implicit operator TupleOrFalse(TestNamespace.SuccessMany<int[]> value) => new TupleOrFalse() {
                IsSuccessManyOfInt32Array = true,
                AsSuccessManyOfInt32Array = value
            };
            public bool TryGetAsSuccessManyOfInt32ArrayValues([NotNullWhen(true)] out int[]? values) {
                if (IsSuccessManyOfInt32Array) {
                    values = AsSuccessManyOfInt32Array.Values;
                    return true;
                }
                values = default;
                return false;
            }
            #endregion
            public object? Value { get {
                if (IsSuccessOfString) return AsSuccessOfString;
                if (IsSuccessManyOfInt32Array) return AsSuccessManyOfInt32Array;
                throw new ArgumentException("Union does not contain a value");
            }}
        
            #region Match and MatchAsync
            public TOutput Match<TOutput>(Func<TestNamespace.Success<string>, TOutput> successofstringCase,Func<TestNamespace.SuccessMany<int[]>, TOutput> successmanyofint32arrayCase){
                switch (this) {
                    case {IsSuccessOfString: true, AsSuccessOfString: var value} : return successofstringCase(value);
                    case {IsSuccessManyOfInt32Array: true, AsSuccessManyOfInt32Array: var value} : return successmanyofint32arrayCase(value);
                }
                throw new ArgumentException("Union does not contain a value");
            }
        
            public async Task<TOutput> MatchAsync<TOutput>(Func<TestNamespace.Success<string>, Task<TOutput>> successofstringCase,Func<TestNamespace.SuccessMany<int[]>, Task<TOutput>> successmanyofint32arrayCase){
                switch (this) {
                    case {IsSuccessOfString: true, AsSuccessOfString: var value} : return await successofstringCase(value);
                    case {IsSuccessManyOfInt32Array: true, AsSuccessManyOfInt32Array: var value} : return await successmanyofint32arrayCase(value);
                }
                throw new ArgumentException("Union does not contain a value");
            }
            #endregion
        
            #region Switch and SwitchAsync
            public void Switch(Action<TestNamespace.Success<string>> successofstringCase,Action<TestNamespace.SuccessMany<int[]>> successmanyofint32arrayCase){
                switch (this) {
                    case {IsSuccessOfString: true, AsSuccessOfString: var value} : successofstringCase(value); return;
                    case {IsSuccessManyOfInt32Array: true, AsSuccessManyOfInt32Array: var value} : successmanyofint32arrayCase(value); return;
                }
                throw new ArgumentException("Union does not contain a value");
            }
        
            public async Task SwitchAsync(Func<TestNamespace.Success<string>, Task> successofstringCase,Func<TestNamespace.SuccessMany<int[]>, Task> successmanyofint32arrayCase){
                switch (this) {
                    case {IsSuccessOfString: true, AsSuccessOfString: var value} : await successofstringCase(value); return;
                    case {IsSuccessManyOfInt32Array: true, AsSuccessManyOfInt32Array: var value} : await successmanyofint32arrayCase(value); return;
                }
                throw new ArgumentException("Union does not contain a value");
            }
            #endregion
        
        }
        """;
    #endregion
}
