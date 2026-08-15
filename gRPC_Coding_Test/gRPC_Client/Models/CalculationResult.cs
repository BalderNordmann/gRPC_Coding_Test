namespace gRPC_Client.Models;

public sealed class CalculationResult
{
    public CalculationResult(string _message)
    {
        Message = _message;
    }

    public string Message { get; }
}