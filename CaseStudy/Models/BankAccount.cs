namespace CaseStudy.Models
{
    public record BankAccount(
        string? Iban,
        string? Bic,
        string? Name
    );
}
