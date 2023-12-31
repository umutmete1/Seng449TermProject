using System.Text.Json.Serialization;

namespace TermProject.models;

public class ErrorResponse
{
    [JsonPropertyName("error")]
    public ErrorResponseDetail? Error { get; set; }

    public ErrorResponse()
    {
    }
    
    public ErrorResponse(int status, string message)
    {
        Error = new ErrorResponseDetail()
        {
            Status = status,
            Message = message
        };
    }
    
    public static ErrorResponse Return(int status, string message)
    {
        return new ErrorResponse(status, message);
    }
}

public class SuccessResponse : ErrorResponse
{
    public SuccessResponse()
    {
    }
    
    public SuccessResponse(int status, string message)
    {
        Error = new ErrorResponseDetail()
        {
            Status = status,
            Message = message
        };
    }
    
    public static SuccessResponse Return(int status, string message)
    {
        return new SuccessResponse(status, message);
    }
    
    public static SuccessResponse Return()
    {
        return new SuccessResponse();
    }
    public static SuccessResponse Return<T>(T data)
    {
        return new SuccessResponse<T>(data);
    }
}

public class SuccessResponse<T> : SuccessResponse
{
    public T? Data { get; set; }

    public SuccessResponse()
    {
    }
    
    public SuccessResponse(int status, string message)
    {
        Error = new ErrorResponseDetail()
        {
            Status = status,
            Message = message
        };
    }
    
    public SuccessResponse(T data)
    {
        Data = data;
    }
    
    public static SuccessResponse<T> Return(int status, string message)
    {
        return new SuccessResponse<T>(status, message);
    }
    
    public static SuccessResponse<T> Return(T data)
    {
        return new SuccessResponse<T>(data);
    }
}

public class ErrorResponseDetail
{
    [JsonPropertyName("status")]
    public int Status { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
}

public class CommandStatusResponse
{
    private CommandStatusResponse(CommandStatus commandStatus, string? message = null)
    {
        Status = commandStatus;
        Message = message;
    }

    /// <summary>
    /// Required. Status 
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CommandStatus Status { get; set; }
    
    public string? Message { get; set; }
    
    public static CommandStatusResponse Accepted()
    {
        return new CommandStatusResponse(CommandStatus.Accepted);
    }
    
    public static CommandStatusResponse Rejected(string? message = null)
    {
        return new CommandStatusResponse(CommandStatus.Rejected, message);
    }
}

public enum CommandStatus
{
    /// <summary>
    /// Command will be executed
    /// </summary>
    Accepted,

    /// <summary>
    /// Command will not be executed
    /// </summary>
    Rejected
};