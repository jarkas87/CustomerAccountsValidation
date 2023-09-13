
namespace CustomerValidator.Domain;

public record CustomerAccount(string FirstName, string AccountNumber)
{
    public override string ToString()
    {
        return $"{FirstName} {AccountNumber}";
    }
}
