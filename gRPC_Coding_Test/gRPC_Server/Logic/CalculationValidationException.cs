namespace gRPC_Server.Logic;

/// <summary>
/// Represents invalid input or an unsupported calculation request.
/// </summary>
public sealed class CalculationValidationException : Exception
{    
    public CalculationValidationException(string _message)
        : base(_message)
    {
    }
}
