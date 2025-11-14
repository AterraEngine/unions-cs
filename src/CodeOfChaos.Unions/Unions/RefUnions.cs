// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases("T0")]
public sealed partial record RefUnion<T0> 
    : IUnion<T0>;

[UnionAliases("T0", "T1")]
public sealed partial record RefUnion<T0, T1> 
    : IUnion<T0, T1>;

[UnionAliases("T0", "T1", "T2")]
public sealed partial record RefUnion<T0, T1, T2>
    : IUnion<T0, T1, T2>;

[UnionAliases("T0", "T1", "T2", "T3")]
public sealed partial record RefUnion<T0, T1, T2, T3>
    : IUnion<T0, T1, T2, T3>;

[UnionAliases("T0", "T1", "T2", "T3", "T4")]
public sealed partial record RefUnion<T0, T1, T2, T3, T4>
    : IUnion<T0, T1, T2, T3, T4>;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5")]
public sealed partial record RefUnion<T0, T1, T2, T3, T4, T5>
    : IUnion<T0, T1, T2, T3, T4, T5>;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6")]
public sealed partial record RefUnion<T0, T1, T2, T3, T4, T5, T6>
    : IUnion<T0, T1, T2, T3, T4, T5, T6>;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7")]
public sealed partial record RefUnion<T0, T1, T2, T3, T4, T5, T6, T7>
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7>;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8")]
public sealed partial record RefUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8>;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9")]
public sealed partial record RefUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10")]
public sealed partial record RefUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11")]
public sealed partial record RefUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12")]
public sealed partial record RefUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12", "T13")]
public sealed partial record RefUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12", "T13", "T14")]
public sealed partial record RefUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>;

[UnionAliases("T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12", "T13", "T14", "T15")]
public sealed partial record RefUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    : IUnion<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>;
