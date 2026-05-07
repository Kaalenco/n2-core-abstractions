using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace N2.Core;

[TestClass]
public sealed class WithValidationResult
{
    [TestMethod]
    public void DefaultCtorSetsEmptyStringsAndNoneErrorCode()
    {
        ValidationResult result = new();
        Assert.AreEqual(string.Empty, result.Message);
        Assert.AreEqual(string.Empty, result.TypeName);
        Assert.AreEqual(ErrorCode.None, result.ErrorCode);
    }

    [TestMethod]
    public void PropertiesCanBeSet()
    {
        ValidationResult result = new()
        {
            Message = "Value is required",
            TypeName = "UserModel",
            ErrorCode = ErrorCode.ValueNullOrEmpty
        };

        Assert.AreEqual("Value is required", result.Message);
        Assert.AreEqual("UserModel", result.TypeName);
        Assert.AreEqual(ErrorCode.ValueNullOrEmpty, result.ErrorCode);
    }
}
