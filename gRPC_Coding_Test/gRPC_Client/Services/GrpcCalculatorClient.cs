using Grpc.Net.Client;
using GrpcCodingTest.Contracts;
using gRPC_Client.Models;

namespace gRPC_Client.Services;

/// <summary>
/// Sends calculator requests to the gRPC server.
/// </summary>
public sealed class GrpcCalculatorClient : ICalculatorGrpcClient
{
    private const string serverAddress = "https://localhost:7042";

    /// <summary>
    /// Converts the client model to a protobuf request and returns the server result.
    /// </summary>
    public async Task<double> CalculateAsync(CalculationInput _input)
    {
        using var channel = GrpcChannel.ForAddress(serverAddress);
        var client = new CalculatorGrpc.CalculatorGrpcClient(channel);
        var response = await client.CalculateAsync(new CalculationRequest
        {
            LeftOperand = _input.LeftOperand,
            RightOperand = _input.RightOperand,
            Operation = _input.Operation
        });

        return response.Result;
    }
}
