using CustomerValidator.Application.Interfaces;
using CustomerValidator.Application.Services;
using CustomerValidator.Application.Validators;
using CustomerValidator.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CustomerValidator.WebApi.Tests;

public class CustomerAccountValidationControllerTests
{
    private readonly CustomerAccountValidationController _customerAccountValidationController;
    public CustomerAccountValidationControllerTests()
    {
        var services = new ServiceCollection()
            .AddSingleton<ICustomerAccountValidator, CustomerAccountValidator>()
            .AddSingleton<IDataLoader, DataLoaderTxtFile>()
            .AddSingleton<IDataValidator, DataValidator>()
            .BuildServiceProvider();

        var dataValidator = services.GetRequiredService<IDataValidator>();
        var dataLoader = services.GetRequiredService<IDataLoader>();
        var logger = new Mock<ILogger<CustomerAccountValidationController>>();

        _customerAccountValidationController = new(dataValidator, dataLoader, logger.Object);
    }

    [Fact]
    public async void PostCustomerAccountValidation_WithEmptyFile_ReturnsBadRequest()
    {
        // Arrange
        var formFile = CreateFormFile("testFile.txt", string.Empty);

        // Act
        var actual = await _customerAccountValidationController.FileImport(formFile);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
    }

    [Theory]
    [InlineData(".pdf")]
    [InlineData(".xls")]
    [InlineData(".zip")]
    [InlineData(".csv")]
    [InlineData(".text")]
    [InlineData(".xml")]
    [InlineData(".json")]
    public async void PostCustomerAccountValidation_WithInvalidFileExtension_ReturnsBadRequest
        (string fileExtension)
    {
        // Arrange
        var formFile = CreateFormFile($"testFile{fileExtension}", string.Empty);

        // Act
        var actual = await _customerAccountValidationController.FileImport(formFile);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
    }

    [Fact]
    public async void PostCustomerAccountValidation_WithInvalidFileDataFormat_ReturnsBadRequest()
    {
        // Arrange
        var formFile = CreateFormFile("testFile.txt", "Thomas 3299997\nwrong_data\nRob 3113902p");

        // Act
        var actual = await _customerAccountValidationController.FileImport(formFile);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
    }

    [Fact]
    public async void PostCustomerAccountValidation_WithValidFileDataFormat_ReturnsOkRequest()
    {
        // Arrange
        var formFile = CreateFormFile("testFile.txt", "Thomas 3299997\nRob 3113902p");

        // Act
        var actual = await _customerAccountValidationController.FileImport(formFile);

        // Assert
        Assert.IsType<OkObjectResult>(actual);
    }



    private static IFormFile CreateFormFile(string fileName, string fileContent)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(fileContent);
        writer.Flush();
        stream.Position = 0;
        
        return new FormFile(stream, 0, stream.Length, "file_form", fileName);
    }
}

