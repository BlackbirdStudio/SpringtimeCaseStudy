namespace CaseStudy.EFCore.Models
{
    public record ContactPerson(
        string? FirstName,
        string? LastName,
        string? Phone,
        string? Mail
        ) : BaseEntity
    {
    }
}
