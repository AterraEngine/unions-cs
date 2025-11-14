// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Unions;

namespace Tests.CodeOfChaos.Unions.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class FalseTest {
    [Test]
    public async Task False_ToBool() {
        // Arrange
        bool value = new False(); 
        
        // Act &  Assert
        await Assert.That(value).IsFalse();
    }

    [Test]
    public async Task Bool_ToFalse() {
        // Arrange
        False value = false; 
        
        // Act &  Assert
        await Assert.That(value).IsDefault();
    }
    
    [Test]
    public async Task True_ToFalse_Fails() {
        await Assert.ThrowsAsync<InvalidOperationException>(() => {
            False value = true;
            return Task.FromResult(value);
        });
    }
}
