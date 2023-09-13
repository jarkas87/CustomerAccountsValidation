using CustomerValidator.Application.Interfaces;
using CustomerValidator.Application.Validators;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CustomerValidator.Application.Tests;

public class CustomerAccountValidatorTests
{
    private readonly ICustomerAccountValidator _customerAccountValidator;
    public CustomerAccountValidatorTests()
    {
        var services = new ServiceCollection()
            .AddSingleton<ICustomerAccountValidator, CustomerAccountValidator>()
            .BuildServiceProvider();

        _customerAccountValidator = services.GetRequiredService<ICustomerAccountValidator>();
    }

    [Theory]
    [InlineData("Thomas", true)]
    [InlineData("XAEA-12", false)]
    [InlineData("michael", false)]
    [InlineData("Michelin1", false)]
    [InlineData("Richard", true)]
    public void CustomerAccountValidator_ValidateCustomerAccountName_ReturnsExpectedValidationResult
        (string accountName, bool expectedResult)
    {
        // Arrange & Act
        var actual = _customerAccountValidator.ValidateCustomerAccountName(accountName);

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("32999921", false)]
    [InlineData("8293982", false)]
    [InlineData("329a982", false)]
    [InlineData("329398", false)]
    [InlineData("3293982", true)]
    [InlineData("3113902", true)]
    [InlineData("3113902p", true)]
    public void CustomerAccountValidator_ValidateCustomerAccountNumber_ReturnsExpectedValidationResult
        (string accountNumber, bool expectedResult)
    {
        // Arrange & Act
        var actual = _customerAccountValidator.ValidateCustomerAccountNumber(accountNumber);

        // Assert
        actual.Should().Be(expectedResult);
    }
}
