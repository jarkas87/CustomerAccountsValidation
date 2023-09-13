using CustomerValidator.Application.Interfaces;
using CustomerValidator.Domain;

namespace CustomerValidator.Application.Services;

public class DataLoaderTxtFile : IDataLoader
{
    public async Task<IEnumerable<CustomerAccount>> LoadCustomerAccounts(string filePath)
        => await ReadCustomerAccounts(filePath);

    public static async Task<IEnumerable<CustomerAccount>> ReadCustomerAccounts(string filePath)
    {
        var customerAccounts = new List<CustomerAccount>();
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using (var reader = new StreamReader(fileStream))
        {
            var line = string.Empty;
            var lineNumber = 1;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var customerAccountArguments = line.Trim().Split(' ');
                if (customerAccountArguments?.Length != 2)
                {
                    throw new FormatException($"Incorrect customer account format at line {lineNumber} ({line})." +
                        $"Format should be 'AccountName AccountNumber'.");
                }
                customerAccounts.Add(new(customerAccountArguments[0], customerAccountArguments[1]));
                lineNumber++;
            }
        }

        return customerAccounts;
    }
}
