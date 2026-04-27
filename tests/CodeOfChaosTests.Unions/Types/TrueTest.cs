// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Unions;

namespace CodeOfChaosTests.Unions.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TrueTest {
    [Test]
    public async Task True_ToBool() {
        // Arrange
        bool value = new True(); 
        
        // Act &  Assert
        await Assert.That(value).IsTrue();
    }

    [Test]
    public async Task Bool_ToTrue() {
        // Arrange
        True value = true; 
        
        // Act &  Assert
        await Assert.That(value).IsDefault();
    }
    
    [Test]
    public async Task False_ToTrue_Fails() {
        await Assert.ThrowsAsync<InvalidOperationException>(() => {
            True value = false;
            return Task.FromResult(value);
        });
    }
}
