using Calculator.Domain.Constants;
using Calculator.Domain.Models.Response;

namespace Calculator.Infrastructure.Extensions;

public static class OperationResponseExtension
{
    public static OperationResponse GetSpCreateResponse<T>(this int result) where T : class
    {
        return (result > 0).GetSpCreateResponse<T>();
    }

    public static OperationResponse GetSpCreateResponse<T>(this bool result) where T : class
    {
        OperationResponse response = new();
        if (result)
        {
            response.Status = true;
            response.ResponseCode = ApiStatusConstants.Created;
            response.ResponseMessage = $"{GetClassName<T>()} Successfully Created";
        }
        else
        {
            response.ResponseMessage = $"Failure Creating {GetClassName<T>()}";
            response.ResponseCode = ApiStatusConstants.BadRequest;
        }
        return response;
    }

    private static string GetClassName<T>() where T : class
    {
        string fullName = typeof(T).FullName!;
        if (!string.IsNullOrWhiteSpace(fullName))
        {
            int lastDotIndex = fullName.LastIndexOf('.');
            if (lastDotIndex >= 0)
            {
                return fullName[(lastDotIndex + 1)..];
            }
        }
        return fullName!;
    }
}
