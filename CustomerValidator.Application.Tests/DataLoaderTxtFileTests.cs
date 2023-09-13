using CustomerValidator.Application.Interfaces;
using CustomerValidator.Application.Services;
using CustomerValidator.Domain;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CustomerValidator.Application.Tests;

public class DataLoaderTxtFileTests
{
    private readonly IDataLoader _dataLoader;
    private readonly string _correctDataFilePath;
    private readonly string _incorrectDataFilePath;
    private readonly List<CustomerAccount> _customerAccounts;
    public DataLoaderTxtFileTests()
    {
        var services = new ServiceCollection()
            .AddSingleton<IDataLoader, DataLoaderTxtFile>()
            .BuildServiceProvider();

        var basePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));

        _correctDataFilePath = $@"{basePath}TestData\CustomerAccountTxtFile.txt";
        _incorrectDataFilePath = $@"{basePath}TestData\IncorrectFileFormatCustomerAccountTxtFile.txt";

        _customerAccounts = new()
        {
            new("Thomas", "3299997"),
            new("Richard", "3293982"),
            new("Rob", "3113902p")
        };

        _dataLoader = services.GetRequiredService<IDataLoader>();
    }

    [Fact]
    public async void DataLoader_LoadCustomerAccounts_ReturnsCustomerAccounts()
    {
        // Arrange & Act
        var actual = await _dataLoader.LoadCustomerAccounts(_correctDataFilePath);

        // Assert
        actual.Should().BeEquivalentTo(_customerAccounts);
    }

    [Fact]
    public async void DataLoader_LoadCustomerAccounts_ThrowFormatException()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => _dataLoader.LoadCustomerAccounts(_incorrectDataFilePath));
    }
}
