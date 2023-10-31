using Calculator.Domain.Constants;
using System.Collections;
using System.Text.Json;

namespace Calculator.Domain.Models.Response;

public class OperationResponse
{
    public bool Status { get; set; }
    public int ResponseCode { get; set; }
    public string? ResponseMessage { get; set; }
    public object? ResponseData { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this);

    public static OperationResponse SuccessResponse(object? data)
    {
        return new OperationResponse
        {
            Status = true,
            ResponseData = data,
            ResponseCode = ApiStatusConstants.OK
        };
    }

    public static OperationResponse CreatedResponse(int? data)
    {
        if (data > 0)
        {
            return new OperationResponse
            {
                Status = true,
                ResponseData = data,
                ResponseCode = ApiStatusConstants.Created
            };
        }
        return BadRequestResponse(data);
    }
    public static OperationResponse CreatedResponse(object? data)
    {
        if (data != null)
        {
            return new OperationResponse
            {
                Status = true,
                ResponseData = data,
                ResponseCode = ApiStatusConstants.Created
            };
        }
        return BadRequestResponse(data);
    }

    public static OperationResponse ErrorResponse(string errorMessage)
    {
        return new OperationResponse
        {
            Status = false,
            ResponseMessage = errorMessage,
            ResponseCode = ApiStatusConstants.InternalServerError,
            ResponseData = errorMessage
        };
    }

    public static OperationResponse ExceptionResponse(string? errorMessage = "")
    {
        return new OperationResponse
        {
            Status = false,
            ResponseCode = ApiStatusConstants.InternalServerError,
            ResponseMessage = errorMessage
        };
    }

    public static OperationResponse BadRequestResponse(object data)
    {
        return new OperationResponse
        {
            Status = false,
            ResponseData = data,
            ResponseCode = ApiStatusConstants.BadRequest
        };
    }

    public static OperationResponse CustomResponse(bool isSuccess, object? data = null, string? message = "")
    {
        if (isSuccess)
        {
            return SuccessResponse(data);
        }
        return ErrorResponse(string.IsNullOrWhiteSpace(message) ? "Operation Failed" : message);
    }

    public static OperationResponse RecordExistsConflictResponse(object data, string msg = "")
    {
        return new OperationResponse
        {
            Status = false,
            ResponseCode = ApiStatusConstants.Conflict,
            ResponseMessage = !string.IsNullOrEmpty(msg) ? msg : "Information Already Exists",
            ResponseData = data
        };
    }
    public static OperationResponse CustomObjectExistsResponse(object data)
    {
        return new OperationResponse
        {
            Status = false,
            ResponseCode = ApiStatusConstants.Conflict,
            ResponseMessage = "Information Already Exists",
            ResponseData = data
        };
    }

    public static OperationResponse CustomExistsResponse(ICollection? data = null)
    {

        bool isSuccess = data?.Count > 0;
        int statusCode = isSuccess ? ApiStatusConstants.OK : ApiStatusConstants.NotFound;

        return new OperationResponse
        {
            Status = isSuccess,
            ResponseCode = statusCode,
            ResponseMessage = isSuccess ? "Record(s) Found" : "No record(s) found",
            ResponseData = data,
        };
    }

    public static OperationResponse CustomExistsResponse<T>(ICollection<T>? data)
    {
        bool isSuccess = data?.Count > 0;
        int statusCode = isSuccess ? ApiStatusConstants.OK : ApiStatusConstants.NotFound;

        return new OperationResponse
        {
            Status = isSuccess,
            ResponseCode = statusCode,
            ResponseMessage = isSuccess ? "Record(s) Found" : "No record(s) found",
            ResponseData = data,
        };
    }

    public static OperationResponse CustomExistsResponse<T>(T? data)
    {
        bool isSuccess = data is not null;
        int statusCode = isSuccess ? ApiStatusConstants.OK : ApiStatusConstants.NotFound;

        return new OperationResponse
        {
            Status = isSuccess,
            ResponseCode = statusCode,
            ResponseMessage = isSuccess ? "Record(s) Found" : "No record(s) found",
            ResponseData = data,
        };
    }

    public static OperationResponse CustomExistsResponse<T>(T? data, string errMessage = "")
    {
        bool isSuccess = data is not null;
        int statusCode = isSuccess ? ApiStatusConstants.OK : ApiStatusConstants.NotFound;

        return new OperationResponse
        {
            Status = isSuccess,
            ResponseCode = statusCode,
            ResponseMessage = string.IsNullOrEmpty(errMessage) ? isSuccess ? "Record(s) Found" : "No record(s) found" : errMessage,
            ResponseData = data,
        };
    }

    public static OperationResponse CustomExistsResponse<T>(List<T>? data)
    {
        bool isSuccess = data.Count > 0;
        return GetFoundResponse(data, isSuccess);
    }

    public static OperationResponse CustomExistsResponse<T>(IEnumerable<T>? data)
    {
        var newData = data?.ToList();
        bool isSuccess = newData?.ToList().Count > 0;
        return GetFoundResponse(newData, isSuccess);
    }

    private static OperationResponse GetFoundResponse<T>(List<T>? newData, bool isSuccess)
    {
        int statusCode = isSuccess ? ApiStatusConstants.OK : ApiStatusConstants.NotFound;

        return new OperationResponse
        {
            Status = isSuccess,
            ResponseCode = statusCode,
            ResponseMessage = isSuccess ? "Record(s) Found" : "No record(s) found",
            ResponseData = newData,
        };
    }
}

