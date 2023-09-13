
namespace CustomerValidator.Application.Interfaces;

public interface ICustomerAccountValidator
{
    bool ValidateCustomerAccountName(string customerAccountName);
    bool ValidateCustomerAccountNumber(string customerAccountNumber);
}
