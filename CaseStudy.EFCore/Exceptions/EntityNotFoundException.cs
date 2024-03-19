namespace CaseStudy.EFCore.Exceptions
{
    public class EntityNotFoundException(int Id, Type Type): Exception
    {
        public override string Message => $"Entity of type {Type.FullName} with id {Id} was not found.";
    }
}
