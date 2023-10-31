namespace Calculator.Infrastructure.Validation
{
    public class ValidationServiceFactory : IValidationServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IValidationService<T> GetValidationService<T>()
        {
            return (IValidationService<T>)_serviceProvider.GetService(typeof(IValidationService<T>))!;
        }
    }

}
