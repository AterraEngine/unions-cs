// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Tests.CodeOfChaos.Unions.Lib;

namespace Tests.CodeOfChaos.Unions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UnionInterfaceTests {
    [Test]
    public async Task UnionSecondInterface_ShouldCreateMembers() {
        // Arrange & Act
        UnionSecondInterface unionIntValue = 1;
        UnionSecondInterface unionStringValue = "Hello";
        
        // Assert
        await Assert.That(unionStringValue.Value).IsTypeOf<string>().And.IsEqualTo("Hello");
        await Assert.That(unionIntValue.Value).IsTypeOf<int>().And.IsEqualTo(1);
    }
}

