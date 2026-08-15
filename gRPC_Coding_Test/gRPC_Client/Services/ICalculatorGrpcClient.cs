using gRPC_Client.Models;

namespace gRPC_Client.Services;

public interface ICalculatorGrpcClient
{
    Task<double> CalculateAsync(CalculationInput _input);
}
