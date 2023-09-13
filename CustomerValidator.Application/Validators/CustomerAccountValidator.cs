using CustomerValidator.Application.Interfaces;
using System.Text.RegularExpressions;

namespace CustomerValidator.Application.Validators;

public class CustomerAccountValidator : ICustomerAccountValidator
{
    public bool ValidateCustomerAccountName(string customerAccountName)
    {
        string customerAccountNameRegex = "^[A-Z]+[a-zA-Z]*$";
        Match customerAccountNameMatch = Regex.Match(customerAccountName, customerAccountNameRegex);

        return customerAccountNameMatch.Success;
    }

    public bool ValidateCustomerAccountNumber(string customerAccountNumber)
    {
        string customerAccountNumberRegex = "^[3-4][0-9]{6}[p]?$";
        Match customerAccountNumberMatch = Regex.Match(customerAccountNumber, customerAccountNumberRegex);

        return customerAccountNumberMatch.Success;
    }
}

