namespace gRPC_Client.Models;

/// <summary>
/// Stores the message displayed after a calculation attempt.
/// </summary>
public sealed class CalculationResult
{    
    public CalculationResult(string _message)
    {
        Message = _message;
    }

    public string Message { get; }
}
