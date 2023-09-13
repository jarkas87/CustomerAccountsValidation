
using CustomerValidator.Domain;

namespace CustomerValidator.Application.Interfaces;

public interface IDataLoader
{
    Task<IEnumerable<CustomerAccount>> LoadCustomerAccounts(string filePath);
}

