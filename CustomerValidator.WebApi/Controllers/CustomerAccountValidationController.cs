using CustomerValidator.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CustomerValidator.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerAccountValidationController : Controller
{
    private readonly IDataValidator _dataValidator;
    private readonly IDataLoader _dataLoader;
    private readonly ILogger _logger;

    public CustomerAccountValidationController(IDataValidator dataValidator,
        IDataLoader dataLoader,
        ILogger<CustomerAccountValidationController> logger)
    {
        _dataValidator = dataValidator;
        _dataLoader = dataLoader;
        _logger = logger;
    }

    [HttpPost]
    [Route("validate")]
    public async Task<IActionResult> FileImport(IFormFile FormFile)
    {
        try
        {
            if (FormFile == null || FormFile.Length == 0)
            {
                _logger.LogInformation("The file is empty.");
                return BadRequest("The file is empty.");
            }

            if (Path.GetExtension(FormFile.FileName) != ".txt")
            {
                _logger.LogInformation("Invalid file extension. Only .txt files are supported.");
                return BadRequest("Invalid file extension. Only .txt files are supported.");
            }

            var filesUploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            if (!Directory.Exists(filesUploadDirectory))
            {
                Directory.CreateDirectory(filesUploadDirectory);
            }

            var filePath = Path.Combine(filesUploadDirectory, FormFile.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await FormFile.CopyToAsync(fileStream);
            }
            
            var customerAccounts = await _dataLoader.LoadCustomerAccounts(filePath);
            var validationResult = await _dataValidator.ValidateCustomerAccounts(customerAccounts);

            if (validationResult.FileValid)
            {
                return Ok(JsonConvert.SerializeObject(new { fileValid = validationResult.FileValid }));
            }
            else
            {
                return Ok(JsonConvert.SerializeObject(new
                {
                    fileValid = validationResult.FileValid,
                    invalidLines = validationResult.InvalidLines
                }));
            }
        }
        catch(Exception ex)
        { 
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }

    } 
}

