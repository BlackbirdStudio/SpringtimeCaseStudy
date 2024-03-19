namespace CaseStudy.EFCore.Models
{
    public record BankAccount(
        string? Iban,
        string? Bic,
        string? Name
        ) : BaseEntity {
    }
}
