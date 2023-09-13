using CustomerValidator.Application.Interfaces;
using CustomerValidator.Application.Logging;
using CustomerValidator.Application.Services;
using CustomerValidator.Application.Validators;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
ILogConfiguration _logConfiguration = new LogConfigurationTxtFile();

// Add services to the container.

builder.Services.AddSingleton<ICustomerAccountValidator, CustomerAccountValidator>();
builder.Services.AddSingleton<IDataLoader, DataLoaderTxtFile>();
builder.Services.AddSingleton<IDataValidator, DataValidator>();
builder.Services.AddLogging(builder => FileLogger.BuildLogger(_logConfiguration.LogFilePath, builder));

builder.Services.AddSwaggerGen(c => {
c.SwaggerDoc("v1", new OpenApiInfo {
    Version = "v1",  
    Title = "Customer Validation API",  
    Description = "ASP.NET Core Web API",   
    Contact = new OpenApiContact { Name = "Jaroslavas" },
    });  
});


builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Validation API V1");
});

app.Run();
