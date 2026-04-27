// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Unions;

namespace CodeOfChaosTests.Unions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RefUnionTests {
    [Test]
    public async Task Test_SingleTypeRefUnion_Success() {
        // Arrange
        RefUnion<int> union = 42;

        // Act
        bool isT0 = union.IsT0;
        object? value = union.Value;

        // Assert
        await Assert.That(isT0).IsTrue();
        await Assert.That(value).IsEqualTo(42);
    }

    [Test]
    public async Task Test_DoubleTypeRefUnion_FirstType() {
        // Arrange
        RefUnion<int, double> union = 42;

        // Act
        bool isT0 = union.IsT0;
        bool isT1 = union.IsT1;
        object? value = union.Value;

        // Assert
        await Assert.That(isT0).IsTrue();
        await Assert.That(isT1).IsFalse();
        await Assert.That(value).IsEqualTo(42);
    }

    [Test]
    public async Task Test_DoubleTypeRefUnion_SecondType() {
        // Arrange
        RefUnion<int, double> union = 3.5;

        // Act
        bool isT0 = union.IsT0;
        bool isT1 = union.IsT1;
        object? value = union.Value;

        // Assert
        await Assert.That(isT0).IsFalse();
        await Assert.That(isT1).IsTrue();
        await Assert.That(value).IsEqualTo(3.5);
    }

    [Test]
    public async Task Test_TripleTypeRefUnion_ThirdType() {
        // Arrange
        RefUnion<int, double, bool> union = true;

        // Act
        bool isT0 = union.IsT0;
        bool isT1 = union.IsT1;
        bool isT2 = union.IsT2;
        object? value = union.Value;

        // Assert
        await Assert.That(isT0).IsFalse();
        await Assert.That(isT1).IsFalse();
        await Assert.That(isT2).IsTrue();
        await Assert.That((bool)(value ?? throw new InvalidOperationException())).IsTrue();
    }

    [Test]
    public async Task Test_TryGetAsExactType_FirstType() {
        // Arrange
        RefUnion<int, double> union = 123;

        // Act
        bool result = union.TryGetAsT0(out int value);

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(value).IsEqualTo(123);
    }

    [Test]
    public async Task Test_TryGetAsExactType_InvalidType() {
        // Arrange
        RefUnion<int, double> union = 99.9;

        // Act
        bool result = union.TryGetAsT0(out int _);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Test_RefTypeMatch_SimpleResult() {
        // Arrange
        RefUnion<int, double, bool> union1 = 21;
        RefUnion<int, double, bool> union2 = 15.5;
        RefUnion<int, double, bool> union3 = false;

        // Act
        int result1 = union1.Match(
            t0Case: t0 => t0 + 1,
            t1Case: t1 => (int)t1,
            t2Case: t2 => t2 ? 1 : 0
        );

        int result2 = union2.Match(
            t0Case: t0 => t0 + 1,
            t1Case: t1 => (int)t1,
            t2Case: t2 => t2 ? 1 : 0
        );

        int result3 = union3.Match(
            t0Case: t0 => t0 + 1,
            t1Case: t1 => (int)t1,
            t2Case: t2 => t2 ? 1 : 0
        );

        // Assert
        await Assert.That(result1).IsEqualTo(22);
        await Assert.That(result2).IsEqualTo(15);
        await Assert.That(result3).IsEqualTo(0);
    }

    [Test]
    public async Task Test_MatchAsync_SimpleResult() {
        // Arrange
        RefUnion<int, double, bool> union = true;

        // Act
        int result = await union.MatchAsync(
            t0Case: async t0 => await Task.FromResult(t0 * 2),
            t1Case: async t1 => await Task.FromResult((int)t1),
            t2Case: async t2 => await Task.FromResult(t2 ? 100 : -100)
        );

        // Assert
        await Assert.That(result).IsEqualTo(100);
    }
}
