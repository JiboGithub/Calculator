namespace Calculator.Infrastructure.Validation
{
    public interface IValidationService<T>
    {
        List<string> Validate(T model);
    }
}
