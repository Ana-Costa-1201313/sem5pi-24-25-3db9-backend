using System;
using Backoffice.Domain.OperationTypes;
using Backoffice.Domain.Shared;
using Xunit;

namespace Backoffice.Tests;
public class OperationTypeNameTests
{
    [Fact]
    public void TestOperationTypeNameWithValidInputs()
    {
        var validName = "Surgery A";

        var operationTypeName = new OperationTypeName(validName);

        Assert.Equal(validName, operationTypeName.Name);
    }

    [Fact]
    public void TestOperationTypeNameWithNullName()
    {
        var exception = Assert.Throws<BusinessRuleValidationException>(() => new OperationTypeName(null));

        Assert.Equal("Error: The operation type name can't be null, empty or consist in only white spaces.", exception.Message);
    }

    [Fact]
    public void TestOperationTypeNameWithEmptyName()
    {
        var exception = Assert.Throws<BusinessRuleValidationException>(() => new OperationTypeName(""));

        Assert.Equal("Error: The operation type name can't be null, empty or consist in only white spaces.", exception.Message);
    }

    [Fact]
    public void TestOperationTypeNameWithWhiteSpacesName()
    {
        var exception = Assert.Throws<BusinessRuleValidationException>(() => new OperationTypeName("   "));

        Assert.Equal("Error: The operation type name can't be null, empty or consist in only white spaces.", exception.Message);
    }
}
