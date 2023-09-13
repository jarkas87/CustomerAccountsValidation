
namespace CustomerValidator.Domain;

public record CustomerAccountsValidationResult(bool FileValid, IEnumerable<string> InvalidLines);


