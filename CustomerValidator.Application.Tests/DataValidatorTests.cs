using CustomerValidator.Application.Interfaces;
using CustomerValidator.Application.Services;
using CustomerValidator.Application.Validators;
using CustomerValidator.Domain;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CustomerValidator.Application.Tests;

public class DataValidatorTests
{
    private readonly IDataValidator _dataValidator;
    private readonly List<CustomerAccount> _validCustomerAccounts;
    private readonly List<CustomerAccount> _invalidCustomerAccounts;
    public DataValidatorTests()
    {
        var services = new ServiceCollection()
        .AddSingleton<ICustomerAccountValidator, CustomerAccountValidator>()
        .AddSingleton<IDataValidator, DataValidator>()
        .BuildServiceProvider();

        _dataValidator = services.GetRequiredService<IDataValidator>();

        _validCustomerAccounts = new()
        {
            new("Richard", "3293982"),
            new("Rob", "3113902p"),
            new("Jack", "4513902p"),
            new("James", "3841247"),
        };

        _invalidCustomerAccounts = new()
        {
            new("Thomas", "32999921"),
            new("Rose", "329a982"),
            new("Bob", "451397702p"),
            new("michael", "3113902"),
            new("XAEA-12", "8293982")
        };
    }

    [Fact]
    public async void DataValidator_ValidateCustomerAccounts_ReturnsIsValidTrueAndInvalidAccountsIsEmpty()
    {
        // Arrange & Act
        var actual = await _dataValidator.ValidateCustomerAccounts(_validCustomerAccounts);

        // Assert
        actual.FileValid.Should().BeTrue();
        actual.InvalidLines.Should().BeEmpty();
    }

    [Fact]
    public async void DataValidator_ValidateCustomerAccounts_ReturnsIsValidFalseAndInvalidAccountsIsNotEmpty()
    {
        // Arrange & Act
        var actual = await _dataValidator.ValidateCustomerAccounts(_invalidCustomerAccounts);

        // Assert
        actual.FileValid.Should().BeFalse();
        actual.InvalidLines.Should().NotBeEmpty();
    }

}
