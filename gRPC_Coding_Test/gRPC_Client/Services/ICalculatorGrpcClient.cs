using gRPC_Client.Models;

namespace gRPC_Client.Services;

/// <summary>
/// Defines the client-side contract for requesting a calculation from the gRPC service.
/// </summary>
public interface ICalculatorGrpcClient
{
    /// <summary>
    /// Sends a calculation request and returns the numeric server response.
    /// </summary>
    Task<double> CalculateAsync(CalculationInput _input);
}
