namespace Calculator.Infrastructure.Validation
{
    public interface IValidationServiceFactory
    {
        IValidationService<T> GetValidationService<T>();
    }
}
