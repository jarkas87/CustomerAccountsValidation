
using CustomerValidator.Application.Interfaces;
using CustomerValidator.Domain;
using System.Security.Cryptography.X509Certificates;

namespace CustomerValidator.Application.Services;

public class DataValidator : IDataValidator
{
    private readonly ICustomerAccountValidator _customerAccountValidator;

    public DataValidator(ICustomerAccountValidator customerAccountValidator)
    {
        _customerAccountValidator = customerAccountValidator;
    }

    public Task<CustomerAccountsValidationResult> ValidateCustomerAccounts
        (IEnumerable<CustomerAccount> customerAccounts)
    {
        var invalidCustomerAccounts = new List<string>();
        var lineNumber = 1;
        foreach (var customerAccount in customerAccounts)
        {
            var validatiomMessage = string.Empty;
            var isAccountNameMatched =
                _customerAccountValidator.ValidateCustomerAccountName(customerAccount.FirstName);
            var isAccountNumberMatched =
                _customerAccountValidator.ValidateCustomerAccountNumber(customerAccount.AccountNumber);

            if (!isAccountNameMatched && !isAccountNumberMatched)
            {
                validatiomMessage = "Account name, account number";
            }
            else if (!isAccountNameMatched)
            {
                validatiomMessage = "Account name";
            }
            else if (!isAccountNumberMatched)
            {
                validatiomMessage = "Account number";
            }
            else
            {
                // No action need
            }

            if (!string.IsNullOrEmpty(validatiomMessage))
            {
                validatiomMessage = $"{validatiomMessage} - not valid for {lineNumber} line '{customerAccount}'";
                invalidCustomerAccounts.Add(validatiomMessage);
            }

            lineNumber++;
        }

        return Task.FromResult(
            new CustomerAccountsValidationResult(FileValid: !invalidCustomerAccounts.Any(), InvalidLines: invalidCustomerAccounts));
    }
}