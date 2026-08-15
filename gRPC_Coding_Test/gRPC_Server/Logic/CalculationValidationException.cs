namespace gRPC_Server.Logic;

public sealed class CalculationValidationException : Exception
{
    public CalculationValidationException(string _message)
        : base(_message)
    {
    }
}