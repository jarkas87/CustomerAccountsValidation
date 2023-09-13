
using CustomerValidator.Domain;

namespace CustomerValidator.Application.Interfaces;

public interface IDataValidator
{
    Task<CustomerAccountsValidationResult> ValidateCustomerAccounts(IEnumerable<CustomerAccount> customerAccounts);
}

